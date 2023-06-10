using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShieldGatyaState : EnemyBaseState
{
    float waitTime = 3f;
    private bool shouldTick = true;
    SceneController sc;

    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        this.anim = currentObject.GetComponent<Animator>();
        anim.SetTrigger("Stun");
        sc = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Shield entered gatya state!");
    }

    public override void Tick()
    {
        if (!shouldTick) return;
        if (NearPlayer() && player.IsAttacking())
        {
            anim.SetTrigger("Hurt");
            sc.StartCoroutine(Recover());

        }
        waitTime -= Time.deltaTime;
        if (waitTime <= 0f)
        {
            anim.SetTrigger("Recover");
            shouldTick = false;
            sc.StartCoroutine(Recover());
        }
       
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.8f);
        currentObject.SetActive(false);
        stateManager.shouldTick = false;
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.6f);
        stateManager.StateSwitch(new ShieldSearchState());
    }

    private bool NearPlayer()
    {
        return CanSeePlayer(1);
    }

}
