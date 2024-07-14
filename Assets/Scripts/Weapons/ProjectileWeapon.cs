using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
	protected float currentAttackInterval; // thoi gian o giua moi lan phong dao
	protected int currentAttackCount; // number of the knife

	protected override void Update()
	{
	 base.Update(); // call update() of Weapon class => Xac nhan xong cooldown chua de gan number bao
        if (currentAttackInterval > 0) // => Xac nhan xem con luot phong nao nua khong
        {
			currentAttackInterval -= Time.deltaTime;
            if (currentAttackInterval <= 0)
            {
				Attack(currentAttackCount);
			}
        }
    }
    public override bool CanAttack()
    {
        if (currentAttackCount > 0) return true;
        return base.CanAttack();
        
    }
	protected override bool Attack(int attackCount =1)
	{
		if (!currentStats.projectilePrefab)
		{
			Debug.LogWarning("Projectile prefabs has not beeen set for {0}");
		//	currentCooldown = data.baseStats.cooldown;
            ActiveCoolDown(true);
            return false;
		}
		if (!CanAttack())
		{
			return false;
		}
		float spawnAngle = GetSpawnAngle();

		Projectile prefab = Instantiate(
			currentStats.projectilePrefab,
			owner.transform.position + (Vector3)GetSpawnOffset(spawnAngle),
			Quaternion.Euler(0,0,spawnAngle)
			);

		prefab.weapon = this;
		prefab.owner = owner;

		ActiveCoolDown(true);
		attackCount--;

        if (attackCount > 0)
        {
			currentAttackCount = attackCount;
			currentAttackInterval = data.baseStats.projectTileInterval;
        }
		return true;
    }
	protected virtual float GetSpawnAngle()
	{
		Vector3 rotation = transform.position - shootingDirection.transform.position;
		return Mathf.Atan2(rotation.y, rotation.x) *Mathf.Rad2Deg;
	}

	protected virtual Vector2 GetSpawnOffset(float spawnAngle = 0)
	{
		return Quaternion.Euler(0, 0, spawnAngle) * new Vector2(
			Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
			Random.Range(currentStats.spawnVariance.yMin, currentStats.spawnVariance.yMax)
			);
	}


}
