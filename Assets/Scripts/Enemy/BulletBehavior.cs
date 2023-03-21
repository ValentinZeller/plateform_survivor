using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class BulletBehavior : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private float strength = 1;
        private float speed = 8f;
        private float direction;
        
        private void Update()
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(strength);
                
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
