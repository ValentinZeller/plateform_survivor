using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability" ,menuName = "CustomData/Ability", order = 2)]
public class AbilityObject : ScriptableObject
{
    public string abilityName;
    public bool isActive;
    public int maxLevel;
}
