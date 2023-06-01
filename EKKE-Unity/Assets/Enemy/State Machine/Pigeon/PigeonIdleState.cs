using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonIdleState : EnemyBaseState
{
    float soundWaitTime = 5f;
    float elapsedTime = 0f;
    AudioSource source;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        this.source = currentObject.GetComponent<AudioSource>();
    }
    public override void Tick()
    {
        if (CanSeePlayer(20)) 
        { 
            stateManager.StateSwitch(new PigeonMoveState()); 
        }
        else
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= soundWaitTime)
            {
                elapsedTime = 0f;
                System.Random rnd = new System.Random();
                if (rnd.NextDouble() < .5f)
                {
                    AudioController.PlayPigeonIdle(source);
                }
                else
                {
                    AudioController.PlayPigeonIdle2(source);
                }
            }
        }
    }
}
