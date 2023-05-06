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

    private enum MovementStates { idling, running, jumping, doubleJumping, wallJumping, falling, hitting, dying };
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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetInteger("state", (int)MovementStates.jumping);
            Jump();
            isGrounded = true;
        }

        if (rb.velocity.y < 0f)
        {
            rb.gravityScale = fallGravityScale;
        }

        UpdateAnimationState();
    }

    private void Jump()
    {
        rb.gravityScale = jumpGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    private void UpdateAnimationState()
    {
        if (xDir < 0f)
        {
            anim.SetInteger("state", (int)MovementStates.running);
            sprite.flipX = true;
        }
        else if (xDir > 0f)
        {
            anim.SetInteger("state", (int)MovementStates.running);
            sprite.flipX = false;
        }
        else
        {
            anim.SetInteger("state", (int)MovementStates.idling);
        }
    }
}
