using System;
using PlateformSurvivor.Player;
using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public class Magnetizable : MonoBehaviour
    {
        private GameObject player;
        private PlayerStat playerStat;

        private const float Force = 10;

        private void Start()
        {
            player = GameObject.Find("Player");
            playerStat = player.GetComponent<PlayerStat>();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= playerStat.currentStats["Magnet"])
            {
                transform.position = Vector3.MoveTowards(transform.position,player.transform.position, Time.deltaTime * Force);
            }
        }
    }
}