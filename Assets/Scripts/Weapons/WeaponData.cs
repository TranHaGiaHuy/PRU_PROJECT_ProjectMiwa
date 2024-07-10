using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Data", menuName ="ScriptableObject/Weapon Data")]
public class WeaponData : ItemData
{
   
    [HideInInspector] public string behaviour;
    public Weapon.Stats baseStats;
    public Weapon.Stats[] linearGrowth;
    public Weapon.Stats[] randomGrowth;

    public Weapon.Stats GetLevelData(int level)
    {
        if (level -2 < linearGrowth.Length)
        {
            return linearGrowth[level - 3];
        }
        if (randomGrowth.Length>0)
        {
            return randomGrowth[UnityEngine.Random.Range(0, randomGrowth.Length)];
        }
        Debug.LogWarning("Weapon doesn;t have its level stats configured for level");
        return new Weapon.Stats();
    }


    
}
