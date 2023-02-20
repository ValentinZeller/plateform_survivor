using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    private float doubleJumpForce = 10f;
    private bool canDoubleJump;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private Dash dash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        dash = GetComponent<Dash>();
    }

    // Update is called once per frame
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
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump") && !playerMovement.IsGrounded())
        {
            if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                canDoubleJump = !canDoubleJump;
                playerMovement.jumpBufferTimeCounter = 0f;
            }

        }

    }
}
