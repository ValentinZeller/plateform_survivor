using System;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class DropSpecialLoot : MonoBehaviour
    {
        [SerializeField] private GameObject loot;
        private const float Offset = 0.2f;
        private void OnDestroy()
        {
            var position = transform.position;
            Vector3 dropPos = new Vector3(position.x, position.y - Offset, position.z);
            Instantiate(loot, dropPos, Quaternion.identity);
        }
    }
}