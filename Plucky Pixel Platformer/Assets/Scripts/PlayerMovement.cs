using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim; 

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform checkGround;


    private float xDir = 0f;
    private float runForce = 8f;
    private float jumpForce = 10f;
    private float jumpGravityScale = 4f;
    private float fallGravityScale = 14f;
    private float checkRadius = 0.3f;
    private float jumpTimeCounter;
    private float jumpTime = 0.2f;

    private bool isJumping;

    private int extraJumps;

    private enum MovementState { idling, running, jumping, doubleJumping, wallJumping, falling, hitting, dying };
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        xDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xDir * runForce, rb.velocity.y);

        if (isGrounded())
        {
            extraJumps = 1;
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumps--;
        } else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (xDir < 0f)
        {
            Debug.Log("Left");
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (xDir > 0f)
        {
            Debug.Log("Right");
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idling;
        }

        if (rb.velocity.y > .1f)
        {
            Debug.Log("Jump");
            rb.gravityScale = jumpGravityScale;
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            Debug.Log("Fall");
            rb.gravityScale = fallGravityScale;
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(checkGround.position, checkRadius, jumpableGround);
    }

}
