using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    static bool inAir = false;
    static bool canDoubleJump = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        if (rb.velocity.x > -5)
        {
            rb.velocity += new Vector2(-.1f, 0);
        }
    }

    private void MoveRight()
    {
        if (rb.velocity.x < 5)
        {
            rb.velocity += new Vector2(.1f,0);
        }
    }

    private void Jump()
    {
        if (inAir && canDoubleJump)
        {
            rb.velocity += new Vector2(0, 5f);
            canDoubleJump = false;
        }
        else if (!inAir)
        {
            rb.velocity += new Vector2(0, 7.5f);
            inAir = true;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            inAir = false;
            canDoubleJump = true;
        }
    }
}
