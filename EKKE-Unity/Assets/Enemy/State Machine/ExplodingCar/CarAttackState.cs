using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttackState : EnemyBaseState
{
    SceneController sceneController;
    public float explosionTimer = 1f;
    public float explosionRadius = 5f;
    Animator animator;
    float moveSpeed = 0.3f;
    public override void EnterState(EnemyStateManager manager, GameObject gameObject, Character player)
    {
        stateManager = manager;
        currentObject = gameObject;
        this.player = player;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        Debug.Log("Exploding car entered attack state!");
        animator = currentObject.GetComponent<Animator>();
    }

    public override void Tick()
    {
        if (!CanSeePlayer(currentObject, player))
        {
            stateManager.StateSwitch(new CarSearchState());
            animator.SetBool("spot", false);
        }
        else
        {
            currentObject.transform.position = Vector2.MoveTowards(currentObject.transform.position, player.transform.position, moveSpeed);
            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0f)
            {
                Explode(); 
            }
        }
    }

    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(0.75f);
        currentObject.SetActive(false);
    }

    private void Explode()
    {
        // Létrehoz egy robbanás hatását
        Collider[] colliders = Physics.OverlapSphere(currentObject.transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(1, currentObject.transform.position, explosionRadius);
            }
        }
        player.TakeDamage();
        animator.SetBool("dead", true);
        stateManager.shouldTick = false;
        sceneController.StartCoroutine(RemoveEffect());
    }
}
