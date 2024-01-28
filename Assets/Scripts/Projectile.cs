using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] int points = 10;

    //SkeletonAnimation skeletonAnimation;

    [SerializeField] LeanTweenType tweenUp, tweenDown;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        //skeletonAnimation = FindObjectOfType<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        StartCoroutine(ThrowProjectileRoutine());
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


    public IEnumerator ThrowProjectileRoutine()
    {
        //skeletonAnimation.AnimationState.SetAnimation(0, "Throw", true);
        transform.position = new Vector2(Random.Range(-8.5f, 8.5f), transform.position.y);
        transform.localScale = Vector2.zero;
        LeanTween.moveLocalY(gameObject, 8.50f, 1f).setEase(tweenUp);
        LeanTween.scale(gameObject, Vector2.one * .5f, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<Collider2D>().enabled = true;
        LeanTween.moveLocalY(gameObject, 0f, 1f).setEase(tweenDown);
        LeanTween.scale(gameObject, Vector2.one, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        if (CompareTag("Projectile"))
        {
            //skeletonAnimation.AnimationState.SetAnimation(0, "Splat", false);
            //yield return new WaitUntil(() => skeletonAnimation.AnimationState.GetCurrent(0).IsComplete);
        }
        gameObject.SetActive(false);

    }
}
