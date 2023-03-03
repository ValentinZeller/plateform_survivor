using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyStatObject enemy;
    private Dictionary<string, float> stats = new();

    private bool canMove = true;
    private float horizontal;
    private float vertical;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    private Transform player;
    [SerializeField] GameObject coin;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        horizontal = player.position.x - transform.position.x;
        vertical = player.position.y - transform.position.y;

        for (int i = 0; i < EnemyStatObject.Keys().Count; i++ )
        {
            stats.Add(EnemyStatObject.Keys()[i], enemy[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Mathf.Clamp(player.position.y - transform.position.y, -0.5f, 0.5f);
        horizontal = Mathf.Clamp(player.position.x - transform.position.x, -0.5f, 0.5f);
        Flip();

        if (stats["JumpForce"] > 0 && IsGrounded())
        {
            StartCoroutine(Jump());
        }

        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (rb.isKinematic)
            {
                rb.velocity = new Vector2(horizontal * stats["Speed"], vertical * stats["Speed"]);
            }
            else
            {
                rb.velocity = new Vector2(horizontal * stats["Speed"], rb.velocity.y);
            }
        }
        
    }

    public void SetMove(bool newState)
    {
        canMove = newState;
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(stats["JumpCooldown"]);
        rb.velocity = new Vector2(rb.velocity.x, stats["JumpForce"]);
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

    public void Damage(float damage)
    {
        stats["Health"] -= damage;
        if (stats["Health"] <= 0)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        } else
        {

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Dash>().enabled && collision.gameObject.GetComponent<Dash>().GetDashing())
            {
                return;
            }
            collision.gameObject.GetComponent<IDamageable>().Damage(stats["Strength"]);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(stats["Strength"]);
        }
    }
}
