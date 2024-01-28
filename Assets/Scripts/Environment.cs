using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    SkeletonAnimation spineAsset;
    [SerializeField] TextMeshProUGUI prompterText;

    private void Awake()
    {
        spineAsset = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        spineAsset.AnimationState.SetAnimation(0, "Curtains/Close idle", true);
        spineAsset.AnimationState.SetAnimation(2, "Audience/Idle", true);
    }

    public void CloseCurtains()
    {
        StartCoroutine(CloseCurtainsRoutine());
    }
    IEnumerator CloseCurtainsRoutine()
    {
        FMODController.PlaySFX("event:/Stage/Stage_Curtains_Close");
        prompterText.enabled = false;
        spineAsset.AnimationState.SetAnimation(0, "Curtains/Close", false);
        yield return new WaitUntil(() => spineAsset.AnimationState.GetCurrent(0).IsComplete);
        spineAsset.AnimationState.SetAnimation(0, "Curtains/Close idle", true);
    }

    public void OpenCurtains()
    {
        StartCoroutine(OpenCurtainsRoutine());
    }
    IEnumerator OpenCurtainsRoutine()
    {
        FMODController.PlaySFX("event:/Stage/Stage_Curtains_Open");
        FMODController.PlaySFX("event:/Audience/Audience_Applause");
        Debug.Log("curtains opening");
        spineAsset.AnimationState.SetAnimation(0, "Curtains/Open", false);
        yield return new WaitUntil(() => spineAsset.AnimationState.GetCurrent(0).IsComplete);
        spineAsset.AnimationState.SetAnimation(0, "Curtains/Open Idle", true);
        prompterText.enabled = true;
    }

    public void SetTelepropmterRecording(bool val)
    {
        spineAsset.AnimationState.SetAnimation(1, "Teleprompter/Recording", val);
    }

    public void SetTeleprompterFeedback(int val)
    {
        if (val > 100 / 3 * 2) { spineAsset.AnimationState.SetAnimation(1, "Teleprompter/Good", false); }
        else if (val > 100 / 3) { spineAsset.AnimationState.SetAnimation(1, "Teleprompter/Average", false); }
        else { spineAsset.AnimationState.SetAnimation(1, "Teleprompter/Poor", false); }
    }
    public void SetAudienceFeedback(bool positive)
    {
        StartCoroutine(SetAudienceFeedbackRoutine(positive));
    }

    IEnumerator SetAudienceFeedbackRoutine(bool positive)
    {
        if (positive) { spineAsset.AnimationState.SetAnimation(2, "Audience/Cheer", false); }
        else { spineAsset.AnimationState.SetAnimation(2, "Audience/Boo", false); }
        yield return new WaitUntil(() => spineAsset.AnimationState.GetCurrent(2).IsComplete);
        spineAsset.AnimationState.SetAnimation(2, "Audience/Idle", true);
    }
    public void WobbleMicrophone(string pos)
    {
        spineAsset.AnimationState.SetAnimation(3, "Microphone/" + pos, false);
    }

    public void SetStageLightsEnabled(bool val)
    {
        spineAsset.AnimationState.SetAnimation(3, "Stage Light/Stage Light", val);
    }

}
