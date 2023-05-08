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

    private float xDir;
    private float runForce = 8f;
    private float jumpForce = 18f;
    private float jumpGravityScale = 4.5f;
    private float fallGravityScale = 14f;

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

        if (Input.GetButtonDown("Jump") && isGrounded())
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

    private bool isGrounded()
    {
        // coll is the boxCollider2D around the player
        // coll.bounds.center, coll.bounds.size creates the same size boxCollider2D around the player
        // 0f represents the rotation of the new boxCollider2D, 0f here because we want the same exact shape
        // Vector2.down, .1f moves the new boxCollider2D we created a little bit down towards the toes of the player
        // jumpableGround is the LayerMask we set on the terrain
        // meaning that if the new boxCollider2D we created (toe box) overlaps with jumpableGround, isGrounded returns true
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

}
