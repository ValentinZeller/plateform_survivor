using System;
using System.Collections;
using System.Collections.Generic;
using PlateformSurvivor.Player;
using UnityEngine;

namespace PlateformSurvivor
{
    public class Spike : MonoBehaviour
    {
        private const float BounceForce = 3f;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, BounceForce);
                other.gameObject.GetComponent<PlayerStat>().Damage(1f);
            }
        }
    }
}
