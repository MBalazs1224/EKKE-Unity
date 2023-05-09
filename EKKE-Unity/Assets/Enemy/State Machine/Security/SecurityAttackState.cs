using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityAttackState : EnemyBaseState
{
    bool moveRight = true;
    float maxXRight;
    float maxXLeft;
    bool canAttack = true;
    SceneController sceneController;
    int attackCooldown = 3;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        maxXLeft = currentObject.transform.position.x - 1;
        maxXRight = currentObject.transform.position.x + 1;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Security entered attack state!");
    }
    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player)) 
            stateManager.StateSwitch(new ShieldSearchState());
        else
        {
            if (moveRight)
            {
                currentObject.transform.Translate(new Vector3(.01f, 0));
                if (currentObject.transform.position.x >= maxXRight) moveRight = false;
            }
            else
            {
                currentObject.transform.Translate(new Vector3(-.01f, 0));
                if (currentObject.transform.position.x <= maxXLeft) moveRight = true;
            }
        }
        if (canAttack)
        {
            Vector2.MoveTowards(currentObject.transform.position, player.transform.position, 1);

            var rayDirection = player.gameObject.transform.position - currentObject.transform.position;
            RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, 1);
            if (result.transform == player.transform)
            {
                player.TakeDamage();
                sceneController.StartCoroutine(AttackCooldown());
            }
        }

    }
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
