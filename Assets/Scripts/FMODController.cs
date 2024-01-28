using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODController : MonoBehaviour
{
    static FMOD.Studio.EventInstance snapshotReverb;
    static List<FMOD.Studio.EventInstance> loopingInstances = new List<FMOD.Studio.EventInstance>();

    FMOD.Studio.EventInstance audienceAmbience;

    private void Awake()
    {
        snapshotReverb = RuntimeManager.CreateInstance("snapshot:/Reading");
    }

    public static void PlaySFX(string val, string param = null, int paramVal = 0)
    {

        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
        if(param == null) { return; }
        newAudioEvent.setParameterByName(param, paramVal);
    }
    public static void Play3DSFX(string val, Vector3 position, string param = null, int paramVal = 0)
    {

        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.set3DAttributes(position.To3DAttributes());
        newAudioEvent.start();
        if (param == null) { return; }
        newAudioEvent.setParameterByName(param, paramVal);
    }
    public static void PlaySFXNoParams(string val)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
    }

    public static FMOD.Studio.EventInstance StartLoopedSFX(string val)
    {
        var audioEvent = RuntimeManager.CreateInstance(val);
        audioEvent.start();
        loopingInstances.Add(audioEvent);
        return audioEvent;
    }

    public void StopLoopedSFX(string val)
    {
        foreach(var audioEvent in loopingInstances)
        {
            audioEvent.getDescription(out FMOD.Studio.EventDescription eventDesc);
            eventDesc.getPath(out string path);
            if (path == val)
            {
                audioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                loopingInstances.Remove(audioEvent);
            }
        }
    }

    public static void PlayReverbSnapshot()
    {
        snapshotReverb.start();
    }
    public static void StopReadingSnapshot()
    {
        snapshotReverb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
