using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager
{
    EnemyBaseState currentState;
    public GameObject current;
    Character player;
    Vector3 spawnPoint;
    public bool shouldTick = true;
    public EnemyStateManager(EnemyBaseState startState,GameObject gameObject ,Vector3 spawnPoint)
    {
        currentState = startState;
        current = gameObject;
        player = GameObject.Find("Player").GetComponent<Character>();
        currentState.EnterState(this,gameObject,player);
        this.spawnPoint = spawnPoint;
    }
    public void Respawn()
    {
        current.transform.position = spawnPoint;
        current.SetActive(true);
        shouldTick = true;
    }
    public void Tick()
    {
        if (!shouldTick) return;
        currentState.Tick();
    }
    public void StateSwitch(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this, current, player);
    }
}
