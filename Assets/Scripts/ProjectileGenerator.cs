using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGenerator : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] LeanTweenType tweenUp, tweenDown;

    [SerializeField] GameObject[] projectilePool;
    [SerializeField] GameObject[] pointPool;


    [Range(1, 100)]

    [SerializeField] float projectileCooldown = 1f;
    float projectileCoolDownTimer;

    [SerializeField] float pointCooldown = 5f;
    float pointCoolDownTimer;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        projectileCoolDownTimer = projectileCooldown;
    }

    private void Update()
    {
        if (!gameManager.gameRunning) { return; }
        ThrowProjectiles();
        ThrowPoints();
    }

    private void ThrowProjectiles()
    {
        projectileCoolDownTimer -= Time.deltaTime;
        if (projectileCoolDownTimer <= 0f)
        {
            for (int i = 0; i < projectilePool.Length; i++)
            {
                if (!projectilePool[i].activeInHierarchy)
                {
                    projectilePool[i].SetActive(true);
                    projectileCoolDownTimer = (projectileCooldown / 100) * gameManager.satisfaction;
                    break;
                }
            }
        }
    }
    private void ThrowPoints()
    {
        if(gameManager.satisfaction < gameManager.targetSatisfaction) { return; }
        pointCoolDownTimer -= Time.deltaTime;
        if (pointCoolDownTimer <= 0f)
        {
            for (int i = 0; i < pointPool.Length; i++)
            {
                if (!pointPool[i].activeInHierarchy)
                {
                    pointPool[i].SetActive(true);
                    pointCoolDownTimer = pointCooldown;
                    break;
                }
            }
        }
    }

    
}
