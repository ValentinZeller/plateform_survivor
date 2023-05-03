using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Ability" ,menuName = "CustomData/Ability", order = 2)]
    public class AbilityObject : UnityEngine.ScriptableObject
    {
        public string abilityName;
        public LocalizedString abilityDisplayName;
        public LocalizedString abilityDescription;
        public bool isActive;
        public int maxLevel;
        public float percent;
        public EvolutionObject evolution;
        public Sprite sprite;
    }
}
