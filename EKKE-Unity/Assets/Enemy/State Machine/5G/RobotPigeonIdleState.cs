using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPigeonIdleState : EnemyBaseState
{
    float maxDetectionDistance = 20;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        Debug.Log("5G pigeon entered idle state!");
        this.stateManager = manager;
        this.player = player;
        this.currentObject = gameObject;
        anim = currentObject.GetComponent<Animator>();
    }
    public override void Tick()
    {
        if (CanSeePlayer(maxDetectionDistance)) stateManager.StateSwitch(new RobotPigeonDetectState()) ;
    }
}
