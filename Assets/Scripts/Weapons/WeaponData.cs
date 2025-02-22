using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponData : ItemData
{
   
    [HideInInspector] public string behaviour;
    public Weapon.Stats baseStats;
    public Weapon.Stats[] linearGrowth;
    public Weapon.Stats[] randomGrowth;

    public override Item.LevelData GetLevelData(int level)
    {
        if (level <=1 )
        {
            return baseStats;
        }
        if (level - 2 < linearGrowth.Length)
        {
            return linearGrowth[level - 2];
        }
        if (randomGrowth.Length>0)
        {
            return randomGrowth[UnityEngine.Random.Range(0, randomGrowth.Length)];
        }
        Debug.LogWarning("Weapon doesn't have its level stats configured for level");
        return new Weapon.Stats();
    }


    
}
