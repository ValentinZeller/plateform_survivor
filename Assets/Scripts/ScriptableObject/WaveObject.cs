using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "CustomData/Wave", order = 2)]
public class WaveObject : ScriptableObject
{
    public List<GameObject> enemy;
    public int minCount;
    public float spawnRate;
    public bool isBossWave;
}
