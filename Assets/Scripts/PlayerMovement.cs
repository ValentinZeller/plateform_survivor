using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpForce = 16f;
    private bool isFacingRight = true;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter;

    private float doubleJumpForce = 12f;
    private bool canDoubleJump;

    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private TrailRenderer tr;

    private bool dashUnlocked = false;
    private bool doubleJumpUnlocked = false;

    public float getHorizontal()
    {
        return horizontal;
    }

    public bool getDashing()
    {
        return isDashing;
    }

    public void UnlockDoubleJump()
    {
        doubleJumpUnlocked = true;
    }

    public void UnlockDash()
    {
        dashUnlocked = true;
    }

    void Start()
    {
        
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            canDoubleJump = false;
        }

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimeCounter = jumpBufferTime;
            if ((coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f) || (canDoubleJump && doubleJumpUnlocked))
            {
                rb.velocity = new Vector2(rb.velocity.x, canDoubleJump ? doubleJumpForce : jumpForce);
                canDoubleJump = !canDoubleJump;
                jumpBufferTimeCounter = 0f;
            }

        } else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashUnlocked)
        {
            StartCoroutine(Dash());
        }

        if(OnEnemy())
        {
            Destroy(Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer).gameObject);
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool OnEnemy()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("TP"))
        {
            if (collision.gameObject.name == "Left")
            {
                transform.SetPositionAndRotation(new Vector3(34, transform.position.y, 0), transform.rotation);
            }
            else if (collision.gameObject.name == "Right")
            {
                transform.SetPositionAndRotation(new Vector3(-34, transform.position.y, 0), transform.rotation);
            }
        }
    }
}
