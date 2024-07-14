using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive Data", menuName = "Data/PassiveData")]

public class PassiveData : ItemData
{
	public Passive.Modifier baseStats;
	public Passive.Modifier[] growth;

	public override Item.LevelData GetLevelData(int level)
	{
        if (level<=1) 
        {
            return baseStats;
        }
        //lay stats tu level tiep theo 
        if (level -2 < growth.Length)
        {
            return growth[level - 2];
        }
        Debug.LogWarning("Passive dont have next level to level up");
        return new Passive.Modifier();

    }
}
