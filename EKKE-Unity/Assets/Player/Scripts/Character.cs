using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    int startRegenerate = 10;
    Checkpoint cp = new Checkpoint();
    [SerializeField]
    int maxHealth = 2;
    int health;
    Animator anim;
    bool inAir = false;
    bool canDoubleJump = false;
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
    int respawnTime = 10;
    private bool isOnWall;
    [SerializeField]
    float fallBackDistance = 5;

    bool wallOnRightSide = false;
    [SerializeField]

    float wallJumpSpeed = 2f;
    [SerializeField]
    private float wallJumpHeight = 10f;

    bool canDash = true;
    public int pigeonsKilled = 0;

    [SerializeField]
    float waitForAFK = 5f;

    float afkTime = 0;

    bool isHurting = false;
    private bool canSave;
    private bool isSaving;

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

    public void StartFallBack()
    {
        StartCoroutine(FallBack());
    }

    IEnumerator FallBack()
    {
        Debug.Log("Player fallback started!");
        var directon = this.transform.rotation == new Quaternion(0, 0, 0, 0) ? Vector3.left : Vector3.right;
        var hit = Physics2D.Raycast(this.transform.position - directon * 3, directon);
        Vector3 target = hit ? hit.transform.position : GetEndFallback();

        while (Vector3.Distance(this.transform.position,target) > .1f)
        {
            this.transform.position += Vector3.MoveTowards(this.transform.position,target,5);
            yield return null;
        }
        Debug.Log("Player fallback ended!");
    }

    private Vector3 GetEndFallback()
    {
        return this.transform.rotation == new Quaternion(0, 0, 0, 0) ? this.transform.position + new Vector3(fallBackDistance,0) : this.transform.transform.position + new Vector3(-fallBackDistance, 0);
    }

    public bool IsAttacking()
    {
        return Input.GetKeyDown(KeyCode.E);
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

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position + new Vector3(1,0), Vector2.right);

        if (hit && hit.transform.gameObject.name.Equals("5G Pigeon"))
        {
            Debug.Log("Pigeon hit!");
        }
    }
    public void TakeDamage()
    {
        if (isDead()) return;
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
            isHurting = true;
            //anim.ResetTrigger("Unhurt");
            //anim.ResetTrigger("Death");

            Debug.Log($"Taken damage! Remeaning health: {health}");
        }
    }
    public void ResetHurt()
    {
        anim.SetTrigger("Unhurt");
        isHurting = false;
    }

    IEnumerator StartRegen()
    {
        yield return new WaitForSeconds(startRegenerate);
        health = maxHealth;
        Debug.Log("Health is back to max value!");
    }

    private void Update()
    {
        if (isDead() || isHurting) return;
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) && !isOnWall)
        {

            anim.SetBool("Run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (!Input.anyKey)
        {
            afkTime += Time.deltaTime;
            if (afkTime >= waitForAFK)
            {
                StartAFK();
            }
            return;
        }
        afkTime = 0;
        if (Input.GetKeyDown(KeyCode.F10)) ReloadScene();
        else if (Input.GetKeyDown(KeyCode.F)) TakeDamage();
        else if (Input.GetKeyDown(KeyCode.E) && !inAir && !isOnWall)
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
        else if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isSliding)
        {
            Dash();
        }
        else if (Input.GetKey(KeyCode.D) && !isOnWall)
        {
            if (isOnWall)
            {
                anim.SetBool("WallStuck", false);
                anim.SetBool("Fall", true);
            }
            MoveRight();
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            //StopAllCoroutines();


        }

        else if (Input.GetKey(KeyCode.A))
        {
            if (isOnWall)
            {
                anim.SetBool("WallStuck", false);
                anim.SetBool("Fall", true);
            }
            MoveLeft();
            this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            //StopAllCoroutines();

        }


        if (Input.GetKeyDown(KeyCode.S) && !inAir)
        {
            Slide();

        }
        if (isOnWall)
        {
            this.transform.position -= moveSpeed * Time.deltaTime * new Vector3(0, .01f);
        }

    }

    private void StartSave()
    {
        isSaving = true;
        anim.SetTrigger("Save");
        cp.resumePoint = this.transform.position;
    }

    private void StartAFK()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Slide", false);
        afkTime = 0;

        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());

        double value = rnd.NextDouble();
        if (value < .33)
        {
            anim.SetTrigger("AFK1");
        }
        else if (value < .66)
        {
            anim.SetTrigger("AFK2");
        }
        else anim.SetTrigger("AFK3");
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Dash()
    {
        anim.SetTrigger("Dash");
        moveSpeed *= 2;
        canDash = false;
    }
    void UnDash()
    {
        moveSpeed /= 2;
        canDash = true;
    }

    void Slide()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Slide", true);
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

        rb.position += new Vector2(-0.05f, 0) * moveSpeed * Time.deltaTime;
        if (!inAir && !isSliding) anim.SetBool("Run", true);
    }
    private void Jump()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Slide", false);
        if (isOnWall)
        {
            anim.SetBool("WallStuck", false);
            anim.SetBool("Fall", false);
            anim.SetTrigger("Jump1");
            Vector2 jumpDirection = new Vector2(-transform.localScale.x * wallJumpSpeed, wallJumpHeight);
            rb.velocity += jumpDirection;
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
        rb.position += new Vector2(0.05f, 0) * Time.deltaTime * moveSpeed;

        if (!inAir && !isSliding) anim.SetBool("Run", true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            inAir = false;
            canDoubleJump = true;
            canWallJump = false;
            anim.SetBool("Fall", false);
            anim.SetBool("Idle", true);
            isOnWall = false;

        }
        else if (collision.gameObject.layer == 7)
        {
            isOnWall = true;
            anim.SetBool("Run", false);
            anim.SetBool("Idle", false);
            anim.SetBool("WallStuck", true);
            anim.SetBool("Fall", false);
            rb.velocity = new Vector2(0, 0);
            canDoubleJump = true;
            inAir = false;
            if (WallOnRightSide())
            {
                this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
        else if(collision.gameObject.layer == 8)
        {
            canSave = true;
        }

    }

    private bool WallOnRightSide()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.right);

        return hit && hit.transform.position.x >= this.transform.position.x;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rb.gravityScale = 1;
            isOnWall = false;
            inAir = true;
            anim.SetBool("WallStuck", false);
            anim.SetBool("Fall", true);
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (collision.gameObject.layer == 8)
        {
            canSave = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Save");
            if (isSaving) return;
            isSaving = true;
            cp.resumePoint = collision.gameObject.transform.position;
            Animator saveAnim = collision.gameObject.GetComponent<Animator>();
            saveAnim.SetTrigger("Save");
            SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
            sc.StartCoroutine(RemoveSave());
        }
    }
    IEnumerator RemoveSave()
    {
        yield return new WaitForSeconds(1.03f);
        isSaving = false;
    }
}
