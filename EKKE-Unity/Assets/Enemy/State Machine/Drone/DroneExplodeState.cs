using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneExplodeState : EnemyBaseState
{
    private float waitTime = 3f;
    float chargedFor = 0f;
    Animator anim;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        anim = currentObject.GetComponent<Animator>();
    
    }
    public override void Tick()
    {
        if (!player.IsAttacking())
        {
            chargedFor += Time.deltaTime;
            if (chargedFor >= waitTime)
            {
                anim.SetTrigger("Explode");
                if (TouchesPlayer())
                {
                    player.TakeDamage();
                }
                GameObject.Find("SceneController").GetComponent<SceneController>().StartCoroutine(RemoveEffect());
                stateManager.AddSelfToRemove();
            }
        }
        else
        {
            if (TouchesPlayer())
            {
                GameObject.Find("SceneController").GetComponent<SceneController>().StartCoroutine(RemoveEffect(false));
                anim.SetTrigger("Hurt");
                stateManager.AddSelfToRemove();
            }
        }
    }
    IEnumerator RemoveEffect(bool explode = true)
    {
        float wait = explode ? .817f : .417f;
        yield return new WaitForSeconds(wait);
        currentObject.SetActive(false);
    }

    private bool TouchesPlayer()
    {
        var hit = Physics2D.Raycast(currentObject.transform.position, player.transform.position, 10);
        return hit.transform == player.transform;
    }
}
