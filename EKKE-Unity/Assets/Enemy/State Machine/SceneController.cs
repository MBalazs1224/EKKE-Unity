using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    List<EnemyStateManager> enemies = new List<EnemyStateManager>();


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
    void Start()
    {
        FindEnemies();
    }

    private void FindEnemies()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var currentObject in enemyGameObjects)
        {
            if (currentObject.name.Equals("ExplodingCar"))
            {
                enemies.Add(new EnemyStateManager(new CarSearchState(), currentObject));
            }
        }

       
    }

    void Update()
    {
        TicKEnemies();
        //Debug.Log($"FPS: {1.0f / Time.deltaTime}");
    }

    private void TicKEnemies()
    {
        foreach (var item in enemies)
        {
            item.Tick();
        }
    }
}
