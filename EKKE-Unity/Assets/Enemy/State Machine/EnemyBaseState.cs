using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class EnemyBaseState
{
    protected EnemyStateManager stateManager = null;
    protected GameObject currentObject = null;
    protected Character player = null;
    protected Animator anim = null;
    public abstract void Tick();
    public abstract void EnterState(EnemyStateManager manager, GameObject gameObject, Character player);
    protected bool CanSeePlayer(float maxDetectionDistance = 10)
    {
        if (player.isDead()) return false;
        var rayDirection = player.transform.position - currentObject.transform.position;
        RaycastHit2D result = Physics2D.Raycast(currentObject.transform.position, rayDirection, maxDetectionDistance);
        Debug.DrawLine(player.transform.position, currentObject.transform.position);
        return result.transform == player.transform;
    }
}
