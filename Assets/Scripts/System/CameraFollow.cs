using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minX = -20;
    [SerializeField] private float maxX = 16.5f;
    [SerializeField] private float minY = 4.5f;
    [SerializeField] private float maxY = 23.5f;
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y, transform.position.z);

        if (player.position.x < minX || player.position.x > maxX)
        {
            newPos.x = transform.position.x;
        }

        if (player.position.y < minY || player.position.y > maxY)
        {
            newPos.y = transform.position.y;
        }
        transform.SetPositionAndRotation(newPos, transform.rotation);
    }
}
