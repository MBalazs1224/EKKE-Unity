using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpotState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Spot");
        Debug.Log("Shield entered spot state!");
        SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
        sc.StartCoroutine(SwitchToAttack());
    }

    public override void Tick()
    {
        return;
    }
    
    IEnumerator SwitchToAttack()
    {
        yield return new WaitForSeconds(0.55f);
        stateManager.StateSwitch(new ShieldAttackState());
    }
}
