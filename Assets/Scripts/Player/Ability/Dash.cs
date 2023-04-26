using System.Collections;
using PlateformSurvivor.Enemy;
using PlateformSurvivor.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Player.Ability
{
    public class Dash : MonoBehaviour, IEvolution
    {
        private bool canDash = true;
        private bool isDashing = false;
        private float dashForce = 20f;
        private float dashTime = 0.2f;
        private float dashCooldown = 1f;
        private float knockback = 1.5f;

        private TrailRenderer tr;
        private Rigidbody2D rb;
        private PlayerStat stat;

        private void Start()
        {
            tr = GetComponent<TrailRenderer>();
            rb = GetComponent<Rigidbody2D>();
            stat= GetComponent<PlayerStat>();
            EventManager.AddListener("add_dash", OnAddDash);
            GetComponent<PlayerInput>().actions.FindAction("Dash").Enable();
        }

        private IEnumerator DashAction()
        {
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            tr.emitting = true;

            yield return new WaitForSeconds(dashTime);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (isDashing)
                {
                    collision.gameObject.GetComponent<IDamageable>().Damage(stat.currentStats["Strength"]);
                    if (collision.gameObject == null)
                    {
                        return;
                    }
                    Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
                    enemyBehavior.SetMove(false);

                    Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0);
                    enemyRb.AddForce(direction * knockback);
                    StartCoroutine(ResetKnockback(enemyBehavior));
                }
            }
        }

        private IEnumerator ResetKnockback(EnemyBehavior enemyBehavior)
        {
            yield return new WaitForSeconds(0.8f);
            enemyBehavior.SetMove(true);
        }

        private void OnAddDash(object data)
        {
            int level = (int)data;
            switch (level)
            {
                case 2:
                    dashForce += dashForce * 5 / 100;
                    break;
                case 3:
                    dashCooldown -= 0.125f;
                    break;
                case 4:
                    dashTime -= 0.1f;
                    break;
                case 5:
                    dashCooldown -= 0.125f;
                    knockback += 0.25f;
                    break;
                case 6:
                    dashCooldown -= 0.125f;
                    break;
                case 7:
                    dashForce += dashForce * 5 / 100;
                    knockback += 0.25f;
                    break;
                case 8:
                    dashCooldown -= 0.125f;
                    break;
            }
        }
        
        public bool GetDashing()
        {
            return isDashing;
        }

        public void DashMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && canDash && !isDashing)
            {
                StartCoroutine(DashAction());
            }
        }

        public bool IsEvolved { get; set; }
        public void OnEvolution()
        {
            IsEvolved = true;
        }
    }
}
