using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    bool playerInputting = false;
    [SerializeField] TextMeshProUGUI speechText;
    SkeletonGraphic skeletonGraphic;
    GameManager gameManager;
    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(!gameManager.gameRunning && speechText.enabled)
        {
            DisableSpeechBubble();
        }
    }

    public void SetPlayerInputText(string text)
    {
        Debug.Log("speeec");
        if (!Settings.GetSpeechBubbleEnabled()) { return; }
        Invoke(nameof(DisableSpeechBubble), 4f);
        speechText.enabled = true;
        speechText.text = text;
    }

    public void ClearSpeechBubble()
    {
        StopAllCoroutines();
        playerInputting = false;
        speechText.enabled = false;
    }

    public void EnableSpeechBubble()
    {
        speechText.enabled = true;
    }

    public void DisableSpeechBubble()
    {
        ClearSpeechBubble();
    }



}
