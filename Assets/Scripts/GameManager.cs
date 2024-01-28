using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool gameRunning = false;
    [SerializeField] UnityEvent gameStarted;

    [SerializeField] TextMeshProUGUI textPoints, textSatisfaction, textHitpoints, textPrompter;
    [SerializeField] TextAsset jokesTextAsset;
    string[] jokes;
    public string currentJoke;
    public int points = 0;
    public int hitPoints = 3;
    public int satisfaction = 50;
    [SerializeField] public int startingHitPoints, startingSatisfaction, satisfactionMaxIncrease, targetSatisfaction;
    [SerializeField] float satisfactionDecreaseTime;
    float satisfactionDecreaseTimer;

    [SerializeField] GameObject screenGameOver;


    Environment environment;

    private void Awake()
    {
        environment = FindObjectOfType<Environment>();

        hitPoints = startingHitPoints;
        satisfaction = startingSatisfaction;

        jokes = jokesTextAsset.text.Split('\n');
        LoadJoke();

    }

    private void Update()
    {
        if(!gameRunning)
        { return; }
        satisfactionDecreaseTimer -= Time.deltaTime;
        if(satisfactionDecreaseTimer <= 0f && satisfaction > 0)
        {
            if(satisfaction == targetSatisfaction)
            {
                environment.SetStageLightsEnabled(false);
            }
            satisfaction -= 1;
            satisfactionDecreaseTimer = satisfactionDecreaseTime;

        }
        textPoints.text = "POINTS: " + points.ToString();
        textSatisfaction.text = "SATISFACTION: " + satisfaction.ToString() + "%";
    }

    public void SetGameRunningState(bool val)
    {
        gameRunning = val;
        if (gameRunning) {
            hitPoints = startingHitPoints;
            points = 0;
            satisfaction = startingSatisfaction;
            gameStarted.Invoke(); }
    }

    public void DecreaseHitPoints()
    {
        hitPoints -= 1;
        textHitpoints.text = "HIT POINTS: " + hitPoints.ToString() + "/" + startingHitPoints.ToString();
        if (hitPoints <= 0 && gameRunning)
        {
            hitPoints = 3;
            GameOver();
        }
    }

    public void GameOver()
    {
        environment.CloseCurtains();
        gameRunning = false;
        Debug.Log("GameOver");
        screenGameOver.SetActive(true);
    }



    public void LoadJoke()
    {
        int randomIndex = Random.Range(0, jokes.Length);
        currentJoke = jokes[randomIndex];
        textPrompter.text = jokes[randomIndex];
    }

    public void IncreaseSatisfaction(int percentage)
    {
        satisfaction += (satisfactionMaxIncrease * percentage) / 100;
        Debug.Log(satisfactionMaxIncrease);
        Debug.Log((satisfactionMaxIncrease * percentage) / 100);
        Debug.Log(satisfaction);
        if(satisfaction > 100)
        {
            satisfaction = 100;
        }
        if(satisfaction > targetSatisfaction)
        {
            environment.SetStageLightsEnabled(true);
        }
    }


}
