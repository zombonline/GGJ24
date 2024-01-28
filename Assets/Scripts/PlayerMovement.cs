using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    GameManager gameManager;

    public SkeletonAnimation skeletonAnimation;
    Spine.Animation currrentAnim;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(2, "Constant/Breathing", true);
        skeletonAnimation.AnimationState.SetAnimation(0, "Movement/Idle", true);
        currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
    }
    public void RandomGesture()
    {
        StartCoroutine(RandomGestureRoutine());
    }
    IEnumerator RandomGestureRoutine()
    {
        int randomIndex = Random.Range(1, 5);
        skeletonAnimation.AnimationState.SetAnimation(1, "Arms/Gesture " + randomIndex.ToString(), false);
        yield return new WaitUntil(() => skeletonAnimation.AnimationState.GetCurrent(1).IsComplete);
        skeletonAnimation.AnimationState.SetAnimation(1, "Arms/Gesture 5", false);
    }
    void Update()
    {
        if (!gameManager.gameRunning)
        {
            if (currrentAnim.Name != "Movement/Idle")
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Movement/Idle", true);
            }
            return;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if(currrentAnim.Name == "Movement/Right") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Movement/Right", true);
            currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (currrentAnim.Name == "Movement/Left") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Movement/Left", true);
            currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
        }
        else
        {
            if (currrentAnim.Name == "Movement/Idle") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Movement/Idle", true);
            currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
        }
        if (transform.position.x > 8.5f)
        {
            transform.position = new Vector2(8.5f, transform.position.y);
        }
        else if( transform.position.x < -8.5f)
        {
            transform.position = new Vector2(-8.5f, transform.position.y);
        }
    }

}
