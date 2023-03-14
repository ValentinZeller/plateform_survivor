using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "CustomData/Wave", order = 2)]
    public class WaveObject : UnityEngine.ScriptableObject
    {
        public List<GameObject> enemy;
        public int minCount;
        public float spawnRate;
        public bool isBossWave;
    }
}
