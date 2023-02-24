using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "CustomData/Upgrade", order = 2)]
public class UpgradeObject : ScriptableObject
{
    public string upgradeName;
    public string upgradeDesc;
    public int maxLevel;
    public int basePrice;
    public float percentEffect;
}