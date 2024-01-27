using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPoints, textSatisfaction, textHitpoints;


    public static int points = 0;
    public static int hitPoints = 3;
    public static int satisfaction = 50;
    [SerializeField] int startingHitPoints, startingSatisfaction;
    [SerializeField] float satisfactionDecreaseTime;
    float satisfactionDecreaseTimer;
   

    private void Awake()
    {
        hitPoints = startingHitPoints;
        satisfaction = startingSatisfaction;
    }

    private void Update()
    {
        satisfactionDecreaseTimer -= Time.deltaTime;
        if(satisfactionDecreaseTimer <= 0f && satisfaction > 0)
        {
            satisfaction -= 1;
            satisfactionDecreaseTimer = satisfactionDecreaseTime;
        
        }
        textPoints.text = "POINTS: " + points.ToString();
        textHitpoints.text = "HIT POINTS: " + hitPoints.ToString() + "/" + startingHitPoints.ToString();
        textSatisfaction.text = "SATISFACTION: " + satisfaction.ToString() + "%";
    }




}
