using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    public enum ActiveAbility { Dash, DoubleJump, Fireball }

    [CreateAssetMenu(fileName = "New Character", menuName = "CustomData/Character", order = 2)]
    public class CharacterObject : UnityEngine.ScriptableObject
    {
        public ActiveAbility startAbility;
        public float speed;
        public float jumpForce;
        public float strength;
        public float health;
        public int price;
        public string displayName;
        public int displayOrder;
        public string description;

        public float this[int key]
        {
            get {
                float[] propArray = new float[4] {speed, jumpForce, strength, health};
                return propArray[key]; 
            }
        }

        public static List<string> Keys()
        {
            return new List<string>() { "Speed", "JumpForce", "Strength", "Health" };
        }
    }
}