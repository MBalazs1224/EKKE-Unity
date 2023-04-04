using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateManager stateManager;
    protected GameObject currentObject;
    protected Character player;
    public abstract void Tick();
    public abstract void EnterState(EnemyStateManager manager,GameObject gameObject, Character player);
    protected bool CanSeePlayer(GameObject current, Character player, float maxDetectionDistance = 3)
    {
        if (player.isDead()) return false;
        var rayDirection = player.gameObject.transform.position - current.transform.position;
        RaycastHit2D result = Physics2D.Raycast(current.transform.position, rayDirection, maxDetectionDistance);
        return result.transform == player.transform;
    }
}
