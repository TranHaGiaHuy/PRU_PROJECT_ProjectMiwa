using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public int currentLevel = 1, maxLevel = 1;
	protected ItemData.Evolution[] evolutionData;
	protected PlayerInventory inventory;
	protected PlayerStats owner;

    public PlayerStats Owner { get { return owner; } }
	[System.Serializable]
	public class LevelData 
	{
		public string name, description;
	}

	public virtual void Initialise(ItemData data)
	{
		maxLevel = data.maxLevel;

		evolutionData = data.evolutionData;

		inventory = FindObjectOfType<PlayerInventory>();
		owner = FindObjectOfType<PlayerStats>();
	}

	// Tra ve list nhung evolution co the co
	public virtual ItemData.Evolution[] CanEvolve()
	{
		//tao 1 list chua cac evolution co the co cua trang bi
		List<ItemData.Evolution> possibleEvolutions = new List<ItemData.Evolution>();

		// duyet qua moi trang bi evolution trong List nhung evolution cua trang bi
        foreach (ItemData.Evolution e in evolutionData)
        {
			if (CanEvolve(e)) // Check dieu khien xem tien hoa dc chua
			{
				// them cai evolution nay vao danh sach nhung evol trang bi co the tien hoa
				possibleEvolutions.Add(e);
			}
        }
		
		return possibleEvolutions.ToArray();
    }
	public virtual bool CanEvolve(ItemData.Evolution evolution, int levelUpAmount = 1)
	{
		// neu trang bi chua du level de tien hoa => thoat ra ngoai
		if (evolution.evolutionLevel > currentLevel + levelUpAmount)
		{
			Debug.LogWarning("Evolution failed. Current level " + currentLevel + ", evolution level " + evolution.evolutionLevel);
			return false;
		}
		// Loc qua nhung nguyen lieu can hien te trong danh sach hien te
		foreach (ItemData.Evolution.Config c in evolution.catalysts)
		{
			Item item = inventory.Get(c.itemType); // lay cai data ra , hoac se la weapon, hoac la passive
			if (!item || item.currentLevel < c.level) //Neu cap hien te chua du dieu kien thi cung => thoat ra ngoai
            {
				Debug.LogWarning("Evolution failed! Missing " + c.itemType.name);
				return false;
            }
        }
		//Duyet qua du dieu khien het roi => Ok , tien hoa
		return true;
    }

	public virtual bool AttempEvolution(ItemData.Evolution evolutionData, int levelUpAmount = 1)
	{
        if (!CanEvolve(evolutionData, levelUpAmount))
        {
			return false;
        }
		bool consumePassive = (evolutionData.consumes & ItemData.Evolution.Consumption.passive) > 0;
		bool consumeWeapon = (evolutionData.consumes & ItemData.Evolution.Consumption.weapon) > 0;
        foreach (ItemData.Evolution.Config c  in evolutionData.catalysts)
        {
            if (c.itemType as WeaponData && consumeWeapon)
            {
				inventory.Remove(c.itemType, true);
            }
			else if (c.itemType as PassiveData && consumePassive)
			{
				inventory.Remove(c.itemType, true);
			}
		}
        if (this is Weapon && consumeWeapon)
        {
            inventory.Remove((this as Weapon).data,true);
        }
		if (this is Passive && consumePassive)
		{
			inventory.Remove((this as Passive).data, true);
		}

		inventory.Add(evolutionData.outcome.itemType);

		return true;
	}
	public virtual bool CanLevelUp() {
		return currentLevel <= maxLevel;
	}

	//Whenever a passive item levels up, attemp to make it evolve
	public virtual bool DoLevelUp()
	{
        if (evolutionData == null)
        {
        return true;

		}
        foreach (ItemData.Evolution e in evolutionData)
        {
            if (e.condition == ItemData.Evolution.Condition.auto)
            {
            AttempEvolution(e);

			}
		}
		return true;

	}

	//What effects you receive an equiping an item
	public virtual void OnEquip() { }

	//What effects you removed an unequiping an item
	public virtual void OnUnequip() { }
}
