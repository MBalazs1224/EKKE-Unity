using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSearchState : EnemyBaseState
{
    int maxDetectionDistance = 5;
    Animator animator;
    float maxXRight;
    float maxXLeft;
    bool moveRight = true;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        maxXLeft = currentObject.transform.position.x - 5;
        maxXRight = currentObject.transform.position.x + 5;
        this.player = player;
        Debug.Log("Exploding car entered search state!");
        animator = currentObject.GetComponent<Animator>();
        sr = currentObject.GetComponent<SpriteRenderer>();
        maxXLeft = currentObject.transform.position.x - 2;
        maxXRight = currentObject.transform.position.x + 2;
        this.player = player;
        Debug.Log("Exploding car entered search state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player, maxDetectionDistance))
        {
            if (moveRight)
            {
                currentObject.transform.Translate(new Vector3(.2f, 0));
                if (currentObject.transform.position.x >= maxXRight)
                {
                    moveRight = false;
                    animator.Play("Turn_Left");
                    sr.flipX = true;
                    currentObject.transform.Translate(new Vector3(.01f, 0f));
                    if (currentObject.transform.position.x >= maxXRight)
                    {
                        moveRight = false;
                        animator.SetBool("Left", true);
                    }
                }
                else
                {

                    currentObject.transform.Translate(new Vector3(-.2f, 0));
                    if (currentObject.transform.position.x <= maxXLeft)
                    {
                        moveRight = true;
                        animator.Play("Turn_Right");
                        sr.flipX = false;
                        currentObject.transform.Translate(new Vector3(-.01f, 0f));
                        if (currentObject.transform.position.x <= maxXLeft)
                        {
                            moveRight = true;
                        }
                    }
                }
            }
            else
            {
                stateManager.StateSwitch(new CarAttackState());
            }
        }
    }
}
