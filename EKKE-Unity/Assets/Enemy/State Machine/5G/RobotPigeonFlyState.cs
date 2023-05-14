using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPigeonFlyState : EnemyBaseState
{
    Vector3 target;
    private float waitTime = 3;
    private float moveSpeed = 0.2f;

    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        GameObject.Find("SceneController").GetComponent<SceneController>().StartCoroutine(RemoveSelf());
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        System.Random rnd = new System.Random();

        double num = rnd.NextDouble();
        if (num < .5d)
        {
            currentObject.transform.rotation = new Quaternion(0, 0, 10,0);
            target = new Vector2(100, 100);
        }
        else
        {
            currentObject.transform.rotation = new Quaternion(0, 0, 10,0);
            target = new Vector2(-100, 100);
        }
    }
    public override void Tick()
    {
        currentObject.transform.position += target * Time.deltaTime * moveSpeed;
    }

    IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(waitTime);
        currentObject.SetActive(false);
        Debug.Log("5G pigeon removed!");
        stateManager.shouldTick = false;

    }
}
