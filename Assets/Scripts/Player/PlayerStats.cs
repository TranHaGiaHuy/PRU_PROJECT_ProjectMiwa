using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class PlayerStats : MonoBehaviour
{
   private CharacterScriptableObject characterData;

    //current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentCollectRange;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set {
            if ( currentHealth != value ) {
                currentHealth = value;
                if (GameManager.instance!=null)
                {
                    GameManager.instance.currentHealth.text = (int)currentHealth + "/" + characterData.MaxHealth;

                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecovery.text = currentRecovery.ToString();
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeed.text = currentMoveSpeed.ToString();
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMight.text = currentMight.ToString();
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeed.text = currentProjectileSpeed.ToString();
                }
            }
        }
    }

    public float CurrentCollectRange
    {
        get { return currentCollectRange; }
        set
        {
            if (currentCollectRange != value)
            {
                currentCollectRange = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentCollectRange.text = currentCollectRange.ToString();
                }
            }
        }
    }
    #endregion





    //Experience leveling
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;
    //Define level range and the increase of Cap for each level, level1 100exp, level2 100exp+50(capIncrease)
    [System.Serializable]   
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;

    }
    public List<LevelRange> levelRanges;

    


    //I-Frames: thoi gian bat tu
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibleTimer;
    bool isInvincible;
    float recoverTimer=0;


    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;


    [Header("Weapon Demo Add")]
    InventoryManager inventory;
    //public GameObject firstPassiveItemTest,secondPassiveItemTest,secondWeaponTest;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();
        //Value of current status
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentCollectRange = characterData.CollectRange;

        //Spawn the starting weapon
        

    }
    private void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        inventory.SpawnWeapon(characterData.StartingWeapon);
        //Set up UI for Pause Screen
        GameManager.instance.currentHealth.text = currentHealth + "/" + characterData.MaxHealth;
        GameManager.instance.currentRecovery.text = currentRecovery.ToString();
        GameManager.instance.currentMoveSpeed.text = currentMoveSpeed.ToString();
        GameManager.instance.currentMight.text = currentMight.ToString();
        GameManager.instance.currentProjectileSpeed.text = currentProjectileSpeed.ToString();
        GameManager.instance.currentCollectRange.text = currentCollectRange.ToString();
        GameManager.instance.AssignChosenCharacrterUI(characterData);
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();


    }
    private void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        { 
        isInvincible = false;
        }
        recoverTimer += Time.deltaTime;
        if (recoverTimer > 1f)
        {
            RestoreHealth(Mathf.FloorToInt(CurrentRecovery));
            recoverTimer = 0;
        }
        //Recover for each 1s
       // Recover();
      
    }

    public void IncreaseExp(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();

    }
    private void LevelUpChecker()
    {
       if (experience >= experienceCap) {
            level++;
            experience -=experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges) 
            {
                if (level>=range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease; //take the first level meet condition
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            UpdateLevelText();
            GameManager.instance.StartLevelUp();
        }
    }
    void UpdateExpBar()
    {
        expBar.fillAmount = (float) experience / experienceCap;
    }
    void UpdateLevelText()
    {
        levelText.text = "Lv "+level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            invincibleTimer = invincibilityDuration;
            isInvincible = true;

            CurrentHealth -= dmg;
            if (dmg > 0 && currentHealth > 0)
            {
                GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform,Color.red);
            }
            if (CurrentHealth <= 0)
            {
                Kill();
            }
           
            UpdateHealthBar();
           
        }

    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth/characterData.MaxHealth;
    }
    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReached(level);
            GameManager.instance.AssignChosenWeaponsAndItemsUI(inventory.weaponIconSlots, inventory.passiveItemIconSlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(int healthToRestore)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += healthToRestore;
            if (CurrentHealth >= characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
            else
            {
                GameManager.GenerateFloatingText(Mathf.FloorToInt(healthToRestore).ToString(), transform,Color.green);
            }

        }
       
        UpdateHealthBar();


    }
    public void Recover()
    {
        if(CurrentHealth <characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
       
        UpdateHealthBar();

    }

}
