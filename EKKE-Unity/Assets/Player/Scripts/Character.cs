using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    int startRegenerate = 10;
    Checkpoint cp = new Checkpoint();
    int maxHealth = 2;
    int health;
    Animator anim;
    static bool inAir = false;
    static bool canDoubleJump = false;
    Rigidbody2D rb;
    bool canWallJump = false;
    Vector3 wallVector;
    void Death()
    {
        anim.SetTrigger("Death");
        anim.SetBool("Run", false);

    }
    public bool isDead() => health == 0;
    void Attack()
    {
        anim.SetTrigger("Attack");
        anim.SetBool("Run", false);
    }
    public void TakeDamage()
    {
        anim.SetBool("Run", false);
        if (health < 1) return;
        health--;
        if (health == 0)
        {
            Death();
            anim.SetTrigger("Death");
            Debug.Log($"Dead!");
        }
        else
        {
            StopCoroutine(StartRegen());
            StartCoroutine(StartRegen());
            anim.SetTrigger("Hurt");
            //anim.ResetTrigger("Unhurt");
            //anim.ResetTrigger("Death");

            Debug.Log($"Taken damage! Remeaning health: {health}");
        }
    }
    public void ResetHurt()
    {
        anim.SetTrigger("Unhurt");
    }

    IEnumerator StartRegen()
    {
        yield return new WaitForSeconds(startRegenerate);
        health = maxHealth;
        Debug.Log("Health is back to max value!");
    }
    
    private void Update()
    {
        if (isDead()) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            //StopAllCoroutines();

        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            //StopAllCoroutines();


        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
            this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            //StopAllCoroutines();

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Attack();
            rb.velocity = new Vector2(0, rb.velocity.y);
            //StopAllCoroutines();
        }

    }


    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        health = maxHealth;
    }

    private void MoveLeft()
    {

        if (rb.velocity.x > -5)
        {
            rb.velocity += new Vector2(-1f, 0) * Time.deltaTime * 50;
        }
        anim.SetBool("Run", true);
    }
    private void Jump()
    {
        if (canWallJump)
        {
            rb.velocity = wallVector;
            Debug.Log("WallJump");
        }
        else if (inAir && canDoubleJump)
        {
            rb.velocity = new Vector2(0, 7.5f) * Time.deltaTime * 50;
            canDoubleJump = false;
        }
        else if (!inAir)
        {
            rb.velocity = new Vector2(0, 7.5f) * Time.deltaTime * 200;
            inAir = true;
        }
        anim.SetBool("Run", false);
    }
    private void MoveRight()
    {
        if (rb.velocity.x < 5)
        {
            rb.velocity += new Vector2(1f, 0) * Time.deltaTime * 50;
        }
        anim.SetBool("Run", true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            inAir = false;
            canDoubleJump = true;
            canWallJump = false;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            canWallJump = true;
        }
        wallVector = hit.normal;
    }
}
