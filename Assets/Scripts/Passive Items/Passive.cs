using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : Item
{
    public PassiveData data;
    [SerializeField]
    CharacterData.Stats currentBoosts;

    [System.Serializable]
    public class Modifier : LevelData
    {
        public CharacterData.Stats boosts;
    }
    public virtual void Initialise(PassiveData data)
    {
        base.Initialise(data);
        this.data = data;
        currentBoosts = data.baseStats.boosts;
    }

    public virtual CharacterData.Stats GetBoosts()
    {
        return currentBoosts;
    }

	public override bool DoLevelUp()
	{
		base.DoLevelUp();
        if (!CanLevelUp())
        {
            Debug.LogWarning("Can not level up "+name+" to Level " + currentLevel+ ", max level of "+data.maxLevel+" already reached.");
            return false;
        }
        currentBoosts += ((Modifier)data.GetLevelData(++currentLevel)).boosts;
        return true;
    }
}
