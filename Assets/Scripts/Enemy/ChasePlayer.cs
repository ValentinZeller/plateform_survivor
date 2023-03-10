using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] EnemyStatObject enemy;
    private Dictionary<string, float> stats = new();

    private float horizontal;
    private float vertical;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        horizontal = player.position.x - transform.position.x;
        vertical = player.position.y - transform.position.y;

        for (int i = 0; i < EnemyStatObject.Keys().Count; i++)
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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * stats["Speed"], vertical * stats["Speed"]);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(stats["Strength"]);
        }
    }
}
