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
            EnemyBaseState enemyToBeAdded;
            switch (currentObject.name)
            {
                case "ExplodingCar":
                    enemyToBeAdded = new CarSearchState();
                    break;
                case "5G Pigeon":
                    enemyToBeAdded = new RobotPigeonIdleState();
                    break;
                case "Pigeon":
                    enemyToBeAdded = new PigeonIdleState();
                    break;
                case "Policeman":
                    enemyToBeAdded = new PolicemanSearchState();
                    break;
                default:
                    enemyToBeAdded = new DroneIdleState();
                    break;
            }

            enemies.Add(new EnemyStateManager(enemyToBeAdded, currentObject, enemiesToBeRemoved));
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
