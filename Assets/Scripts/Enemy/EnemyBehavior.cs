using System.Collections;
using System.Collections.Generic;
using PlateformSurvivor.InteractiveObject;
using PlateformSurvivor.Player.Ability;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class EnemyBehavior : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject xpObject;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private EnemyStatObject enemy; 
        
        private Dictionary<string, float> stats = new();
        private bool canMove = true;
        private float horizontal;
        private bool isFacingRight = true;

        private void Start()
        {
            horizontal = 0.5f;

            for (int i = 0; i < EnemyStatObject.Keys().Count; i++ )
            {
                stats.Add(EnemyStatObject.Keys()[i], enemy[i]);
            }
        }
        
        private void Update()
        {
            Flip();

            if (stats["JumpForce"] > 0 && IsGrounded())
            {
                StartCoroutine(Jump());
            }

            if (rb.velocity.y > 0f)
            {
                Vector2 velocity = rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                rb.velocity = velocity;
            }
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                if (rb.isKinematic)
                {
                    rb.velocity = new Vector2(horizontal * stats["Speed"], 0);
                }
                else if (IsGrounded())
                {
                    rb.velocity = new Vector2(horizontal * stats["Speed"], 0);
                }
            }
        
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

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Dash dash = collision.gameObject.GetComponent<Dash>();
                if (dash.enabled && dash.GetDashing())
                {
                    return;
                }
                collision.gameObject.GetComponent<IDamageable>().Damage(stats["Strength"]);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                horizontal = -horizontal;
            }
        }
        
        public void SetMove(bool newState)
        {
            canMove = newState;
        }
        
        public void Damage(float damage)
        {
            stats["Health"] -= damage;
            if (stats["Health"] <= 0)
            {
                GameObject xpInstance = Instantiate(xpObject, transform.position, Quaternion.identity);
                xpInstance.GetComponent<Item>().value = enemy.xpDrop;
                Destroy(gameObject);
            }
        }
    }
}
