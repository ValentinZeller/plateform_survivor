using System;
using PlateformSurvivor.InteractiveObject;
using PlateformSurvivor.Player;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class AbilityPickupDisplay : MonoBehaviour
    {
        private Item item; 
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            item = GetComponent<Item>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (spriteRenderer.sprite != item.abilityObject.sprite)
            {
                spriteRenderer.sprite = item.abilityObject.sprite;
            }
            
        }
    }
}