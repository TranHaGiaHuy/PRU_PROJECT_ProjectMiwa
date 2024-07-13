using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory slots")]
    public List<WeaponController> weaponSlots = new List<WeaponController>();
    public int[] weaponLevels = new int[6];
    public List<Image> weaponIconSlots = new List<Image>(6);
    public List<PassiveItemController> passiveitemSlots = new List<PassiveItemController>();
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemIconSlots = new List<Image>(6);

    PlayerStats player;
    public int weaponIndex;
    public int passiveItemIndex;
    
    #region Level Up Choose Item
    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }
    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passtiveItemData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }
    #endregion

    [Header("Level up Upgrade board")]
    //List for Chooose Item Upgrade when Level Up
    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();
    [Header("Weapons Evolution")]
    public List<WeaponEvolution> weaponEvolutions = new List<WeaponEvolution>();

    void Awake()
    {
        player = GetComponent<PlayerStats>();

    }
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponIconSlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance!=null && GameManager.instance.isChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void AddPassiveItem(int slotIndex, PassiveItemController passiveItem)
    {
        passiveitemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemIconSlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void LevelUpWeapon(int slotIndex,int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogWarning("NO NEXT LEVEL FOR " + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab,transform.position,Quaternion.identity);
            upgradedWeapon.transform.parent = transform;// set weapon to be child of owner
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
           
            //For level up to Lv3 not get error 
            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }

    }
    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveitemSlots.Count > slotIndex)
        {
            PassiveItemController passiveItem = passiveitemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogWarning("NO NEXT LEVEL FOR " + passiveItem.name);
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.parent = transform;// set passive item to be child of owner
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItemController>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItemController>().passiveItemData.Level;

            //For level up to Lv3 not get error
            passiveItemUpgradeOptions[upgradeIndex].passtiveItemData = upgradedPassiveItem.GetComponent<PassiveItemController>().passiveItemData;


            if (GameManager.instance != null && GameManager.instance.isChoosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex >= weaponSlots.Count - 1)
        {
            return;
        }
        GameObject spawnedWeapon = Instantiate(weapon, player.transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(player.transform);
        AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }
    public void SpawnPassive(GameObject passiveItem)
    {
        if (passiveItemIndex >= passiveitemSlots.Count - 1)
        {
            return;

        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, player.transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(player.transform);
        AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItemController>());
        passiveItemIndex++;
    }

    void ApplyUpgradeOptions()
    {
        int dem = 1;
        //Temporaly list to prevent items duplicate by remove from tempo list
        List<WeaponUpgrade> availableWeaponUpgradeOptions = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            Debug.LogWarning("Mon thu " + dem);
            Debug.LogWarning("so luong weapion hien tai " + availableWeaponUpgradeOptions.Count);
            Debug.LogWarning("so luong passive hien tai " + availablePassiveItemUpgradeOptions.Count);


            dem++;
            if (availableWeaponUpgradeOptions.Count == 0 && availablePassiveItemUpgradeOptions.Count ==0)
            {
                return;
            }
            int upgradeType;
            if (availableWeaponUpgradeOptions.Count == 0)
            {
                upgradeType = 2;
                Debug.Log("het weapon");
            }
            else if (availablePassiveItemUpgradeOptions.Count == 0)
            {
                upgradeType = 1;
                Debug.Log("het passisve");

            }
            else
            {
            upgradeType = Random.Range(1, 3);//Choose between weapon and passive item 1-2 only, 3 is like a limit end

            }

            //==========================================================================================================================================

            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgradeOptions[Random.Range(0, availableWeaponUpgradeOptions.Count)];
                Debug.Log("Xoa weapon: " + chosenWeaponUpgrade.weaponData.Name);
                availableWeaponUpgradeOptions.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool isNewWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            isNewWeapon = false;
                            if (!isNewWeapon) //If Weapin exist and need to upgrade
                            {
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisablueUpgradeUi(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Icon;

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i,chosenWeaponUpgrade.weaponUpgradeIndex));
                                upgradeOption.upgradeNameDisplay.text=chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;

                            }
                            break;
                        }
                        else
                        {
                            isNewWeapon = true;
                        }
                    }
                    //If Weapin is new
                    if (isNewWeapon)
                    {
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;

                        upgradeOption.upgradeButton.onClick.AddListener(() => SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;

                    }
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgradeOptions[Random.Range(0, availablePassiveItemUpgradeOptions.Count)];
                Debug.Log("Xoa passive: " + chosenPassiveItemUpgrade.passtiveItemData.Name);

                availablePassiveItemUpgradeOptions.Remove(chosenPassiveItemUpgrade);
                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool isNewPassiveItem = false;

                    //Check item in inventory to spawn the lv2 lv3 of Item if it already exist
                    for (int i = 0; i < passiveitemSlots.Count; i++)
                    {
                        if (passiveitemSlots[i] != null && passiveitemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passtiveItemData)
                        {
                            isNewPassiveItem = false;
                            if (!isNewPassiveItem) //If Weapin exist and need to upgrade
                            {
                                if (!chosenPassiveItemUpgrade.passtiveItemData.NextLevelPrefab)
                                {
                                    DisablueUpgradeUi(upgradeOption);

                                    break;
                                }
                                upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passtiveItemData.NextLevelPrefab.GetComponent<PassiveItemController>().passiveItemData.Icon;
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i,chosenPassiveItemUpgrade.passiveItemUpgradeIndex));
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passtiveItemData.NextLevelPrefab.GetComponent<PassiveItemController>().passiveItemData.Name;
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passtiveItemData.NextLevelPrefab.GetComponent<PassiveItemController>().passiveItemData.Description;

                            }
                            break;
                        }
                        else
                        {
                            isNewPassiveItem = true;
                        }
                    }
                    //If Weapin is new
                    if (isNewPassiveItem)
                    {
                        upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passtiveItemData.Icon;

                        upgradeOption.upgradeButton.onClick.AddListener(() => SpawnPassive(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passtiveItemData.Name;
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passtiveItemData.Description;
                    }
                }
            }
           
        }
        dem = 1;
    }
    void RemoveUpdateOptions()
    {
        foreach (var upgradeUIOption in upgradeUIOptions)
        {
            upgradeUIOption.upgradeButton.onClick.RemoveAllListeners();
            DisablueUpgradeUi(upgradeUIOption);

        }
    }
    public void RemoveAndApplyUpgrades()
    {
        RemoveUpdateOptions();
        ApplyUpgradeOptions();
    }
    void DisablueUpgradeUi ( UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }
    void EnableUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);

    }

    //=========================================================

    public List<WeaponEvolution> GetPossibleEvolutions()
    {
        List<WeaponEvolution> possibleEvolutions = new List<WeaponEvolution> ();
        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon !=null)
            {
                foreach (PassiveItemController passiveItem in passiveitemSlots)
                {
                    if (passiveItem !=null)
                    {
                        foreach (WeaponEvolution evolution in weaponEvolutions)
                        {
                            if (weapon.weaponData.Level >= evolution.baseWeaponData.Level && passiveItem.passiveItemData.Level >= evolution.basePassiveItemData.Level)
                            {
                                possibleEvolutions.Add(evolution);
                            }
                        }
                    }
                }
            }
        }
        return possibleEvolutions;
    }

    public void EvolveWeapon(WeaponEvolution evolution)
    {
        for (int weaponSlotIndex = 0; weaponSlotIndex < weaponSlots.Count; weaponSlotIndex++)
        {
            WeaponController weapon = weaponSlots[weaponSlotIndex];
            if (!weapon)
            {
                continue;
            }
            for (int passiveItemSlotIndex = 0; passiveItemSlotIndex < weaponSlots.Count; passiveItemSlotIndex++)
            {
                PassiveItemController passiveItem = passiveitemSlots[passiveItemSlotIndex];
                if (!passiveItem)
                {
                    continue;
                }

                if (weapon && passiveItem
                    && weapon.weaponData.Level >= evolution.baseWeaponData.Level 
                    && passiveItem.passiveItemData.Level >= evolution.basePassiveItemData.Level)
                {
                    GameObject evolveWeapon = Instantiate(evolution.evolveWeaponController, transform.position, Quaternion.identity);
                    WeaponController evolveWeaponController = evolveWeapon.GetComponent<WeaponController>();
                    evolveWeapon.transform.SetParent(transform);
                    AddWeapon(weaponSlotIndex, evolveWeaponController);
                    Destroy(weapon.gameObject);

                    weaponLevels[weaponSlotIndex] = evolveWeaponController.weaponData.Level;
                    weaponIconSlots[weaponSlotIndex].sprite = evolveWeaponController.weaponData.Icon;

                    weaponUpgradeOptions.RemoveAt(evolveWeaponController.weaponData.EvolvedIndexToRemove);

                    return;
                }

            }
        }
    }
 
}
