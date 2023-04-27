using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPigeonFlyState : EnemyBaseState
{
    Vector3 target;
    private float waitTime = 3;

    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        this.stateManager = manager;
        this.currentObject = gameObject;
        this.player = player;
        System.Random rnd = new System.Random();

        double num = rnd.NextDouble();
        if (num < .5d)
        {
            currentObject.transform.rotation = new Quaternion(0, 0, 45, 0);
            target = new Vector2(100, 100);
        }
        else
        {
            currentObject.transform.rotation = new Quaternion(0, 0, -45, 0);
            target = new Vector2(-100, -100);
        }
    }
    public override void Tick()
    {
        currentObject.transform.position += target * Time.deltaTime;
    }

    IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(waitTime);
        stateManager.AddSelfToRemove();
        currentObject.SetActive(false);
        Debug.Log("5G pigeon removed!");
    }
}
