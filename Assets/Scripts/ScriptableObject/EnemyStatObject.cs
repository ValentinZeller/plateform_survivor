using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Enemy Stat", menuName = "CustomData/EnemyStat", order = 2)]
    public class EnemyStatObject : UnityEngine.ScriptableObject
    {
        public float speed;
        public float strength;
        public float health;
        public float jumpForce;
        public float jumpCooldown;
        public float xpDrop;

        public float this[int key]
        {
            get
            {
                float[] propArray = { speed, strength, health, jumpForce, jumpCooldown, xpDrop };
                return propArray[key];
            }
        }

        public static List<string> Keys()
        {
            return new List<string>() { "Speed", "Strength", "Health", "JumpForce", "JumpCooldown", "XpDrop"};
        }
    }
}
