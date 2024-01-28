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
    public void SetPlayerInputting(bool val)
    {
        if (!Settings.GetSpeechBubbleEnabled()) { return; }
        
        playerInputting = val;
        if (playerInputting) { StartCoroutine(PlayerInputtingRoutine()); }
    }
    IEnumerator PlayerInputtingRoutine()
    {
        EnableSpeechBubble();
        speechText.enabled = true;
        while (playerInputting)
        {
            speechText.text = ".    ";
            yield return new WaitForSeconds(.33f);
            speechText.text = ". .  ";
            yield return new WaitForSeconds(.33f);
            speechText.text = ". . .";
            yield return new WaitForSeconds(.33f);
        }
    }

    public void SetPlayerInputText(string text)
    {
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
        StartCoroutine(EnableSpeechBubbleRoutine());
    }
    IEnumerator EnableSpeechBubbleRoutine()
    {
        skeletonGraphic.AnimationState.SetAnimation(0, "Enable", false);
        yield return new WaitUntil(() => skeletonGraphic.AnimationState.GetCurrent(0).IsComplete);
        skeletonGraphic.AnimationState.SetAnimation(0, "Idle", true);
        speechText.enabled = true;
    }
    public void DisableSpeechBubble()
    {
        skeletonGraphic.AnimationState.SetAnimation(0, "Disable", false);
        ClearSpeechBubble();
    }



}
