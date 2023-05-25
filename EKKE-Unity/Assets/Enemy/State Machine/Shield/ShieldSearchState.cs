using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSearchState : EnemyBaseState
{
    int maxDetectionDistance = 15;
    public override void EnterState(EnemyStateManager manager, GameObject gameobject, Character player)
    {
        stateManager = manager;
        currentObject = gameobject;
        this.player = player;
        Debug.Log("Shield entered searching state!");
        anim = gameobject.GetComponent<Animator>();
        anim.SetTrigger("Idle");

    }
    public override void Tick()
    {
        if (CanSeePlayer(maxDetectionDistance))
        {
            stateManager.StateSwitch(new ShieldSpotState());
        }

    }
}
