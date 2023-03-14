using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player.Ability
{
    public class DoubleJump : MonoBehaviour
    {
        private float doubleJumpForce = 10f;
        private bool canDoubleJump;
        private bool canRegain;

        private int maxAmount = 1;
        private int currentAmount = 1;

        private Rigidbody2D rb;
        private PlayerMovement playerMovement;
        private Dash dash;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovement>();
            dash = GetComponent<Dash>();
            EventManager.AddListener("add_doublejump", OnAddDoubleJump);
            EventManager.AddListener("bounce_enemy", OnBounce);
        }
        
        void Update()
        {
            if (dash.enabled)
            {
                if (dash.GetDashing())
                {
                    return;
                }
            }

            if (playerMovement.IsGrounded() && !Input.GetButton("Jump"))
            {
                currentAmount = maxAmount;
            }

            if (Input.GetButtonDown("Jump") && !playerMovement.IsGrounded())
            {
                if (currentAmount > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                    currentAmount--;
                    playerMovement.SetJumpBufferTimeCounter(0);
                }

            }

        }

        private void OnBounce()
        {
            if (currentAmount < maxAmount && canRegain)
            {
                currentAmount++;
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
    }
}
