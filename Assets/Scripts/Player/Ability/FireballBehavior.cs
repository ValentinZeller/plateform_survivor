using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player.Ability
{
    public class FireballBehavior : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;

        private float lifespan = 1.5f;
        private Vector2 velocity;
        private float strength = 1;

        private void Start()
        {
            Destroy(gameObject, lifespan + PlayerStat.currentStats["Duration"]);
            velocity = rb.velocity;
            velocity.x += velocity.x * PlayerStat.currentStats["ProjectileSpeed"];

            transform.localScale += transform.localScale * PlayerStat.currentStats["Size"];
        }
        
        private void Update()
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
                collision.gameObject.GetComponent<IDamageable>().Damage(strength);
                EventManager.Trigger("damage_done", new TypedDamage("Fireball", strength));
                Explode();
            }
            
            if (collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x <= -1)
            {
                Explode();
            }
        }

        private void Explode()
        {
            Destroy(gameObject);
        }

        public void SetStrength(float newStrength)
        {
            strength = newStrength;
        }
    }
}
