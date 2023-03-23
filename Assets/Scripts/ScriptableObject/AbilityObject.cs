using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Ability" ,menuName = "CustomData/Ability", order = 2)]
    public class AbilityObject : UnityEngine.ScriptableObject
    {
        public string abilityName;
        public string abilityDisplayName;
        public string abilityDescription;
        public bool isActive;
        public int maxLevel;
        public float percent;
        public EvolutionObject evolution;
    }
}
