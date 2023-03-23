using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "CustomData/Evolution", order = 2)]
    public class EvolutionObject : UnityEngine.ScriptableObject
    {
        public List<AbilityObject> active;
        public List<AbilityObject> passive;
        public bool maxPassive;
    }
}