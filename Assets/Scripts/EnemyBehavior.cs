using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    private float horizontal;
    private float vertical;
    public float speed = 8f;
    public float strength = 1f;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    private Transform player;
    [SerializeField] GameObject coin;
    private bool isQuitting = false;

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        horizontal = player.position.x - transform.position.x;
        vertical = player.position.y - transform.position.y;
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
        if (rb.isKinematic)
        {
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        } else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        
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

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }   
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(strength);
        }
    }
}
