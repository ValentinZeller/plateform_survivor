using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    private float lifespan = 1.5f;
    private Vector2 velocity;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        Destroy(this.gameObject, lifespan);
        velocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < velocity.y)
        {
            rb.velocity = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = new Vector2(velocity.x, -velocity.y);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(collision.gameObject);
            Explode();
        }

        if (collision.contacts[0].normal.x != 0 && collision.gameObject.tag != "Player")
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
