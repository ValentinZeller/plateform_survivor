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
}
