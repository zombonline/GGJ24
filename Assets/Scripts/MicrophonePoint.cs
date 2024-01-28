using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HuggingFace.API;
using System.IO;
using Spine.Unity;

public class MicrophonePoint : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] SkeletonAnimation spotLight;
    [SerializeField] float talkTime;
    float talkTimer;

    bool canRecord = false;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    bool lightsOn;
    

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        talkTimer = talkTime;
    }   
    private void Update() {
        if(recording)
        {
            talkTimer -= Time.deltaTime;
            if(talkTimer <= 0f || Microphone.GetPosition(Settings.activeMicrophone) >= clip.samples)
            {
                StartCoroutine(FinishJoke());
            }
        }
        if(canRecord && !recording && Input.GetKeyDown(KeyCode.Space))
        {
            StartRecording();
        }
    }

    private IEnumerator FinishJoke()
    {
        StopRecording();
        canRecord = false;
        spotLight.AnimationState.SetAnimation(0, "Off", false);
        yield return new WaitUntil(() => spotLight.AnimationState.GetCurrent(0).IsComplete);
        spotLight.AnimationState.SetAnimation(0, "Idle Off", true);
        lightsOn = false;

        talkTimer = talkTime;

        yield return new WaitForSeconds(3f);
        transform.parent.GetComponent<MicrophoneManager>().EnableRandomMicrophone();
        gameManager.LoadJoke();
    }


    public void EnableMicrophonePoint()
    {
        StartCoroutine(EnableMicrophonePointRoutine());
    }

    IEnumerator EnableMicrophonePointRoutine()
    {
        lightsOn = true;
        spotLight.AnimationState.SetAnimation(0, "On", false);
        yield return new WaitUntil(() => spotLight.AnimationState.GetCurrent(0).IsComplete);
        spotLight.AnimationState.SetAnimation(0, "Idle On", true);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")&& lightsOn)
        {
            canRecord = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if(recording)
            {
                StartCoroutine(FinishJoke());
            }
        }
    }
    private void StartRecording() {
        FindObjectOfType<SpeechBubble>().SetPlayerInputting(true);
        clip = Microphone.Start(Settings.activeMicrophone, false, 4, 44100);
        recording = true;
        FindObjectOfType<Environment>().SetTelepropmterRecording(true);
    }

    private void StopRecording() {
        var position = Microphone.GetPosition(Settings.activeMicrophone);
        Microphone.End(Settings.activeMicrophone);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        FindObjectOfType<SpeechBubble>().SetPlayerInputting(false);
        SendRecording();
        FindObjectOfType<Environment>().SetTelepropmterRecording(false);
    }

    private void SendRecording() {
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            FindObjectOfType<SpeechBubble>().SetPlayerInputText(response);
            CompareText(response);
        }, error => {
            Debug.Log(error);
        });
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    private void CompareText(string inputText)
    {
        string[] words = inputText.Split(' ');
        string[] jokeWords = gameManager.currentJoke.Split(' ');
        int correctWords = 0;
        foreach(string jokeWord in jokeWords)
        {
            if (jokeWord == "_") { continue; }
            foreach(string word in words)
            {
                if (word.ToLower() == jokeWord.ToLower())
                {
                    correctWords++;
                    continue;
                }
            }
        }
        int percentage = (correctWords * 100) / jokeWords.Length;
        FindObjectOfType<Environment>().SetTeleprompterFeedback(percentage);
        FindObjectOfType<Environment>().SetAudienceFeedback(percentage > 50);
        gameManager.IncreaseSatisfaction(percentage);
    }

}
