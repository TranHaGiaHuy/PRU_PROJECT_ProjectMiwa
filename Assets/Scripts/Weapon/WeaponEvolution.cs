using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolution", menuName = "ScriptableObjects/WeaponEvolution")]
public class WeaponEvolution : ScriptableObject
{
    public WeaponScriptableObject baseWeaponData;
    public PassiveItemScriptableObject basePassiveItemData;

    public WeaponScriptableObject evolveWeaponData;

    public GameObject evolveWeaponController;

}
