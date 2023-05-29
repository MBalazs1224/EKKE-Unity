using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField]
    Sprite saveSprite;
    [SerializeField]
    Sprite powerUpSprite;
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
    public bool isSliding;
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
    private GameObject notification;
    private float dashCooldown = 5f;


    BoxCollider2D playerCollider;

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
    private void FixedUpdate()
    {
        if (isDead() || isHurting || isSaving) return;
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


        if (Input.GetKeyDown(KeyCode.S) && !inAir && !isSliding)
        {
            Slide();

        }
        if (isOnWall)
        {
            this.transform.position -= moveSpeed * Time.deltaTime * new Vector3(0, .01f);
        }
    }

    private void Update()
    {
       

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
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        Debug.Log("Can dash!");
        canDash = true;
    }
    void UnDash()
    {
        moveSpeed /= 2;
    }

    void Slide()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Slide", true);
        isSliding = true;
        playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y / 2);
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y * 6);
    }

    public void UnSlide()
    {
        anim.SetBool("Slide", false);
        isSliding = false;
        playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y * 2);
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y / 6);

    }

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        health = maxHealth;
        cp.resumePoint = this.gameObject.transform.position;
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        this.notification = GameObject.Find("Notification");
        notification.SetActive(false);
    }

    private void MoveLeft()
    {

        rb.position += new Vector2(-0.07f, 0) * moveSpeed * Time.deltaTime;
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
            rb.velocity = new Vector2(0, jumpHeight) * Time.deltaTime;
            //rb.AddForce(new Vector2(0, jumpHeight));
            inAir = true;
        }

    }
    private void MoveRight()
    {
        rb.position += new Vector2(0.07f, 0) * Time.deltaTime * moveSpeed;

        if (!inAir && !isSliding) anim.SetBool("Run", true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
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
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            //rb.gravityScale = 1;
            isOnWall = false;
            inAir = true;
            anim.SetBool("WallStuck", false);
            anim.SetBool("Fall", true);
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint") && Input.GetKeyDown(KeyCode.G))
        {
            if (isSaving || inAir || isSliding) return;
            Debug.Log("Save");
            anim.SetTrigger("Graffiti");
            isSaving = true;
            cp.resumePoint = collision.gameObject.transform.position;
            Animator saveAnim = collision.gameObject.GetComponent<Animator>();
            saveAnim.SetTrigger("Save");
            SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
            sc.StartCoroutine(RemoveSave(collision.gameObject));
            
            collision.gameObject.tag = "Checkpoint (saved)";
            notification.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint"))
        {
            canSave = true;
            notification.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint"))
        {
            canSave = false;
            notification.SetActive(false);
        }
    }
    IEnumerator RemoveSave(GameObject collision)
    {
        yield return new WaitForSeconds(.5f);
        SpriteRenderer r = collision.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        r.sprite = saveSprite;
        yield return new WaitForSeconds(.53f);
        isSaving = false;
    }
}
