using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class CarSearchState : EnemyBaseState
{
    float moveSpeed = 0.25f;
    Animator animator;
    float maxXRight;
    float maxXLeft;
    bool moveRight = true;
    SpriteRenderer sr;
    Rigidbody2D rb;

    float elapsedTime = 0f;
    AudioSource source;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        maxXLeft = currentObject.transform.position.x - 8;
        maxXRight = currentObject.transform.position.x + 8;
        this.player = player;
        Debug.Log("Exploding car entered search state!");
        sr = currentObject.GetComponent<SpriteRenderer>();
        animator = currentObject.GetComponent<Animator>();

        source = currentObject.GetComponent<AudioSource>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer())
        {
            if (moveRight)
            {
                currentObject.transform.Translate(Vector3.right * moveSpeed);
                if (currentObject.transform.position.x >= maxXRight)
                {
                    moveRight = false;
                    sr.flipX = true;
                    animator.SetBool("left", true);
                }
            }
            else
            {

                currentObject.transform.Translate(Vector3.left * moveSpeed);
                if (currentObject.transform.position.x <= maxXLeft)
                {
                    moveRight = true;
                    animator.SetBool("right", true);
                    sr.flipX = false;
                }
            }

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= AudioController.soundWaitTime)
            {
                elapsedTime = 0f;
                AudioController.PlayDroneIdle(source);
            }
        }
        else
        {
            animator.SetBool("spot", true);
            stateManager.StateSwitch(new CarAttackState());
        }
    }
}
