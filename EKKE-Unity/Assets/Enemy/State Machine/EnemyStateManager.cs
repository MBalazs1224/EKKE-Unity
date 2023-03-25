using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager
{
    EnemyBaseState currentState;
    GameObject current;
    Character player;
    public EnemyStateManager(EnemyBaseState startState,GameObject gameObject)
    {
        currentState = startState;
        current = gameObject;
        player = GameObject.Find("Player").GetComponent<Character>();
        currentState.EnterState(this,gameObject,player);
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
