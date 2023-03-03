using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private bool canDash = true;
    private bool isDashing = false;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;
    private float knockback = 1.5f;

    private TrailRenderer tr;
    private Rigidbody2D rb;
    private PlayerStat stat;
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        stat= GetComponent<PlayerStat>();
    }

    public bool GetDashing()
    {
        return isDashing;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine(DashAction());
        }

    }

    private IEnumerator DashAction()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (isDashing)
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(stat.currentStats["Strength"]);
                if (collision.gameObject == null)
                {
                    return;
                }
                Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
                enemyBehavior.SetMove(false);

                Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0);
                enemyRb.AddForce(direction * knockback);
                StartCoroutine(ResetKnockback(enemyBehavior));
            }
        }
    }

    private IEnumerator ResetKnockback(EnemyBehavior enemyBehavior)
    {
        yield return new WaitForSeconds(0.8f);
        enemyBehavior.SetMove(true);
    }
}
