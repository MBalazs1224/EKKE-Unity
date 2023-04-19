using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    int startRegenerate = 10;
    Checkpoint cp = new Checkpoint();
    [SerializeField]
    int maxHealth = 2;
    int health;
    Animator anim;
    static bool inAir = false;
    static bool canDoubleJump = false;
    Rigidbody2D rb;
    bool canWallJump = false;
    [SerializeField]
    float jumpHeight = 50;
    [SerializeField]
    float doubleJumpHeight = 10;
    Vector3 wallVector;
    private bool isSliding;
    [SerializeField]
    private int moveSpeed = 50;
    [SerializeField]
    int respawnTime = 3;

    IEnumerator Death()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
        anim.SetBool("Slide", false);
        anim.SetTrigger("Death");
        Debug.Log("Respawning");
        yield return new WaitForSeconds(respawnTime);
        Respawn();
    }

    private void Respawn()
    {
        
        this.gameObject.transform.position = cp.resumePoint;
        anim.SetBool("Idle", true);
        health = maxHealth;
    }

    public bool isDead() => health == 0;
    void Attack()
    {
        anim.SetTrigger("Attack");
        anim.SetBool("Run", false);
        anim.SetBool("Slide", false);
        anim.SetBool("Idle", true);
    }
    public void TakeDamage()
    {
        if (health < 1) return;
        anim.SetBool("Run", false);
        anim.SetBool("Slide", false);
        if (--health == 0)
        {
            StopAllCoroutines();
            StartCoroutine(Death());
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
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage();
        else if (Input.GetKeyDown(KeyCode.E) && !inAir)
        {
            Attack();
            rb.velocity = new Vector2(0, rb.velocity.y);
            //StopAllCoroutines();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            //StopAllCoroutines();

        }
        else if (Input.GetKey(KeyCode.D) && !isSliding)
        {
            MoveRight();
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            //StopAllCoroutines();


        }
        else if (Input.GetKeyUp(KeyCode.D) && !isSliding)
        {
            anim.SetBool("Run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A) && !isSliding)
        {
            MoveLeft();
            this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            //StopAllCoroutines();

        }
        else if (Input.GetKeyUp(KeyCode.A) && !isSliding)
        {
            anim.SetBool("Run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }

    }
    void Slide()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Slide",true);
        isSliding = true;
    }

    public void UnSlide()
    {
        anim.SetBool("Slide", false);
        isSliding = false;
    }

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        health = maxHealth;
        cp.resumePoint = this.gameObject.transform.position;
    }

    private void MoveLeft()
    {

        if (rb.velocity.x > -5)
        {
            rb.velocity += moveSpeed * Time.deltaTime * new Vector2(-1f, 0);
        }
        if (!inAir) anim.SetBool("Run", true);
    }
    private void Jump()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Slide", false);
        if (canWallJump)
        {
            rb.velocity = wallVector;
            Debug.Log("WallJump");
        }
        else if (inAir && canDoubleJump)
        {
            anim.SetTrigger("Jump2");
            rb.velocity = doubleJumpHeight * Time.deltaTime * new Vector2(0, 7.5f);
            canDoubleJump = false;
        }
        else if (!inAir)
        {
            anim.SetTrigger("Jump1");
            rb.velocity = new Vector2(0, 7.5f) * Time.deltaTime * jumpHeight;
            inAir = true;
        }
        
    }
    private void MoveRight()
    {
        if (rb.velocity.x < 5)
        {
            rb.velocity += new Vector2(1f, 0) * Time.deltaTime * moveSpeed;
        }
        if (!inAir) anim.SetBool("Run", true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            inAir = false;
            canDoubleJump = true;
            canWallJump = false;
            anim.SetBool("Idle",true);
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
