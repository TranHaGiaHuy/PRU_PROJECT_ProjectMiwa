using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Diagnostics.Contracts;
using Unity.Burst.CompilerServices;

public class PlayerStats : MonoBehaviour
{

	CharacterData characterData;
	public CharacterData.Stats baseStats;
	[SerializeField]
	CharacterData.Stats actualStats;

	public CharacterData.Stats Stats
	{
		get { return actualStats; }
		set { actualStats = value; }
	}
	float health;
	public float CurrentHealth
	{
		get { return health; }
		set
		{
			if (health != value)
			{
				health = value;

                UpdateHealthBar();

                /*if (GameManager.instance != null)
				{
					GameManager.instance.currentHealthDisplay.text = (int)CurrentHealth + "/" + actualStats.maxHealth;

				}*/
            }
        }
	}
	/*#region Current Stats Properties
	

	public float MaxHealth
	{
		get { return actualStats.maxHealth; }
		set
		{
			if (actualStats.maxHealth != value)
			{
				actualStats.maxHealth = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentHealthDisplay.text = (int)health + "/" + actualStats.maxHealth;

				}
			}
		}
	}
	public float CurrentRecovery
	{
		get { return Recovery; }
		set { Recovery = value; }
	}
	public float Recovery
	{
		get { return actualStats.recovery; }
		set
		{
			//if value change
			if (actualStats.recovery != value)
			{
				//set new value 
				actualStats.recovery = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentRecoveryDisplay.text = actualStats.recovery.ToString();
				}
			}
		}
	}

	public float CurrentMoveSpeed
	{
		get { return MoveSpeed; }
		set { MoveSpeed = value; }
	}
	public float MoveSpeed
	{
		get { return actualStats.moveSpeed; }
		set
		{
			if (actualStats.moveSpeed != value)
			{
				actualStats.moveSpeed = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentMoveSpeedDisplay.text = actualStats.moveSpeed.ToString();
				}
			}
		}
	}

	public float CurrentMight
	{
		get { return Might; }
		set { Might = value; }
	}
	public float Might
	{
		get { return actualStats.might; }
		set
		{
			if (actualStats.might != value)
			{
				actualStats.might = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentMightDisplay.text = actualStats.might.ToString();
				}
			}
		}
	}

	public float CurrentProjectileSpeed
	{
		get { return ProjectileSpeed; }
		set { ProjectileSpeed = value; }
	}
	public float ProjectileSpeed
	{
		get { return actualStats.projectileSpeed; }
		set
		{
			if (actualStats.projectileSpeed != value)
			{
				actualStats.projectileSpeed = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentProjectileSpeedDisplay.text = actualStats.projectileSpeed.ToString();
				}
			}
		}
	}

	public float CurrentCollectRange
	{
		get { return CollectRange; }
		set { CollectRange = value; }
	}
	public float CollectRange
	{
		get { return actualStats.collectRange; }
		set
		{
			if (actualStats.collectRange != value)
			{
				actualStats.collectRange = value;
				if (GameManager.instance != null)
				{
					GameManager.instance.currentCollectRangeDisplay.text = actualStats.collectRange.ToString();
				}
			}
		}
	}
	#endregion*/

	[Header("Visual")]
	public ParticleSystem damageEffect;
	public ParticleSystem blockedEffect;


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
	float recoverTimer = 0;


	[Header("UI")]
	public Image healthBar;
	public Image expBar;
	public TMP_Text levelText;


	[Header("Weapon Demo Add")]
	PlayerInventory inventory;
	public int weaponIndex;
	public int passiveItemIndex;
	//public GameObject firstPassiveItemTest,secondPassiveItemTest,secondWeaponTest;

	PlayerAnimator playerAnimator;
	PlayerCollector playerCollector;
	private void Awake()
	{
		characterData = CharacterSelector.GetData();
        if (CharacterSelector.instance)
        {
			CharacterSelector.instance.DestroySingleton();
        }

        inventory = GetComponent<PlayerInventory>();
		playerCollector = GetComponentInChildren<PlayerCollector>();
		
		//Assign value
		baseStats = actualStats = characterData.stats;
		playerCollector.SetRadius(actualStats.collectRange);
		health = baseStats.maxHealth;

        playerAnimator = GetComponent<PlayerAnimator>();
		if (characterData.controller)
		{
            playerAnimator.SetSprites(characterData.Icon, characterData.controller);
        }
    }
	private void Start()
	{
		experienceCap = levelRanges[0].experienceCapIncrease;

		inventory.Add(characterData.StartingWeapon);

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
        /*recoverTimer += Time.deltaTime;
		if (recoverTimer > 1f && CurrentRecovery > 0)
		{
			RestoreHealth(Mathf.FloorToInt(CurrentRecovery));
			recoverTimer = 0;
		}*/
        //Recover for each 1s
        if (Stats.recovery > 0)
        {
			Recover();
		}

	}

	public void RecalculateStats()
	{
		actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
			Passive p = s.item as Passive;
			if (p){
				actualStats += p.GetBoosts();
			}
        }
		playerCollector.SetRadius(actualStats.collectRange);
    }


	public void IncreaseExp(int amount)
	{
		experience += amount;
		LevelUpChecker();
		UpdateExpBar();

	}
	private void LevelUpChecker()
	{
		if (experience >= experienceCap)
		{
			level++;
			experience -= experienceCap;
			int experienceCapIncrease = 0;
			foreach (LevelRange range in levelRanges)
			{
				if (level >= range.startLevel && level <= range.endLevel)
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
		expBar.fillAmount = (float)experience / experienceCap;
	}
	void UpdateLevelText()
	{
		levelText.text = "Lv " + level.ToString();
	}

	public void TakeDamage(float dmg)
	{
		if (!isInvincible)
		{
			dmg -= Stats.armor;
            if (dmg>0)
            {
                CurrentHealth -= dmg;
                if (damageEffect)
                {
                    Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);
                }
                if (dmg > 0 && CurrentHealth > 0)
                {
                    GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform, Color.red);
                }

                if (CurrentHealth <= 0)
                {
                    Kill();
                }
                UpdateHealthBar();
            }
            else
			{
                if (blockedEffect) 
                {
                    Destroy(Instantiate(blockedEffect, transform.position, Quaternion.identity), 5f);

                }
            }
			invincibleTimer = invincibilityDuration;
			isInvincible = true;


		}

	}
	void UpdateHealthBar()
	{
		healthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
	}
	public void Kill()
	{
		if (!GameManager.instance.isGameOver)
		{
			GameManager.instance.AssignLevelReached(level);
			GameManager.instance.AssignChosenWeaponsAndItemsUI(inventory.weaponSlots, inventory.passiveSlots);
			GameManager.instance.GameOver();
		}
	}

	public void RestoreHealth(int healthToRestore)
	{
		if (CurrentHealth < actualStats.maxHealth)
		{
			CurrentHealth += healthToRestore;

			//khong cho phep hoi vuot qua max hp
			if (CurrentHealth >= actualStats.maxHealth)
			{
				CurrentHealth = actualStats.maxHealth;
			}
			else
			{
				GameManager.GenerateFloatingText(Mathf.FloorToInt(healthToRestore).ToString(), transform, Color.green);
			}
            UpdateHealthBar();
        }

		
	}
	void Recover()
	{
		if (CurrentHealth < actualStats.maxHealth)
		{
            if (Stats.recovery>0)
            {
				CurrentHealth += Stats.recovery * Time.deltaTime;
				recoverTimer += Time.deltaTime;
				if (CurrentHealth > actualStats.maxHealth)
				{
					CurrentHealth = actualStats.maxHealth;
				}
				else if (recoverTimer > 1f)
				{
					GameManager.GenerateFloatingText(Mathf.FloorToInt(Stats.recovery).ToString(), transform, Color.green);
					recoverTimer = 0f;
				}
                UpdateHealthBar();
            }
           
		}
		
	}

}
