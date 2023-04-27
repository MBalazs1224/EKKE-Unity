using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotPigeonDetectState : EnemyBaseState
{
    Animator anim;
    float waitTime = 3;
    SceneController sceneController;
    bool canDie = true;
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
    }
    public override void Tick()
    {
        var rayDirection = player.gameObject.transform.position - currentObject.transform.position;
        RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, 1);
        if (result.transform == player.transform && player.IsAttacking()) 
            Death();
       

    }
    public void Death()
    {
        if (canDie)
        {
            sceneController.StopCoroutine(WaitForFly());
            Debug.Log("5G pigeon died!");
            anim.SetTrigger("Death");
            stateManager.AddSelfToRemove();
            currentObject.SetActive(false);
        }
       
    }

    IEnumerator WaitForFly()
    {
        yield return new WaitForSeconds(waitTime);
        canDie = false;
        anim.SetTrigger("Move");
        Debug.Log("5G pigeon started flying away!");
    }
}
