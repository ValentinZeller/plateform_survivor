using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "CustomData/Stat", order = 2)]
public class StatObject : ScriptableObject
{
    public float speed;
    public float jumpForce;
    public float strength;
    public float health;

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
