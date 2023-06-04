using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DroneIdleState : EnemyBaseState
{
    float maxRight;
    float maxLeft;
    private float moveSpeed = 3f;
    bool spotted = false;

    float soundWaitTime = 5f;
    float elapsedTime = 0f;

    float waitedFor = 0;

    AudioSource source;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        maxRight = currentObject.transform.position.x + 10;
        maxLeft = currentObject.transform.position.x - 10;
        anim = currentObject.GetComponent<Animator>();
        Debug.Log("Drone entered idle state!");

        this.source = currentObject.GetComponent<AudioSource>();

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
        if (!CanSeePlayer())
        {
            if (currentObject.transform.position.x < maxRight)
            {
                currentObject.transform.position += new Vector3(1, 0) * Time.deltaTime * moveSpeed;
            }
            else
            {
                currentObject.transform.position += new Vector3(-1, 0) * Time.deltaTime * moveSpeed;
            }
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= AudioController.soundWaitTime)
            {
                elapsedTime = 0f;
                AudioController.PlayDroneIdle(source);
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
