using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicemanSearchState : EnemyBaseState
{
    int maxDetectionDistance = 15;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        Debug.Log("Policeman entered search state!");
        anim = currentObject.GetComponent<Animator>();
    }
    public override void Tick()
    {
        if (CanSeePlayer(maxDetectionDistance)) 
        {
            anim.SetBool("Spot",true);
            stateManager.StateSwitch(new PolicemanAttackState());
        }
    }
}
