using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    GameManager gameManager;

    SkeletonAnimation skeletonAnimation;
    Spine.Animation currrentAnim;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        skeletonAnimation.AnimationState.SetAnimation(0, "Walk/Idle", true);
        currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
    }

    void Update()
    {
        if (!gameManager.gameRunning)
        {
            if (currrentAnim.Name != "Walk/Idle")
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Walk/Idle", true);
            }
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if(currrentAnim.Name == "Walk/Right") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk/Right", true);
            currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (currrentAnim.Name == "Walk/Left") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk/Left", true);
            currrentAnim = skeletonAnimation.AnimationState.GetCurrent(0).Animation;
        }
        else
        {
            if (currrentAnim.Name == "Walk/Idle") { return; }
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk/Idle", true);
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
