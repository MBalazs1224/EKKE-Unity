using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    List<EnemyStateManager> enemies = new List<EnemyStateManager>();
    void Start()
    {
        FindEnemies();
    }

    private void FindEnemies()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var currentObject in enemyGameObjects)
        {
            if (currentObject.name.Equals("Shield"))
            {
                enemies.Add(new EnemyStateManager(new ShieldSearchState(), currentObject));
            }
            else
            {
                enemies.Add(new EnemyStateManager(new PolicemanSearchState(), currentObject));
            }
        }

       
    }

    void Update()
    {
        TicKEnemies();
    }

    private void TicKEnemies()
    {
        foreach (var item in enemies)
        {
            item.Tick();
        }
    }
}
