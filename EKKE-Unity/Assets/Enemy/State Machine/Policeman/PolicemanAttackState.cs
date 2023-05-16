using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PolicemanAttackState : EnemyBaseState
{
    
    int maxDetectionDistance = 5;
    int chargeUpTime = 1;
    float chargedFor = 0f;
    bool canAttack = true;
    int attackCooldown = 3;
    LineRenderer lr;

    SceneController sceneController;
    Vector3[] currentPositions = new Vector3[2];

    Animator animator;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        lr = currentObject.GetComponent<LineRenderer>();
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        Debug.Log("RangedPoliceman entered attack state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player, maxDetectionDistance)) 
        {
            sceneController.StopCoroutine(AttackCooldown());
            //lr.SetPositions(emptyPositions);
            lr.startWidth = 0;
            lr.endWidth = 0;
            animator.SetBool("Spot", false);
            stateManager.StateSwitch(new PolicemanSearchState());
        }

        else if (CanSeePlayer(currentObject,player,1) && player.IsAttacking())
        {
            Death();
        }
        else
        {
            if (player.transform.position.x < currentObject.transform.position.x)
            {
                currentObject.transform.rotation = new Quaternion(0, -180, 0, 0);
            }
            else
            {
                currentObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (canAttack)
            {
                lr.startWidth = .01f;
                lr.endWidth = .01f;
                currentPositions[0] = currentObject.transform.position;
                currentPositions[1] = player.transform.position;
                lr.SetPositions(currentPositions);
                chargedFor += Time.deltaTime;
                if (chargedFor >= chargeUpTime)
                {
                    animator.SetTrigger("Attack");
                    player.TakeDamage();
                    chargedFor = 0;
                    sceneController.StartCoroutine(AttackCooldown());
                }
            }
            else
            {
                lr.startWidth = 0;
                lr.endWidth = 0;
            }
            
        }
    }

    private void Death()
    {
        animator.SetTrigger("Death");
        stateManager.shouldTick = false;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        animator.SetTrigger("Spot");
    }
}
