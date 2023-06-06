using System;
using PlateformSurvivor.Player;
using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public class Magnetizable : MonoBehaviour
    {
        private GameObject player;

        private const float Force = 10;

        private void Start()
        {
            player = GameObject.Find("Player");
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= PlayerStat.currentStats["Magnet"])
            {
                transform.position = Vector3.MoveTowards(transform.position,player.transform.position, Time.deltaTime * Force);
            }
        }
    }
}