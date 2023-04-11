using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSearchState : EnemyBaseState
{
    float maxXRight;
    float maxXLeft;
    bool moveRight = true;
    int maxDetectionDistance = 3;
    public override void EnterState(EnemyStateManager manager, GameObject gameobject, Character player)
    {
        stateManager = manager;
        currentObject = gameobject;
        maxXLeft = gameobject.transform.position.x - 1;
        maxXRight = gameobject.transform.position.x + 1;
        this.player = player;
        Debug.Log("Policeman entered searching state!");
    }
    public override void Tick()
    {
        if (!CanSeePlayer(currentObject,player,maxDetectionDistance))
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
            stateManager.StateSwitch(new CarAttackState());
        }

    }
}
