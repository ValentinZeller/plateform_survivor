using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Stage" ,menuName = "CustomData/Stage", order = 2)]
    public class StageObject : UnityEngine.ScriptableObject
    {
        public string displayName;
        public string description;
        public int displayOrder;
        public List<WaveObject> waves;
        public int waveDurationSecond;
    }
}