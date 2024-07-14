using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignedItem)
        {
            item = assignedItem;
            if (item is Weapon)
            {
                Weapon w = item as Weapon;
                image.enabled = true;
                image.sprite = w.data.icon;
            }
            else
            {
                Passive p = item as Passive;
                image.enabled = true;
                image.sprite = p.data.icon;
            }
            Debug.Log("Assigned " + item.name + " to owner");
        }
        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite = null;
        }
        public bool isEmpty()
        {
            return item == null;
        }

    }
    public List<Slot> weaponSlots = new List<Slot>();
    public List<Slot> passiveSlots = new List<Slot>();

  /*  [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }*/

    [Header("UI Elements")]
    //List for Chooose Item Upgrade when Level Up
    public List<WeaponData> availableWeapons = new List<WeaponData>();
    public List<PassiveData> availablePassives = new List<PassiveData>();
    // public List<UpgradeUI> availableUIOptions = new List<UpgradeUI>();
    public UIUpgradeWindow upgradeWindow;
    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public Item Get(ItemData item)
    {
        if (item is WeaponData)
        {
            return Get(item as WeaponData);
        }
        else if (item is PassiveData)
        {
            return Get(item as PassiveData);
        }
        return null;
    }

    public Weapon Get(WeaponData weapon)
    {
        foreach (Slot s in weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w && w.data == weapon)
            {
                return w;
            }
        }
        return null;
    }

    public Passive Get(PassiveData passive)
    {
        foreach (Slot s in passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p && p.data == passive)
            {
                return p;
            }
        }
        return null;
    }

    public bool Remove(ItemData data, bool removeUpgradeAvailabilirty = false)
    {
        if (data is WeaponData)
        {
            return Remove(data as WeaponData, removeUpgradeAvailabilirty);
        }
        else if (data is PassiveData)
        {
            return Remove(data as PassiveData, removeUpgradeAvailabilirty);
        }
        return false;
    }


    public bool Remove(WeaponData data, bool removeUpgradeAvailabilirty = false)
    {
        if (removeUpgradeAvailabilirty)
        {
            availableWeapons.Remove(data);
        }
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            Weapon w = weaponSlots[i].item as Weapon;
            if (w.data == data)
            {
                weaponSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool Remove(PassiveData data, bool removeUpgradeAvailabilirty = false)
    {
        if (removeUpgradeAvailabilirty)
        {
            availablePassives.Remove(data);
        }
        for (int i = 0; i < passiveSlots.Count; i++)
        {
            Passive p = passiveSlots[i].item as Passive;
            if (p.data == data)
            {
                passiveSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return true;
            }
        }
        return false;
    }

    public int Add(ItemData data)
    {
        if (data is WeaponData)
        {
            return Add(data as WeaponData);
        }
        else if (data is PassiveData)
        {
            return Add(data as PassiveData);
        }
        return -1;
    }

    public int Add(WeaponData data)
    {
        int emptySlotIndex = -1;

        for (int i = 0; i < weaponSlots.Capacity; i++)
        {
            if (weaponSlots[i].isEmpty())
            {
                emptySlotIndex = i;
                break;
            }
        }
        // khong co thi thoat ra
        if (emptySlotIndex < 0)
        {
            return emptySlotIndex; // -1
        }
        // co thi tao weapon vao cai slot bang cach lay type of weapon ( cach list xo xuong cua behavior trong weapon data)
        Type weaponType = Type.GetType(data.behaviour);

        if (weaponType != null)
        {
            GameObject go = new GameObject(data.baseStats.name + " Controller");
            Weapon spawnedWeapon = (Weapon)go.AddComponent(weaponType);

            spawnedWeapon.transform.SetParent(transform); // tro thanh 1 doi tuong con cua Player
            spawnedWeapon.transform.localPosition = Vector2.zero;
            spawnedWeapon.Initialise(data);
            spawnedWeapon.OnEquip();

            weaponSlots[emptySlotIndex].Assign(spawnedWeapon);

            if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
            return emptySlotIndex;
        }
        else
        {
            Debug.LogWarning("Invalid weapion type specified for " + data.name);
        }
        return -1;
    }

    public int Add(PassiveData data)
    {
        int emptySlotIndex = -1;

        for (int i = 0; i < passiveSlots.Capacity; i++)
        {
            if (passiveSlots[i].isEmpty())
            {
                emptySlotIndex = i;
                break;
            }
        }
        // khong co thi thoat ra
        if (emptySlotIndex < 0)
        {
            return emptySlotIndex; // -1
        }
        // co thi tao weapon vao cai slot bang cach lay type of weapon ( cach list xo xuong cua behavior trong weapon data)

        GameObject go = new GameObject(data.baseStats.name + " Passive");
        Passive spawnedPassive = go.AddComponent<Passive>();
        spawnedPassive.Initialise(data);
        spawnedPassive.transform.SetParent(transform); // tro thanh 1 doi tuong con cua Player
        spawnedPassive.transform.localPosition = Vector2.zero;
        spawnedPassive.OnEquip();

        passiveSlots[emptySlotIndex].Assign(spawnedPassive);

        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();
        return emptySlotIndex;
    }

    /*public  void LevelUpWeapon(int slotIndex, int upgradeIndex)
	{
        if (weaponSlots.Count>slotIndex)
        {
            Weapon weapon = weaponSlots[slotIndex].item as Weapon;

			//Dont level up the weapion if it is already at max level
            if (!weapon.DoLevelUp())
            {
				Debug.LogWarning("Failed to level up " + weapon.name);
				return;
            }
        }
        if (GameManager.instance && GameManager.instance.isChoosingUpgrade)
        {
			GameManager.instance.EndLevelUp();
        }
    }*/

    public bool LevelUp(ItemData data)

    {

        Item item = Get(data);
        if (item) return LevelUp(item);
        return false;
    }

    // Levels up a selected weapon in the player inventory.

    public bool LevelUp(Item item)
    {
        // Tries to level up the item.
        if (!item.DoLevelUp())
        {
            Debug.LogWarning(string.Format("Failed to level up {0}.", item.name));
            return false;
        }
        // Close the level up screen afterwards.
        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }

        // If it is a passive, recalculate player stats.
        if (item is Passive) player.RecalculateStats();
        return true;
    }

    /*  public void LevelUpPassive(int slotIndex, int upgradeIndex)
      {
          if (passiveSlots.Count > slotIndex)
          {
              Passive passive = passiveSlots[slotIndex].item as Passive;

              //Dont level up the weapion if it is already at max level
              if (!passive.DoLevelUp())
              {
                  Debug.LogWarning("Failed to level up " + passive.name);
                  return;
              }
          }
          if (GameManager.instance && GameManager.instance.isChoosingUpgrade)
          {
              GameManager.instance.EndLevelUp();
          }
          player.RecalculateStats();
      }*/
    int GetSlotsLeft(List<Slot> slots)
    {
        int count = 0;
        foreach (Slot s in slots)
        {
            if (s.isEmpty()) count++;
        }
        return count;
    }
    //Load nhung upgrade option len 
    /*  void ApplyUpgradeOptions()
          {
              //Temporaly list to prevent items duplicate by remove from tempo list
              List<WeaponData> availableWeaponUpgradeOptions = new List<WeaponData>(availableWeapons);
              List<PassiveData> availablePassiveItemUpgradeOptions = new List<PassiveData>(availablePassives);
              int dem = 1;
              //Luot trong moi o vi tri cua ui options (max 4 vi tri)
              foreach (var upgradeOption in availableUIOptions)
              {
                  Debug.LogWarning("Lua chon : " + dem);
                  dem++;
                  Debug.LogWarning("			So luong vu khi con" + availableWeaponUpgradeOptions.Count);
                  Debug.LogWarning("			So luong bi dong con" + availablePassiveItemUpgradeOptions.Count);

                  //neu khong co bat ki nang cap nao thi thoat ra
                  if (availableWeaponUpgradeOptions.Count == 0 && availablePassiveItemUpgradeOptions.Count == 0)
                  {
                      return;
                  }
                  int upgradeType;
                  if (availableWeaponUpgradeOptions.Count == 0)
                  {
                      upgradeType = 2;
                  }
                  else if (availablePassiveItemUpgradeOptions.Count == 0)
                  {
                      upgradeType = 1;

                  }
                  else
                  {
                      upgradeType = UnityEngine.Random.Range(1, 3);//Choose between weapon and passive item 1-2 only, 3 is like a limit end

                  }

                  //==========================================================================================================================================

                  if (upgradeType == 1)
                  {
                      Debug.LogWarning("===>> Chon vu khi");

                      WeaponData chosenWeaponUpgrade = availableWeaponUpgradeOptions[UnityEngine.Random.Range(0, availableWeaponUpgradeOptions.Count)];
                      availableWeaponUpgradeOptions.Remove(chosenWeaponUpgrade);

                      if (chosenWeaponUpgrade != null)
                      {
                          EnableUpgradeUI(upgradeOption);

                          bool isLevelUp = false;
                          for (int i = 0; i < weaponSlots.Count; i++)
                          {
                              Weapon w = weaponSlots[i].item as Weapon;
                              if (w != null && w.data == chosenWeaponUpgrade)
                              {
                                  Debug.LogWarning("===> Nang cap vu khi");

                                  if (chosenWeaponUpgrade.maxLevel <= w.currentLevel) //neu weapon da max level thi khong can tiep tuc
                                  {
                                      Debug.LogWarning("===> Da max");

                                      DisablueUpgradeUi(upgradeOption);
                                      isLevelUp = true;
                                      break;
                                  }
                                  Debug.LogWarning("===>Nang cap passive " + chosenWeaponUpgrade.baseStats.name);

                                  //Upgrade weapon len
                                  upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                                  upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                                  Weapon.Stats nextLevel = chosenWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                                  upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                                  upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                                  isLevelUp = true;
                                  break;
                              }
                          }
                          //If Weapin is new
                          if (!isLevelUp)
                          {
                              Debug.LogWarning("===>> Them moi vu khi");

                              upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                              upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenWeaponUpgrade));
                              upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.baseStats.name;
                              upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.baseStats.description;

                          }
                      }
                  }
                  else if (upgradeType == 2)
                  {
                      Debug.LogWarning("===>> Chon bi dong");

                      PassiveData chosenPassiveUpgrade = availablePassiveItemUpgradeOptions[UnityEngine.Random.Range(0, availablePassiveItemUpgradeOptions.Count)];
                      availablePassiveItemUpgradeOptions.Remove(chosenPassiveUpgrade);

                      if (chosenPassiveUpgrade != null)
                      {
                          EnableUpgradeUI(upgradeOption);

                          bool isLevelUp = false;
                          for (int i = 0; i < passiveSlots.Count; i++)
                          {

                              Passive p = passiveSlots[i].item as Passive;
                              if (p != null && p.data == chosenPassiveUpgrade)
                              {
                                  Debug.LogWarning("===>Check Nang cap passive");

                                  if (chosenPassiveUpgrade.maxLevel <= p.currentLevel) //neu passive da max level thi khong can tiep tuc
                                  {
                                      Debug.LogWarning("===>Da max");

                                      DisablueUpgradeUi(upgradeOption);
                                      isLevelUp = true;
                                      break;
                                  }
                                  Debug.LogWarning("===>Nang cap passive " + chosenPassiveUpgrade.baseStats.name);

                                  //Upgrade weapon len
                                  upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                                  Passive.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.currentLevel + 1);
                                  upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                                  upgradeOption.upgradeNameDisplay.text = chosenPassiveUpgrade.name;
                                  upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                                  isLevelUp = true;
                                  break;
                              }
                          }
                          //If Passive is new
                          if (!isLevelUp)
                          {
                              Debug.LogWarning("===>> Them moi bi dong");

                              upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                              upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenPassiveUpgrade));
                              upgradeOption.upgradeNameDisplay.text = chosenPassiveUpgrade.baseStats.name;
                              upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveUpgrade.baseStats.description;
                          }
                      }
                  }
              }
          }*/
    public void ApplyUpgradeOptions()
    {
        List<ItemData> availableUpgrades = new List<ItemData>();
        List<ItemData> allUpgrades = new List<ItemData>(availableWeapons);
        allUpgrades.AddRange(availablePassives);

        // We need to know how many weapon / passive slots are left. 
        int weaponSlotsLeft = GetSlotsLeft(weaponSlots);
        int passiveSlotsLeft = GetSlotsLeft(passiveSlots);

        // Filters through the available weapons and passives and add those

        // that can possibly be an option.

        foreach (ItemData data in allUpgrades)
        {
            // If a weapon of this type exists, allow for the upgrade if the
            // level of the weapon is not already maxed out.
            Item obj = Get(data);
            if (obj)
            {
                if (obj.currentLevel < data.maxLevel) availableUpgrades.Add(data);
            }
            else
            {
                if (data is WeaponData && weaponSlotsLeft > 0) availableUpgrades.Add(data);
                else if (data is PassiveData && passiveSlotsLeft > 0) availableUpgrades.Add(data);
            }
        }
        // Show the UI upgrade window if we still have available upgrades left.

        int availUpgradeCount = availableUpgrades.Count; 
        if (availUpgradeCount > 0)
        {

            bool getExtraItem = 1f - 1f/player.Stats.luck > UnityEngine.Random.value;
            if (getExtraItem/* || availUpgradeCount < 4*/) upgradeWindow.SetUpgrades(this, availableUpgrades, 4);
            else upgradeWindow.SetUpgrades(this, availableUpgrades, 3, "Increase your Luck stat for a chance to get 4 items!");
        }
        else if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
        // Iterate through each slot in the upgrade UI and populate the options.
        /* foreach (UpgradeUI upgradeOption in availableUIOptions)
         {
             // If there are no more available upgrades, then we abort.
             if (availableUpgrades.Count <= 0) return;

             // Pick an upgrade, then remove it so that we don't get it twice.
             ItemData chosenUpgrade = availableUpgrades[UnityEngine.Random.Range(0, availableUpgrades.Count)];
             availableUpgrades.Remove(chosenUpgrade);

             // Ensure that the selected weapon data is valid.
             if (chosenUpgrade != null)
             {
                 // Turns on the UI slot.
                 EnableUpgradeUI(upgradeOption);

                 // If our inventory already has the upgrade, we will make it a level up.
                 Item item = Get(chosenUpgrade);
                 if (item)
                 {
                     upgradeOption.upgradeButton.onClick.AddListener(() => LevelUp(item)); //Apply button functionality
                     if (item is Weapon)
                     {
                         Weapon.Stats nextLevel = ((WeaponData)chosenUpgrade).GetLevelData(item.currentLevel + 1);
                         upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                         upgradeOption.upgradeNameDisplay.text = chosenUpgrade.name + " - " + nextLevel.name;
                         upgradeOption.upgradeIcon.sprite = chosenUpgrade.icon;
                     }
                     else
                     {
                         Passive.Modifier nextLevel = ((PassiveData)chosenUpgrade).GetLevelData(item.currentLevel + 1);
                         upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                         upgradeOption.upgradeNameDisplay.text = chosenUpgrade.name + " - " + nextLevel.name;
                         upgradeOption.upgradeIcon.sprite = chosenUpgrade.icon;
                     }
                 }
                 else
                 {
                     if (chosenUpgrade is WeaponData)
                     {
                         WeaponData data = chosenUpgrade as WeaponData;
                         upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenUpgrade)); //Apply button func scription
                         upgradeOption.upgradeDescriptionDisplay.text = data.baseStats.description; //Apply initial de
                         upgradeOption.upgradeNameDisplay.text = data.baseStats.name; //Apply initial name
                         upgradeOption.upgradeIcon.sprite = data.icon;
                     }
                     else
                     {
                         PassiveData data = chosenUpgrade as PassiveData;
                         upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenUpgrade)); //Apply button functionality
                         upgradeOption.upgradeDescriptionDisplay.text = data.baseStats.description; //Apply initial description
                         upgradeOption.upgradeNameDisplay.text = data.baseStats.name; //Apply initial name
                         upgradeOption.upgradeIcon.sprite = data.icon;
                     }
                 }

             }

         }*/

    }
/*
    void RemoveUpdateOptions()
    {
        foreach (var upgradeUIOption in availableUIOptions)
        {
            upgradeUIOption.upgradeButton.onClick.RemoveAllListeners();
            DisablueUpgradeUi(upgradeUIOption);
        }
    }*/
    public void RemoveAndApplyUpgrades()
    {
       // RemoveUpdateOptions();
        ApplyUpgradeOptions();
    }
  /*  void DisablueUpgradeUi(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }
    void EnableUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);

    }
*/
}
