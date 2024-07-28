using System;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class DropSpecialLoot : MonoBehaviour
    {
        [SerializeField] private GameObject loot;
        private const float Offset = 0.2f;
        private bool isQuitting = false;

        void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void OnDestroy()
        {
            if (!isQuitting)
            {
                var position = transform.position;
                Vector3 dropPos = new Vector3(position.x, position.y - Offset, position.z);
                Instantiate(loot, dropPos, Quaternion.identity, transform.parent);
            }
        }
    }
}