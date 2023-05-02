using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotPigeonDetectState : EnemyBaseState
{
    Animator anim;
    float waitTime = 1;
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
    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(0.75f);
        currentObject.SetActive(false);
    }
    public void Death()
    {
        if (canDie)
        {
            sceneController.StopCoroutine(WaitForFly());
            Debug.Log("5G pigeon died!");
            anim.SetTrigger("Death");
            stateManager.AddSelfToRemove();
            sceneController.StartCoroutine(RemoveEffect());
        }
       
    }

    IEnumerator WaitForFly()
    {
        yield return new WaitForSeconds(waitTime);
<<<<<<< Updated upstream
        canDie = false;
        anim.SetTrigger("Move");
        Debug.Log("5G pigeon started flying away!");
=======
        if (!isDead)
        {
            canDie = false;
            anim.SetTrigger("Move");
            stateManager.StateSwitch(new RobotPigeonFlyState());
        }

>>>>>>> Stashed changes
    }
}
