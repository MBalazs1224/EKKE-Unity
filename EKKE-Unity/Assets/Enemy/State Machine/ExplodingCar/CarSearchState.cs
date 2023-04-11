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
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        maxXLeft = currentObject.transform.position.x - 1;
        maxXRight = currentObject.transform.position.x + 1;
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
                currentObject.transform.Translate(new Vector3(.01f, 0));
                if (currentObject.transform.position.x >= maxXRight) moveRight = false;
            }
            else
            {
                currentObject.transform.Translate(new Vector3(-.01f, 0));
                if (currentObject.transform.position.x <= maxXLeft) moveRight = true;
            }
        }
        else
        {
            stateManager.StateSwitch(new ShieldAttackState());
        }
    }
}
