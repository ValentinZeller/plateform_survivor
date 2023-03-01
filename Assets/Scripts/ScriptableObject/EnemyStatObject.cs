using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "CustomData/EnemyStat", order = 2)]
public class EnemyStatObject : ScriptableObject
{
    public float speed;
    public float strength;
    public float health;
    public float jumpForce;
    public float jumpCooldown;

    public float this[int key]
    {
        get
        {
            float[] propArray = new float[5] { speed, strength, health, jumpForce, jumpCooldown };
            return propArray[key];
        }
    }

    public static List<string> Keys()
    {
        return new List<string>() { "Speed", "Strength", "Health", "JumpForce", "JumpCooldown" };
    }
}
