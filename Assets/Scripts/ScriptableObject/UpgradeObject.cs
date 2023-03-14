using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Upgrade", menuName = "CustomData/Upgrade", order = 2)]
    public class UpgradeObject : UnityEngine.ScriptableObject
    {
        public string upgradeName;
        public string upgradeDesc;
        public int maxLevel;
        public int basePrice;
        public float percentEffect;
    }
}