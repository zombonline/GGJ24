using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameRunning = false;
    [SerializeField] UnityEvent gameStarted;

    [SerializeField] TextMeshProUGUI textPoints, textFinalPoints, textPrompter;
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
    [SerializeField] Image[] hitPointIcons;
    [SerializeField] Slider satisfactionSlider;

    Environment environment;
    FMOD.Studio.EventInstance audienceAmbience;

    private void Awake()
    {
        audienceAmbience = FMODController.StartLoopedSFX("event:/Audience/Audience_Ambience");

        environment = FindObjectOfType<Environment>();

        hitPoints = startingHitPoints;
        satisfaction = startingSatisfaction;

        jokes = jokesTextAsset.text.Split('\n');
        LoadJoke();

    }

    private void Update()
    {
        if(!gameRunning)
        {
            audienceAmbience.setParameterByName("Curtains", 0);
            return;
        }
        audienceAmbience.setParameterByName("Curtains", 1);
        satisfactionDecreaseTimer -= Time.deltaTime;
        float param = satisfaction / 100f;
        audienceAmbience.setParameterByName("Satisfaction", param);

        if (satisfactionDecreaseTimer <= 0f && satisfaction > 0)
        {
            if(satisfaction == targetSatisfaction)
            {
                environment.SetStageLightsEnabled(false);
            }
            satisfaction -= 1;
            satisfactionDecreaseTimer = satisfactionDecreaseTime;
        }
        textPoints.text = points.ToString();
        satisfactionSlider.value = (float)satisfaction / 100;

  
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
        if (hitPoints == 2) { FMODController.Play3DSFX("event:/Character/Character_Damage_01", GameObject.Find("Player").transform.position); }
        else if (hitPoints == 1) { FMODController.Play3DSFX("event:/Character/Character_Damage_02", GameObject.Find("Player").transform.position); }
        else { FMODController.Play3DSFX("event:/Character/Character_Damage_03", GameObject.Find("Player").transform.position); }

        for (int i = 0; i < hitPointIcons.Length; i++)
        {
            if (i + 1 > hitPoints)
            {
                hitPointIcons[i].gameObject.SetActive(false);
            }
        }
        if (hitPoints <= 0 && gameRunning)
        {
            hitPoints = 3;
            foreach(Image icon in hitPointIcons) { icon.gameObject.SetActive(true); }
            GameOver();
        }
    }

    public void GameOver()
    {
        environment.CloseCurtains();
        textFinalPoints.text = points.ToString();
        gameRunning = false;
        screenGameOver.SetActive(true);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
