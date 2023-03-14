using UnityEngine;

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
                newPos.x = transform.position.x;
            }

            if (player.position.y < minY || player.position.y > maxY)
            {
                newPos.y = transform.position.y;
            }

            newPos.z = transform.position.z;

            transform.SetPositionAndRotation(newPos, transform.rotation);
        }
    }
}
