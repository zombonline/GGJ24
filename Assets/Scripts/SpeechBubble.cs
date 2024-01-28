using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    bool playerInputting = false;
    [SerializeField] TextMeshProUGUI speechText;



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

        EnableSpeechBubble();
        Invoke(nameof(ClearSpeechBubble), 4f);
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
        speechText.enabled = false;
    }

    public void DisableSpeechBubble()
    {
        //spine anim
        ClearSpeechBubble();
    }



}
