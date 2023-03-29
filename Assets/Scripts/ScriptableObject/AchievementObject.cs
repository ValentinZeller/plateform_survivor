using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Achievement" ,menuName = "CustomData/Achievement", order = 2)]
    public class AchievementObject : UnityEngine.ScriptableObject
    {
        public string displayName;
        public string description;
        public int displayOrder;
    }
}