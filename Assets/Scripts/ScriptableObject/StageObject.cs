using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Stage" ,menuName = "CustomData/Stage", order = 2)]
    public class StageObject : UnityEngine.ScriptableObject
    {
        public LocalizedString displayName;
        public LocalizedString description;
        public int displayOrder;
        public List<WaveObject> waves;
        public int waveDurationSecond;
        public int timeBeforeEvolveSecond;
    }
}