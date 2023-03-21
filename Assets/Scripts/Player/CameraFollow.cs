using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace PlateformSurvivor.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float minX = -20;
        [SerializeField] private float maxX = 16.5f;
        [SerializeField] private float minY = 4.5f;
        [SerializeField] private float maxY = 23.5f;
        [SerializeField] private Transform player;

        private void Update()
        {
            Vector3 newPos = player.position;

            if (player.position.x < minX || player.position.x > maxX)
            {
                newPos.x = AdjustPosition(player.position.x, maxX, minX);
            }

            if (player.position.y < minY || player.position.y > maxY)
            {
                newPos.y = AdjustPosition(player.position.y, maxY, minY);
            }

            newPos.z = transform.position.z;

            transform.SetPositionAndRotation(newPos, transform.rotation);
        }

        private float AdjustPosition(float pos, float max, float min)
        {
            if (pos > max)
            {
                return max;
            }

            return min;
        }
    }
}
