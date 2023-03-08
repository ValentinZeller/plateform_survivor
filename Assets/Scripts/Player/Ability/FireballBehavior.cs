using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    private float lifespan = 1.5f;
    private Vector2 velocity;
    private float strength = 1;
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

    public void SetStrength(float newValue)
    {
        strength = newValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = new Vector2(velocity.x, -velocity.y);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(strength);
            Explode();
        }

        if (collision.contacts[0].normal.x != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
