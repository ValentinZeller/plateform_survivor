using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        dash = GetComponent<Dash>();
        EventManager.AddListener("add_doublejump", OnAddDoubleJump);
        EventManager.AddListener("bounce_enemy", OnBounce);
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
            currentAmount = maxAmount;
        }

        if (Input.GetButtonDown("Jump") && !playerMovement.IsGrounded())
        {
            if (currentAmount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                currentAmount--;
                playerMovement.jumpBufferTimeCounter = 0f;
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
                doubleJumpForce += doubleJumpForce * 5 / 100;
                break;
            case 4:
                doubleJumpForce += doubleJumpForce * 5 / 100;
                break;
            case 5:
                maxAmount++;
                break;
            case 6:
                doubleJumpForce += doubleJumpForce * 5 / 100;
                break;
            case 7:
                doubleJumpForce += doubleJumpForce * 5 / 100;
                break;
            case 8:
                doubleJumpForce += doubleJumpForce * 5 / 100;
                break;
        }
    }
}
