using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName ="Assets/UpgradeObject")]
public class UpgradeObject : ScriptableObject
{
    public UpgradeManager.UpgradeType upgradeType;
}
