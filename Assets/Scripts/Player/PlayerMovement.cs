using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [HideInInspector] public float jumpBufferTime = 0.2f;
    [HideInInspector] public float jumpBufferTimeCounter;

    private float bounceForce = 12f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private PlayerStat stat;
    [SerializeField] private Dash dash;

    void Start()
    {
        
    }
    void Update()
    {
        if (dash.enabled)
        {
            if (dash.GetDashing())
            {
                return;
            }
        }

        horizontal = Input.GetAxis("Horizontal");

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
            if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, stat.currentStats["JumpForce"]);
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

        if(OnEnemy())
        {
            Destroy(Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer).gameObject);
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (dash.enabled)
        {
            if (dash.GetDashing())
            {
                return;
            }
        }
        rb.velocity = new Vector2(horizontal * stat.currentStats["Speed"], rb.velocity.y);
    }

    private bool OnEnemy()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer);
    }

    public bool IsGrounded()
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
