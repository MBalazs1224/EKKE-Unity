using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritySearchState : EnemyBaseState
{
    
    int maxDetectionDistance = 3;
    public override void EnterState(EnemyStateManager manager, GameObject gameobject, Character player)
    {
        stateManager = manager;
        currentObject = gameobject;
        this.player = player;
        Debug.Log("Security entered searching state!");
    }
    public override void Tick()
    {
        if (CanSeePlayer(currentObject, player, maxDetectionDistance))
        {
            stateManager.StateSwitch(new SecurityAttackState());
        }
    }
}
