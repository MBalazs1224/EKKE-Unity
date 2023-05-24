using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
    }
    public override void Tick()
    {
        if (CanSeePlayer(20)) 
        { 
            stateManager.StateSwitch(new PigeonMoveState()); 
        }
    }
}
