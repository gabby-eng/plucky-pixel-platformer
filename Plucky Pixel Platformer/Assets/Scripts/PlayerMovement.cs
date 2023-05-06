using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim; 
    private float xDir;
    private float yDir;
    private float runForce = 8f;
    private float jumpForce = 14f;
    private bool isGrounded = true;

    [SerializeField] private float jumpGravityScale = 5f;
    [SerializeField] private float fallGravityScale = 10f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        xDir = Input.GetAxisRaw("Horizontal");
        yDir = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(xDir * runForce, rb.velocity.y);

        if (xDir < 0)
        {
            Debug.Log("Going left!");
            sprite.flipX = true;
        } else if (xDir > 0)
        {
            Debug.Log("Going right!");
            sprite.flipX = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jumping once!");
            Jump();
            isGrounded = true;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    private void Jump()
    {
        rb.gravityScale = jumpGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }
}
