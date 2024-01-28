using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    GameManager gameManager;
    Environment environment;
    [SerializeField] MicrophonePoint[] microphones;
    private void Awake()
    {
        environment = FindObjectOfType<Environment>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void EnableRandomMicrophone()
    {
        if(!gameManager.gameRunning) { return; }
        int randomIndex = Random.Range(0, microphones.Length);
        microphones[randomIndex].EnableMicrophonePoint();
        string pos = "";
        switch(randomIndex)
        {
            case 0:
                pos = "Left";
                break;
            case 1:
                pos = "Centre";
                break;
            case 2:
                pos = "Right";
                break;
        }
        environment.WobbleMicrophone(pos);
    }

}
