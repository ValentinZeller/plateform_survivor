using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Achievement" ,menuName = "CustomData/Achievement", order = 2)]
    public class AchievementObject : UnityEngine.ScriptableObject
    {
        public LocalizedString displayName;
        public LocalizedString description;
        public int displayOrder;
    }
}