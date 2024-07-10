using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public int currentLevel = 1, maxLevel = 1;
	protected PlayerStats owner;

	public virtual void Initialise(ItemData data)
	{
		maxLevel = data.maxLevel;
		owner = FindObjectOfType<PlayerStats>();
	}
	public virtual bool CanLevelUp() {
		return currentLevel <= maxLevel;
	}

	//Whenever a passive item levels up, attemp to make it evolve
	public virtual bool DoLevelUp()
	{
		return true;
	}

	//What effects you receive an equiping an item
	public virtual void OnEquip() { }

	//What effects you removed an unequiping an item
	public virtual void OnUnequip() { }
}
