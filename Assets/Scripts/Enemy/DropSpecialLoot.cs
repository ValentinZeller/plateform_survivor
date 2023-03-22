using System;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class DropSpecialLoot : MonoBehaviour
    {
        [SerializeField] private GameObject loot;
        private void OnDestroy()
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}