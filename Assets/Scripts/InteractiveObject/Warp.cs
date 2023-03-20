using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public class Warp : MonoBehaviour
    {
        [SerializeField] private Transform exit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.transform.position = exit.position;
        }
    }
}
