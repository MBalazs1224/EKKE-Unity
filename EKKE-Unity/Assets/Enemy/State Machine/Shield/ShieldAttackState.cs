using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttackState : EnemyBaseState
{
    SceneController sceneController;
    int attackCooldown = 3;
    float maxRight;
    float maxLeft;
    bool moveRight = true;
    bool canAttack = true;
    private float moveSpeed = 2f;

    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        maxLeft = currentObject.transform.position.x;
        maxRight = maxLeft + 20;
        Debug.Log("Shield entered attack state!");
    }
    public override void Tick()
    {
        if (!CanSeePlayer(15)) stateManager.StateSwitch(new ShieldSearchState());
        if (!canAttack) return;
        if (moveRight)
        {
            currentObject.transform.position += new Vector3(1, 0) * Time.deltaTime * moveSpeed;
            if (currentObject.transform.position.x <= maxRight) moveRight = false;
        }
        else
        {
            currentObject.transform.position += new Vector3(-1, 0) * Time.deltaTime * moveSpeed;
            if (currentObject.transform.position.x >= maxLeft) moveRight = true;
        }

        if (NearPlayer())
        {
            if (player.isSliding)
            {
                stateManager.StateSwitch(new ShieldGatyaState());
            }
            else if (canAttack)
            {
                player.TakeDamage();
                canAttack = false;
                sceneController.StartCoroutine(AttackCooldown());
            }
        }


    }

    private bool NearPlayer()
    {
        return CanSeePlayer(.1f);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
