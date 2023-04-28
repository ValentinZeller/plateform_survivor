using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "CustomData/Evolution", order = 2)]
    public class EvolutionObject : UnityEngine.ScriptableObject
    {
        public AbilityObject active;
        public AbilityObject passive;
        public bool maxPassive;
        public string evolutionName;
        public string displayName;
        public Sprite sprite;
    }
}