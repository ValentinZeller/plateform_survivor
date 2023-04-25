using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class BulletBehavior : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EnemyStatObject stat;
        private float direction;
        
        private void Update()
        {
            rb.velocity = new Vector2(stat.speed * direction, rb.velocity.y);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(stat.strength);
                
            }
            Explode();
        }
        
        private void Explode()
        {
            Destroy(gameObject);
        }

        public void SetDirection(float value)
        {
            direction = value;
        }

    }
}
