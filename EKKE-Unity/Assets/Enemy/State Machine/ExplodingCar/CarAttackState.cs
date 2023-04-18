using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttackState : EnemyBaseState
{
    SceneController sceneController;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Exploding car entered attack state!");
    }

    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player))
            stateManager.StateSwitch(new CarSearchState());
        else
        {
            Vector2.MoveTowards(currentObject.transform.position, player.transform.position, 1);

            var rayDirection = player.gameObject.transform.position - currentObject.transform.position;
            RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, 1);
            if (result.transform == player.transform)
            {
                player.TakeDamage();
            }
        }
    }
}
