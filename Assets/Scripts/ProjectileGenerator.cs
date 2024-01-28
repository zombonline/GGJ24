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
                    StartCoroutine(ThrowProjectile(projectilePool[i]));
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
                    StartCoroutine(ThrowProjectile(pointPool[i]));
                    pointCoolDownTimer = pointCooldown;
                    break;
                }
            }
        }
    }

    public IEnumerator ThrowProjectile(GameObject projectile)
    {
        projectile.SetActive(true);
        projectile.transform.position = new Vector2(Random.Range(-8.5f, 8.5f), projectile.transform.position.y);
        projectile.transform.localScale = Vector2.zero;
        LeanTween.moveLocalY(projectile, 8.50f, 1f).setEase(tweenUp);
        LeanTween.scale(projectile, Vector2.one * .5f, 1f);
        yield return new WaitForSeconds(1f);
        projectile.GetComponent<Collider2D>().enabled = true;
        LeanTween.moveLocalY(projectile, 0f, 1f).setEase(tweenDown);
        LeanTween.scale(projectile, Vector2.one, 1f);
        yield return new WaitForSeconds(1f);
        projectile.GetComponent<Collider2D>().enabled = false;
        projectile.SetActive(false);
    }
}
