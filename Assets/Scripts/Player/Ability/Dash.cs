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

        private void Start()
        {
            tr = GetComponent<TrailRenderer>();
            rb = GetComponent<Rigidbody2D>();
            EventManager.AddListener("add_dash", OnAddDash);
            GetComponent<PlayerInput>().actions.FindAction("Dash").Enable();
            EventManager.AddListener("evolution_dash",OnEvolution);
        }

        private void Update()
        {
            if (isDashing && IsEvolved)
            {
                Collider2D enemy = Physics2D.OverlapCircle(transform.position, 2);
                if (Physics2D.OverlapCircle(transform.position, 2) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemy.gameObject.GetComponent<IDamageable>().Damage(PlayerStat.currentStats["Strength"]);
                }
            }
        }

        private IEnumerator DashAction()
        {
            if (IsEvolved)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"),true);
            }
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            tr.emitting = true;

            yield return new WaitForSeconds(dashTime + PlayerStat.currentStats["Duration"]);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            if (IsEvolved)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"),false);
            }

            yield return new WaitForSeconds(dashCooldown - dashCooldown * PlayerStat.currentStats["Cooldown"]);
            canDash = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (isDashing)
                {
                    collision.gameObject.GetComponent<IDamageable>().Damage(PlayerStat.currentStats["Strength"]);
                    if (collision.gameObject == null)
                    {
                        return;
                    }
                    Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
                    enemyBehavior.SetMove(false);

                    Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0);
                    enemyRb.AddForce(direction * knockback);

                    EventManager.Trigger("damage_done", new TypedDamage("Dash", PlayerStat.currentStats["Strength"]));
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
                    dashForce += dashForce * 5 / 100;
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
