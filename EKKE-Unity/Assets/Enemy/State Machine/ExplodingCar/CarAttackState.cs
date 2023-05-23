using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttackState : EnemyBaseState
{
    SceneController sceneController;
    Animator animator;
    float moveSpeed = 0.3f;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Exploding car entered attack state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player))
            stateManager.StateSwitch(new CarSearchState());
        else
        {
            currentObject.transform.position = Vector2.MoveTowards(currentObject.transform.position, player.transform.position, moveSpeed);

            var rayDirection = player.gameObject.transform.position - currentObject.transform.position;
            RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, 1);
            if (result.transform == player.transform)
            {
                animator.SetBool("dead", true);
                player.TakeDamage();
                stateManager.shouldTick = false;
                sceneController.StartCoroutine(RemoveEffect());
            }
        }
    }

    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(0.75f);
        currentObject.SetActive(false);
    }
}
