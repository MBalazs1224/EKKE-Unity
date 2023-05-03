using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIdleState : EnemyBaseState
{
    float maxRight;
    float maxLeft;
    private float moveSpeed = 3f;
    bool spotted = false;
    Animator anim;

    float waitedFor = 0;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        maxRight = currentObject.transform.position.x + 10;
        maxLeft = currentObject.transform.position.x - 10;
        anim = currentObject.GetComponent<Animator>();
        Debug.Log("Drone entered idle state!");
    }

    public override void Tick()
    {
        if (spotted)
        {
            waitedFor += Time.deltaTime;
            if (waitedFor >= .267f)
            {
                stateManager.StateSwitch(new DroneAttackState());
            }
            return;
        }
        if (!CanSeePlayer(currentObject,player,30))
        {
            if (currentObject.transform.position.x < maxRight)
            {
                currentObject.transform.position += new Vector3(1, 0) * Time.deltaTime * moveSpeed;
            }
            else
            {
                currentObject.transform.position += new Vector3(-1, 0) * Time.deltaTime * moveSpeed;
            }
        }
        else
        {
            anim.SetTrigger("Spot");
            spotted = true;
            Debug.Log("Drone spotted the player!");
        }
    }

}
