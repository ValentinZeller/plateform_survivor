using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Upgrade", menuName = "CustomData/Upgrade", order = 2)]
    public class UpgradeObject : UnityEngine.ScriptableObject
    {
        public string upgradeName;
        public LocalizedString displayName;
        public LocalizedString upgradeDesc;
        public int maxLevel;
        public int basePrice;
        public float percentEffect;
    }
}