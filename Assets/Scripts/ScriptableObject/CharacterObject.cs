using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

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

        public float regen;
        public float protection;
        public float size;
        public float projectileSpeed;
        public float duration;
        public float amount;
        public float cooldown;
        
        public float luck;
        public float magnet;
        public float xpRate;
        public float coinRate;

        public float live;
        public float skip;
        public float reroll;
        public float banish;
        
        public int price;
        public string displayName;
        public int displayOrder;
        public LocalizedString description;

        public float this[int key]
        {
            get {
                float[] propArray = {speed, jumpForce, strength, health, regen, protection, size, projectileSpeed, duration, amount, cooldown, luck, magnet, xpRate, coinRate, live, skip, reroll, banish};
                return propArray[key]; 
            }
        }

        public static List<string> Keys()
        {
            return new List<string>() { "Speed", "JumpForce", "Strength", "Health", "Regen", "Protection", "Size", "ProjectileSpeed", "Duration", "Amount", "Cooldown", "Luck", "Magnet", "XpRate", "CoinRate", "Live", "Skip", "Reroll", "Banish" };
        }
    }
}