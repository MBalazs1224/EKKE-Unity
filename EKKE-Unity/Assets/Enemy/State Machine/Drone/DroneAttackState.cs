using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackState : EnemyBaseState
{
    bool exploded = false;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        anim = currentObject.GetComponent<Animator>();
        AudioController.PlayDroneSpotAttack(currentObject.GetComponent<AudioSource>());
        Debug.Log("Drone entered attack state!");
    }

    public override void Tick()
    {
        if (exploded) return;
        if (!TouchesPlayer())
        {
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, player.transform.position, 1f);
        }
        else
        {
            stateManager.StateSwitch(new DroneExplodeState());
        }
       
    }

    private bool TouchesPlayer()
    {
        var hit = Physics2D.Raycast(currentObject.transform.position, player.transform.position, 3);
        return hit.transform == player.transform;
    }
}
