using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Class co abstract se KHONG THE gan vao GameObject
public abstract class Weapon : Item
{
    [System.Serializable]
    public struct Stats
    {
        public string name;
        public string description;

        [Header("Visuals")]
        public ParticleSystem hitEffect;
		public Projectile projectilePrefab;
		public Rect spawnVariance;
       // public Aura auraPrefab;;


		[Header("Values")]
        public float lifeSpan; // if 0 last forever
        public float damage;
        public float damageVariance;
        public float area;
        public float speed;
        public float cooldown;
        public float projectTileInterval;
        public float knockback;
        public int number;
        public int pierce;
        public int maxInstances;

        public static Stats operator +(Stats s1, Stats s2)
        {
            Stats result = new Stats();
            result.name = s2.name ?? s1.name;
            result.description = s2.description ?? s1.description;
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect;
            result.spawnVariance = s2.spawnVariance;
            result.lifeSpan = s1.lifeSpan + s2.lifeSpan;
            result.damage = s1.damage + s2.damage;
            result.damageVariance = s1.damageVariance + s2.damageVariance;
            result.area = s1.area + s2.area;
            result.speed = s1.speed + s2.speed;
            result.cooldown = s1.cooldown + s2.cooldown;
            result.number = s1.number + s2.number;
            result.pierce = s1.pierce + s2.pierce;
            result.projectTileInterval = s1.projectTileInterval + s2.projectTileInterval;
            result.knockback = s1.knockback + s2.knockback;
            return result;
        }
        public float GetDamage()
        {
            return damage + Random.Range(0, damageVariance);
        }
    }
    //=================================================================================================================

    protected Stats currentStats;
    public WeaponData data;
    protected float currentCooldown;
    protected PlayerMovement movement;
	protected gun shootingDirection;

	public virtual void Initialise(WeaponData data)
    {
        base.Initialise(data);

		shootingDirection = FindObjectOfType<gun>();
		this.data = data;
        currentStats = data.baseStats;
        movement = GetComponent<PlayerMovement>();

        currentCooldown = currentStats.cooldown;
    }

    protected virtual void Awake()
    {
        if (data)
        {
            currentStats = data.baseStats;
        }
    }
    protected virtual void Start()
    {
        if (data)
        {
            Initialise(data);
        }
    }

    protected virtual void Update()
    {
        currentCooldown -=Time.deltaTime;
        if (currentCooldown <=0f)
        {
            Attack(currentStats.number);
        }
    }
    public virtual Stats GetStats()
    {
        return currentStats;
    }
    
    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp())
        {
            Debug.LogWarning("Cannot level up " + name + " to Level "+currentLevel+", max level of " + data.maxLevel + " already reached");
            return false;
        }

        currentStats += data.GetLevelData(++currentLevel);
        return true;
    }
    public virtual bool CanAttack() 
    {
        return currentCooldown <= 0;
    }
    protected virtual bool Attack(int attackCount = 1)
    {
        if (CanAttack())
        {
            currentCooldown += currentStats.cooldown;
            return true;
        }
        return false;
    }
    public virtual float GetDamage()
    {
        return currentStats.GetDamage() * player.CurrentMight;
    }                       
    
}
