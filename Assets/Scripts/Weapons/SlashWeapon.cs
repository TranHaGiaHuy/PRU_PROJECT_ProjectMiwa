using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
    public class SlashWeapon : ProjectileWeapon
    {
        int currentSpawnCount;
        float currentSpawnYOffset; // for multiple slash, it move upward on top of other

        protected override bool Attack(int attackCount = 1)
        {
            if (!currentStats.projectilePrefab)
            {
                Debug.LogWarning("Projectile prefabs has not beeen set for " + name);
                currentCooldown = data.baseStats.cooldown;
                return false;
            }
            if (!CanAttack())
            {
                return false;
            }
            if (currentCooldown <= 0)
            {
                currentSpawnCount = 0;
                currentSpawnYOffset = 0f;
            }
            Vector3 dir = shootingDirection.transform.position - transform.position ;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float spawnDir = Mathf.Sign(dir.x) * (currentSpawnCount % 2 != 0 ? -1 : 1);
            Vector2 spawnOffset = new Vector2(
                 spawnDir * UnityEngine.Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax)
                 , currentSpawnYOffset);

            Projectile prefab = Instantiate(
            currentStats.projectilePrefab,
            owner.transform.position + (Vector3)spawnOffset,
            Quaternion.Euler(0, 0, angle) );

            prefab.owner = owner;


            if (spawnDir < 0)
            {
                prefab.transform.localScale = new Vector3(-Mathf.Abs(prefab.transform.localScale.x), prefab.transform.localScale.y, prefab.transform.localScale.z);
                Debug.Log(spawnDir + " | " + prefab.transform.localScale);
            }


            prefab.weapon = this;
            currentCooldown = data.baseStats.cooldown;
            attackCount--;

            currentSpawnCount++;
            if (currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
            {
                currentSpawnYOffset += 1;
            }
            if (attackCount > 0)
            {
                currentAttackCount = attackCount;
                currentAttackInterval = data.baseStats.projectTileInterval;
            }
            return true;
        }

    }
