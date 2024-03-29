using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotPigeonDetectState : EnemyBaseState
{
    float waitTime = 3;
    SceneController sceneController;
    bool canDie = true;
    bool isDead = false;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        Debug.Log("5G pigeon entered detect state!");
        this.stateManager = manager;
        this.player = player;
        this.currentObject = gameObject;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        anim = currentObject.GetComponent<Animator>();
        anim.SetTrigger("Spot");
        sceneController.StartCoroutine(WaitForFly());

        AudioController.PlayPigeonSpot5G(currentObject.GetComponent<AudioSource>());
    }
    public override void Tick()
    {
        var rayDirection = player.gameObject.transform.position - currentObject.transform.position;
        RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, 2);
        if (result.transform == player.transform && player.IsAttacking()) 
            Death();
       

    }
    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(0.75f);
        currentObject.SetActive(false);
    }
    public void Death()
    {
        if (canDie)
        {
            player.pigeonsKilled++;
            sceneController.StopCoroutine(WaitForFly());
            isDead = true;
            Debug.Log("5G pigeon died!");
            anim.SetTrigger("Death");
            stateManager.shouldTick = false;
            sceneController.StartCoroutine(RemoveEffect());
        }
       
    }

    IEnumerator WaitForFly()
    {
        yield return new WaitForSeconds(waitTime);
        if (!isDead)
        {
            canDie = false;
            anim.SetTrigger("Move");
            stateManager.StateSwitch(new RobotPigeonFlyState());
            Debug.Log("5G pigeon started flying away!");
        }

    }
}
