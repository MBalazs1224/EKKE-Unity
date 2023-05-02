using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    List<EnemyStateManager> enemies = new List<EnemyStateManager>();
    List<EnemyStateManager> enemiesToBeRemoved = new List<EnemyStateManager>();


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
            currentObject.layer = 2;
            if (currentObject.name.Equals("ExplodingCar"))
                enemies.Add(new EnemyStateManager(new CarSearchState(), currentObject, enemiesToBeRemoved));
            else if (currentObject.name.Equals("5G Pigeon")) enemies.Add(new EnemyStateManager(new RobotPigeonIdleState(), currentObject, enemiesToBeRemoved));
            else if (currentObject.name.Equals("Pigeon")) enemies.Add(new EnemyStateManager(new PigeonIdleState(), currentObject, enemiesToBeRemoved));
            else if (currentObject.name.Equals("Policeman")) enemies.Add(new EnemyStateManager(new PolicemanSearchState(), currentObject, enemiesToBeRemoved));
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
        foreach (var item in enemiesToBeRemoved)
        {
            enemies.Remove(item);
        }
        if (enemiesToBeRemoved.Count != 0) enemiesToBeRemoved.Clear();
    }
}
