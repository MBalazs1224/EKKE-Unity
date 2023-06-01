using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class RobotPigeonIdleState : EnemyBaseState
{
    float maxDetectionDistance = 20;
    float waitForSound = 5f;
    float elapsedTime = 0f;
    AudioSource source;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        Debug.Log("5G pigeon entered idle state!");
        this.stateManager = manager;
        this.player = player;
        this.currentObject = gameObject;
        anim = currentObject.GetComponent<Animator>();
        source = currentObject.GetComponent<AudioSource>();
    }
    public override void Tick()
    {
        if (CanSeePlayer(maxDetectionDistance)) stateManager.StateSwitch(new RobotPigeonDetectState()) ;
        else
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitForSound)
            {
                elapsedTime = 0f;
                AudioController.PlayPigeonIdle5G(source);
            }
        }
    }
}
