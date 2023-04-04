using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicemanSearchState : EnemyBaseState
{
    int maxDetectionDistance = 5;
    Animator animator;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        Debug.Log("Policeman entered search state!");
        animator = currentObject.GetComponent<Animator>();
    }
    public override void Tick()
    {
        if (CanSeePlayer(currentObject, player, maxDetectionDistance)) 
        {
            animator.SetTrigger("Spot");
            stateManager.StateSwitch(new PolicemanAttackState());
        }
    }
}
