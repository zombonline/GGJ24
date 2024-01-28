using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Settings : MonoBehaviour
{
    public static string SPEECH_BUBBLE_ENABLED = "speech bubble enabled";
    public static string VOLUME_KEY = "volume key";

    public static string activeMicrophone;
    public TMP_Dropdown microphoneDropdown;
    private void Awake()
    {
        PopulateMicrophoneDropDown();
    }
    public void SetSpeechBubbleEnabled(bool val)
    {
        PlayerPrefs.SetInt(SPEECH_BUBBLE_ENABLED, BoolToInt(val));
    }

    public static bool GetSpeechBubbleEnabled()
    {
        return IntToBool(PlayerPrefs.GetInt(SPEECH_BUBBLE_ENABLED, 1));
    }

    public void SetVolume(float val)
    {
        PlayerPrefs.SetFloat(SPEECH_BUBBLE_ENABLED, val);
    }

    public static float GetVolume()
    {
        return PlayerPrefs.GetFloat(SPEECH_BUBBLE_ENABLED, 1f);
    }
    private static int BoolToInt(bool val)
    {
        if(val == true) { return 1; }
        else { return 0; }
    }
    private static bool IntToBool(int val)
    {
        if(val == 1) { return true; }
        else { return false; }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void PopulateMicrophoneDropDown()
    {
        microphoneDropdown.ClearOptions();
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            microphoneDropdown.options.Add(new TMP_Dropdown.OptionData() { text = Microphone.devices[i] });
        }
    }

    public void SetActiveMicrophone(int index)
    {
        activeMicrophone = Microphone.devices[index];
    }

}
