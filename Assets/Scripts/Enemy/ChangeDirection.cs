using System;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class ChangeDirection : MonoBehaviour
    {
        private const float ChangeTime = 2.5f;
        private float changeTimeCounter;

        [SerializeField] private EnemyBehavior behavior;

        private void Update()
        {
            changeTimeCounter += Time.deltaTime;
            if (changeTimeCounter >= ChangeTime)
            {
                behavior.InverseDirection();
                changeTimeCounter = 0;
            }
        }
    }
}