using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "CustomData/Wave", order = 2)]
public class WaveObject : ScriptableObject
{
    public GameObject enemy;
    public int count;
    public float waitTime;
}
