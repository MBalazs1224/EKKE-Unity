using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager
{
    EnemyBaseState currentState;
    GameObject current;
    Character player;
    List<EnemyStateManager> toBeRemovedList;
    public EnemyStateManager(EnemyBaseState startState,GameObject gameObject, List<EnemyStateManager> list)
    {
        currentState = startState;
        current = gameObject;
        this.toBeRemovedList = list;
        player = GameObject.Find("Player").GetComponent<Character>();
        currentState.EnterState(this,gameObject,player);
    }

    public void AddSelfToRemove()
    {
        toBeRemovedList.Add(this);
    }
    public void Tick()
    {
        currentState.Tick();
    }
    public void StateSwitch(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this, current, player);
    }
}
