using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New Evolution", menuName = "CustomData/Evolution", order = 2)]
    public class UnionObject : UnityEngine.ScriptableObject
    {
        public AbilityObject active1;
        public AbilityObject active2;
        public AbilityObject passive;
        public bool maxPassive;
    }
}