using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int points = 10;
    [SerializeField] int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(CompareTag("Point"))
            {
                GameManager.points += points;
            }
            if(CompareTag("Projectile"))
            {
                GameManager.hitPoints -= damage;
            }
            Debug.Log("Hitpoints: " + GameManager.hitPoints);
            Debug.Log("Points: " + GameManager.points);
        }

    }
}
