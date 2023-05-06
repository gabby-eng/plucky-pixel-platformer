using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim; 
    private float xDir;
    private float runForce = 8f;
    private float jumpForce = 18f;
    private float jumpGravityScale = 4.5f;
    private float fallGravityScale = 14f;

    private enum MovementState { idling, running, jumping, doubleJumping, wallJumping, falling, hitting, dying };
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        xDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xDir * runForce, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (xDir < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (xDir > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idling;
        }

        if (rb.velocity.y > .1f)
        {
            rb.gravityScale = jumpGravityScale;
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            rb.gravityScale = fallGravityScale;
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);

    }

}
