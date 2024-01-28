using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] int points = 10;

    SkeletonAnimation skeletonAnimation;

    [SerializeField] LeanTweenType tweenUp, tweenDown;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        gameObject.SetActive(false);
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
            StopAllCoroutines();
            LeanTween.cancel(gameObject);
            if(CompareTag("Point"))
            {
                FMODController.Play3DSFX("event:/Projectiles/Projectiles_Rose_Catch", transform.position);
                gameManager.points += points;
                gameObject.SetActive(false);

            }
            if (CompareTag("Projectile"))
            {
                gameManager.DecreaseHitPoints();
                StartCoroutine(SplashAnimationRoutine());
            }
        }
    }

    IEnumerator SplashAnimationRoutine()
    {
        skeletonAnimation.skeleton.SetSkin("Splash");
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.SetAnimation(0, "Splash", false);
        yield return new WaitUntil(() => skeletonAnimation.AnimationState.GetCurrent(0).IsComplete);
        gameObject.SetActive(false);
    }

    public IEnumerator ThrowProjectileRoutine()
    {
        if (CompareTag("Projectile"))
        {
            skeletonAnimation.skeleton.SetSkin("Tomato");
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
        }
        skeletonAnimation.AnimationState.SetAnimation(0, "Throw", true);
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
            FMODController.Play3DSFX("event:/Projectiles/Projectiles_Tomato_Splat", transform.position);
            skeletonAnimation.skeleton.SetSkin("Splash");
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.SetAnimation(0, "Splash", false);
            yield return new WaitUntil(() => skeletonAnimation.AnimationState.GetCurrent(0).IsComplete);
        }
        gameObject.SetActive(false);
    }
}
