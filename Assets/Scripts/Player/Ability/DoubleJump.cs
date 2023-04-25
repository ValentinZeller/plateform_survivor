using PlateformSurvivor.Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Player.Ability
{
    public class DoubleJump : MonoBehaviour
    {
        private float doubleJumpForce = 10f;
        private bool canDoubleJump;
        private bool canRegain;
        private bool isEvolved;

        private int maxAmount = 1;
        private int currentAmount = 1;

        private Rigidbody2D rb;
        private PlayerMovement playerMovement;
        private Dash dash;
        private PlayerStat stat;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovement>();
            dash = GetComponent<Dash>();
            stat= GetComponent<PlayerStat>();
            EventManager.AddListener("add_doublejump", OnAddDoubleJump);
            EventManager.AddListener("evolution_doublejump", OnEvolution);
            EventManager.AddListener("bounce_enemy", OnBounce);
        }
        
        void Update()
        {
            if (dash.enabled && dash.GetDashing()) return;

            if (playerMovement.IsGrounded() && !Input.GetButton("Jump"))
            {
                currentAmount = maxAmount;
            }
        }

        private void OnBounce()
        {
            if (currentAmount < maxAmount && canRegain)
            {
                currentAmount++;
            }

            if (isEvolved)
            {
                EventManager.Trigger("regen_health", 2f);
            }
        }

        private void OnAddDoubleJump(object data)
        {
            int level = (int)data;
            switch(level)
            {
                case 2:
                    canRegain = true;
                    break;
                case 3:
                    doubleJumpForce += doubleJumpForce * 0.05f;
                    break;
                case 4:
                    doubleJumpForce += doubleJumpForce * 0.05f;
                    break;
                case 5:
                    maxAmount++;
                    break;
                case 6:
                    doubleJumpForce += doubleJumpForce * 0.05f;
                    break;
                case 7:
                    doubleJumpForce += doubleJumpForce * 0.05f;
                    break;
                case 8:
                    doubleJumpForce += doubleJumpForce * 0.05f;
                    break;
            }
        }

        private void OnEvolution()
        {
            isEvolved = true;
        }

        public void DoubleJumpMove()
        {
            if (currentAmount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce + stat.currentStats["JumpForce"] * 2 / 100);
                currentAmount--;
                playerMovement.SetJumpBufferTimeCounter(0);
            }
        }
    }
}
