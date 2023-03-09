using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool canDamage = true;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [HideInInspector] public float jumpBufferTime = 0.2f;
    [HideInInspector] public float jumpBufferTimeCounter;

    private float bounceForce = 6f;

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

        if(canDamage)
        {
            StartCoroutine(CanDamageEnemy());
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

    private IEnumerator CanDamageEnemy()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer);
        if (collider2D)
        {
            canDamage = false;
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
            EventManager.Trigger("bounce_enemy");
            collider2D.gameObject.GetComponent<EnemyBehavior>().Damage(stat.currentStats["Strength"]);
            yield return new WaitForSeconds(0.2f);
            canDamage = true;
        }
        yield return null;
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
}
