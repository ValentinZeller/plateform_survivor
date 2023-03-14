using System.Collections.Generic;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class ChasePlayer : MonoBehaviour
    {
        [SerializeField] EnemyStatObject enemy;
        [SerializeField] private Rigidbody2D rb;
        
        private readonly Dictionary<string, float> stats = new();
        private float horizontal;
        private float vertical;
        private bool isFacingRight = true;
        private Transform player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            Vector3 playerPosition = player.position;
            Vector3 position = transform.position;
            horizontal = playerPosition.x - position.x;
            vertical = playerPosition.y - position.y;

            for (int i = 0; i < EnemyStatObject.Keys().Count; i++)
            {
                stats.Add(EnemyStatObject.Keys()[i], enemy[i]);
            }
        }
        
        private void Update()
        {
            Vector3 playerPosition = player.position;
            Vector3 position = transform.position;
            vertical = Mathf.Clamp(playerPosition.y - position.y, -0.5f, 0.5f);
            horizontal = Mathf.Clamp(playerPosition.x - position.x, -0.5f, 0.5f);
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
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(stats["Strength"]);
            }
        }
    }
}
