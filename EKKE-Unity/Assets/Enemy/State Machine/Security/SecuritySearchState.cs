using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritySearchState : EnemyBaseState
{
    Animator animator;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        Debug.Log("Security entered search state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (CanSeePlayer())
        {
            animator.SetBool("spot", true);
            stateManager.StateSwitch(new SecurityAttackState());
        }
    }
}
