using System.Collections;
using PlateformSurvivor.Enemy;
using PlateformSurvivor.Player.Ability;
using PlateformSurvivor.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private PlayerStat stat;
        [SerializeField] private Dash dash;
        [SerializeField] private PlayerInput playerInput;
        
        private const float BounceForce = 6f;
        private const float CoyoteTime = 0.2f;
        private const float JumpBufferTime = 0.2f;
        
        private float horizontal;
        private bool isFacingRight = true;
        private bool canDamage = true;
        private float coyoteTimeCounter;
        private float jumpBufferTimeCounter;

        private void Update()
        {
            if (dash.enabled && dash.GetDashing()) return;

            //horizontal = Input.GetAxis("Horizontal");

            if (IsGrounded())
            {
                coyoteTimeCounter = CoyoteTime;
            } else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            //jumpBufferTimeCounter -= Time.deltaTime;

/*            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimeCounter = JumpBufferTime;
                if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, stat.currentStats["JumpForce"]);
                    jumpBufferTimeCounter = 0f;
                }

            } else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }*/

/*            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                Vector2 velocity = rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                rb.velocity = velocity;
                coyoteTimeCounter = 0f;
            }*/

            if(canDamage)
            {
                StartCoroutine(CanDamageEnemy());
            }

            Flip();
        }

        private void FixedUpdate()
        {
            if (dash.enabled)
            {
                if (dash.GetDashing())
                {
                    return;
                }
            }
            rb.velocity = new Vector2(horizontal * stat.currentStats["Speed"], rb.velocity.y);
        }

        private IEnumerator CanDamageEnemy()
        {
            Collider2D enemyCollider2D = Physics2D.OverlapCircle(groundCheck.position, 0.4f, enemyLayer);
            if (enemyCollider2D)
            {
                canDamage = false;
                rb.velocity = new Vector2(rb.velocity.x, BounceForce);
                EventManager.Trigger("bounce_enemy");
                enemyCollider2D.gameObject.GetComponent<EnemyBehavior>().Damage(stat.currentStats["Strength"]);
                yield return new WaitForSeconds(0.2f);
                canDamage = true;
            }
            yield return null;
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
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        public void SetJumpBufferTimeCounter(float value)
        {
            jumpBufferTimeCounter = value;
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && IsGrounded())
            {
                jumpBufferTimeCounter = JumpBufferTime;
                if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, stat.currentStats["JumpForce"]);
                    jumpBufferTimeCounter = 0f;
                }
            }


            if (ctx.canceled && rb.velocity.y > 0f)
            {
                Vector2 velocity = rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                rb.velocity = velocity;
                coyoteTimeCounter = 0f;
            }
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            horizontal = ctx.ReadValue<Vector2>().x;
        }
    }
}
