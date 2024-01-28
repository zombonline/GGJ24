using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] int points = 10;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameManager.gameRunning) { return; }
        if(collision.CompareTag("Player"))
        {
            if(CompareTag("Point"))
            {
                gameManager.points += points;
            }
            if(CompareTag("Projectile"))
            {
                gameManager.DecreaseHitPoints();
            }
        }

    }
}
