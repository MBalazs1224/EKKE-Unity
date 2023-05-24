using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityAttackState : EnemyBaseState
{
    SceneController sceneController;
    private bool canAttack = true;
    private float moveSpeed = .1f;
    Animator animator;
    int attackCooldown = 3;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Security entered attack state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer())
        {
            animator.SetBool("spot", false);
            stateManager.StateSwitch(new SecuritySearchState());
        }
        else
        {
            if (!TouchesPlayer())
            {
                animator.SetTrigger("move");
                currentObject.transform.position = Vector2.MoveTowards(currentObject.transform.position, player.transform.position, moveSpeed);
            }
            else
            {
                if (canAttack)
                {
                    player.TakeDamage();
                    sceneController.StartCoroutine(AttackCooldown());
                }
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        anim.SetTrigger("Spot");
    }

    private bool TouchesPlayer()
    {
        var hit = Physics2D.Raycast(currentObject.transform.position, player.transform.position, 3);
        return hit.transform == player.transform;
    }
}
