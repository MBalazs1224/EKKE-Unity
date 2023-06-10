using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;

public class Character : MonoBehaviour
{
    [SerializeField]
    Sprite saveSprite;
    [SerializeField]
    Sprite powerSprite;
    [SerializeField]
    Sprite powerUpSprite;
    int regenerationWaitTime = 15;
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
    private bool CheckpointAnimationRunning;
    private GameObject notification;
    private float dashCooldown = 5f;


    BoxCollider2D playerCollider;

    InputMaster controllerInput;

    Vector2 controllerMoveValue = new Vector2(0, 0);
    AudioSource audioSource;

    [SerializeField]
    float keyboardMovementSpeed = 1f;



    CameraController camController;

    GameObject nearestCheckpoint;

    bool nearPowerPoint = false;
    private bool isStomping;
    private bool isPaused = false;

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

        while (Vector3.Distance(this.transform.position, target) > .1f)
        {
            this.transform.position += Vector3.MoveTowards(this.transform.position, target, 5);
            yield return null;
        }
        Debug.Log("Player fallback ended!");
    }

    private Vector3 GetEndFallback()
    {
        return this.transform.rotation == new Quaternion(0, 0, 0, 0) ? this.transform.position + new Vector3(fallBackDistance, 0) : this.transform.transform.position + new Vector3(-fallBackDistance, 0);
    }

    public bool IsAttacking()
    {
        return Input.GetKeyDown(KeyCode.E) || isStomping;
    }

    private void Respawn()
    {

        this.gameObject.transform.position = cp.resumePoint;
        anim.SetBool("Idle", true);
        health = maxHealth;
        UIController.Heal();
        UIController.CanDash();
    }

    public bool isDead() => health == 0;
    void Attack()
    {
        if (!inAir && !isOnWall)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Run", false);
            anim.SetBool("Slide", false);
            anim.SetBool("Idle", true);
        }

    }
    public void TakeDamage()
    {
        if (isDead()) return;
        anim.SetBool("Run", false);
        anim.SetBool("Slide", false);
        camController.Shake();
        if (--health == 0)
        {
            StopAllCoroutines();
            StartCoroutine(Death());
            AudioController.PlayDeath(audioSource);
            Debug.Log($"Dead!");
            UIController.Death();
        }
        else
        {
            StopCoroutine(StartRegen());
            StartCoroutine(StartRegen());
            anim.SetTrigger("Hurt");
            UIController.TakeDamage();

            AudioController.PlayHurt(audioSource);
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
        yield return new WaitForSeconds(regenerationWaitTime);
        AudioController.PlayHeal(audioSource);
        health = maxHealth;
        Debug.Log("Health is back to max value!");
        UIController.Heal();

    }

    public void LowerRegenerationTime()
    {
        regenerationWaitTime -= 5;
        Debug.Log($"Regeneration time is down to : {regenerationWaitTime} seconds!");
    }

    private void FixedUpdate()
    {
        
    }

    private void ESC_Pressed()
    {

        if (!isPaused)
        {
            UIController.Pause();
            isPaused = true;
        }
        else
        {
            UIController.Unpause();
            isPaused = false;
        }
    }

    private void Update()
    {
        if (isDead() || isHurting || CheckpointAnimationRunning || isStomping) return;

        Move(controllerMoveValue);

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) && !isOnWall)
        {

            anim.SetBool("Run", false);
        }
        //if (!Input.anyKey)
        //{
        //    afkTime += Time.deltaTime;
        //    if (afkTime >= waitForAFK)
        //    {
        //        StartAFK();
        //    }
        //    return;
        //}
        //afkTime = 0;
        if (Input.GetKeyDown(KeyCode.F10)) ReloadScene();
        else if (Input.GetKeyDown(KeyCode.Escape)) ESC_Pressed();

        else if (Input.GetKeyDown(KeyCode.B)) camController.Shake();
        else if (Input.GetKeyDown(KeyCode.F)) TakeDamage();

        else if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
            rb.velocity = new Vector2(0, rb.velocity.y);
            //StopAllCoroutines();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Button_A_Press();
            //StopAllCoroutines();

        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
        else if (Input.GetKey(KeyCode.D))
        {

            Move(new Vector2(keyboardMovementSpeed, 0));
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            //StopAllCoroutines();


        }

        else if (Input.GetKey(KeyCode.A))
        {

            Move(new Vector2(-keyboardMovementSpeed, 0));
            this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            //StopAllCoroutines();

        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            ButtonDownPress();

        }
        if (isOnWall)
        {
            this.transform.position -= moveSpeed * Time.deltaTime * new Vector3(0, .01f);
        }

        notification.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 5);


    }

    private void StartSave()
    {
        CheckpointAnimationRunning = true;
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
        if (canDash && !isSliding)
        {
            anim.SetTrigger("Dash");
            UIController.CantDash();
            moveSpeed *= 2;
            canDash = false;
            StartCoroutine(DashCooldown());
            //StartCoroutine(UnDash());
        }

    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        Debug.Log("Can dash!");
        AudioController.PlayDash(audioSource);
        canDash = true;
        UIController.CanDash();


    }
    void UnDash()
    {
        moveSpeed /= 2;

    }

    void ButtonDownPress()
    {
        if (isStomping || isSliding) return;
        if (inAir)
        {
            anim.SetTrigger("Stomp");
            isStomping = true;
        }

        else if (!StandingStill())
        {
            anim.SetBool("Run", false);
            anim.SetBool("Slide", true);
            isSliding = true;
            playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y / 2);
            playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y * 6);
            StartCoroutine(UnSlide());
        }

    }

    IEnumerator UnSlide()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        anim.SetBool("Slide", false);
        isSliding = false;
        playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y * 2);
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y / 6);
    }


    private bool StandingStill()
    {
        return !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && controllerMoveValue.x == 0;
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
        controllerInput = new InputMaster();
        controllerInput.Player.Enable();
        controllerInput.Player.Jump.started += ctx => Button_A_Press();
        controllerInput.Player.Dash.started += ctx => Dash();
        controllerInput.Player.Attack.started += ctx => Attack();
        controllerInput.Player.MoveRight.performed += ctx => controllerMoveValue = ctx.ReadValue<Vector2>() / 15;
        controllerInput.Player.MoveLeft.performed += ctx => controllerMoveValue = ctx.ReadValue<Vector2>() / 15;
        controllerInput.Player.MoveRight.canceled += ctx => controllerMoveValue = new Vector2(0, 0);
        controllerInput.Player.MoveLeft.canceled += ctx => controllerMoveValue = new Vector2(0, 0);
        controllerInput.Player.Slide.started += ctx => ButtonDownPress();

        audioSource = this.gameObject.GetComponent<AudioSource>();

        camController = Camera.main.GetComponent<CameraController>();

    }

    private void Move(Vector2 distance)
    {

        if (distance.x < 0 && distance.x > 0.1f || distance.x >= 0 && distance.x < 0.01f)
        {
            anim.SetBool("Run", false);
            return;
        }
        if (isOnWall)
        {
            anim.SetBool("WallStuck", false);
            anim.SetBool("Fall", true);
        }
        if (!isSliding && !inAir) anim.SetBool("Run", true);
        distance.y = 0;
        if (distance.x > 0) this.transform.rotation = new Quaternion(0, 0, 0, 0);
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        rb.position += distance * moveSpeed * Time.deltaTime;
    }


    IEnumerator RemoveSave(GameObject collision)
    {
        yield return new WaitForSeconds(.5f);
        SpriteRenderer r = collision.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        r.sprite = saveSprite;
        yield return new WaitForSeconds(.53f);
        CheckpointAnimationRunning = false;
    }
    IEnumerator PowerUp(GameObject collision)
    {
        yield return new WaitForSeconds(.5f);
        SpriteRenderer r = collision.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        r.sprite = powerSprite;
        yield return new WaitForSeconds(.51f);
        CheckpointAnimationRunning = false;
    }

    private void Button_A_Press()
    {
        if (canSave || nearPowerPoint)
        {
            if (CheckpointAnimationRunning && inAir && isSliding) return;
            if (canSave)
            {
                Debug.Log("Save");
                anim.SetTrigger("Graffiti");
                CheckpointAnimationRunning = true;
                cp.resumePoint = nearestCheckpoint.transform.position;
                Animator saveAnim = nearestCheckpoint.GetComponent<Animator>();
                saveAnim.SetTrigger("Save");
                SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
                sc.StartCoroutine(RemoveSave(nearestCheckpoint.gameObject));
                nearestCheckpoint.gameObject.tag = "Checkpoint (saved)";
                notification.SetActive(false);
                canSave = false;
                UIController.ActivateSaveNotification();
            }
            else
            {
                Debug.Log("Powerup");
                anim.SetTrigger("Graffiti");
                CheckpointAnimationRunning = true;
                LowerRegenerationTime();
                Animator powerAnim = nearestCheckpoint.GetComponent<Animator>();
                powerAnim.SetTrigger("Power");
                SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
                sc.StartCoroutine(PowerUp(nearestCheckpoint.gameObject));
                nearestCheckpoint.gameObject.tag = "Powerpoint (activated)";
                notification.SetActive(false);
                nearPowerPoint = false;
                UIController.ActivatePowerNotification();
            }


            return;
        }



        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Slide", false);
        if (isOnWall)
        {
            anim.SetBool("WallStuck", false);
            anim.SetTrigger("Jump1");

            int multiplier = transform.rotation.y == 0 ? 1 : -1;

            Vector2 jumpDirection = new Vector2(wallJumpSpeed * multiplier, wallJumpHeight) * Time.deltaTime;
            rb.velocity += jumpDirection;
        }
        else if (inAir && canDoubleJump)
        {
            anim.SetTrigger("Jump2");
            rb.velocity += new Vector2(0, doubleJumpHeight) * Time.deltaTime;
            canDoubleJump = false;
        }
        else if (!inAir)
        {
            Debug.Log("Jump");
            anim.SetTrigger("Jump1");
            rb.velocity += new Vector2(0, jumpHeight) * Time.fixedDeltaTime;

            Debug.Log($"Velocity: {rb.velocity}");
            //rb.AddForce(new Vector2(0, jumpHeight));
            inAir = true;
        }

    }

    IEnumerator ResetIsStomping()
    {
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        isStomping = false;
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
            if (isStomping)
            {
                anim.SetTrigger("StompFinish");
                StartCoroutine(ResetIsStomping());
            }

        }
        else if (collision.gameObject.layer == 10)
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
        if (collision.gameObject.layer == 10)
        {
            //rb.gravityScale = 1;
            isOnWall = false;
            inAir = true;
            anim.SetBool("WallStuck", false);
            anim.SetBool("Fall", true);
            this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint"))
        {
            canSave = true;
            notification.SetActive(true);
            nearestCheckpoint = collision.gameObject;
        }
        else if (collision.gameObject.tag.Equals("Powerpoint"))
        {
            nearPowerPoint = true;
            notification.SetActive(true);
            nearestCheckpoint = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Checkpoint"))
        {
            canSave = false;
            notification.SetActive(false);
        }
        else if (collision.gameObject.tag.Equals("Powerpoint"))
        {
            nearPowerPoint = false;
            notification.SetActive(false);
        }
    }

}
