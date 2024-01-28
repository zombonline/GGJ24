using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] TextMeshProUGUI textTimer;
    [SerializeField] float startTime;
    float timeRemaining, mins, secs;
    bool soundTriggered = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        timeRemaining = startTime;
    }

    private void Update()
    {
        if(!gameManager.gameRunning) { return; }
        timeRemaining -= Time.deltaTime;
        mins = Mathf.FloorToInt(timeRemaining / 60f);
        secs = timeRemaining % 60f;
        textTimer.text = mins.ToString("00") + ":" + secs.ToString("00");
        if(timeRemaining <= 10f && !soundTriggered)
        {
            soundTriggered = true;
            FMODController.Play3DSFX("event:/Stage/Stage_Timer_Last10Seconds", transform.position);
        }

        if (timeRemaining < 0f)
        {
            textTimer.text = 0.ToString("00") + ":" + 0.ToString("00");
            timeRemaining = startTime;
            soundTriggered = false;
            gameManager.GameOver();
        }
    }
}
