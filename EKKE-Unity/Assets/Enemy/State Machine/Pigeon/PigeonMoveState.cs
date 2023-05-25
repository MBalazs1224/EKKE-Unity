using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PigeonMoveState : EnemyBaseState
{
    Vector3 target;
    float moveSpeed = 0.2f;
    float waitTime = 10f;

    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;

        currentObject.GetComponent<Animator>().SetTrigger("Move");
        System.Random rnd = new System.Random();

        double num = rnd.NextDouble();
        if (num < .5d)
        {
            target = new Vector2(100, 100);
        }
        else
        {
            target = new Vector2(-100, 100);
        }

        GameObject.Find("SceneController").GetComponent<SceneController>().StartCoroutine(RemoveSelf());

        Debug.Log("Pigeon started flying away!");
    }
    public override void Tick()
    {
        currentObject.transform.position += target * Time.deltaTime * moveSpeed;
    }
    IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(waitTime);
        stateManager.shouldTick = false;
        currentObject.SetActive(false);
    }
}
