using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingWeapon : ProjectileWeapon
{
    
    List<EnemyStats> allSelectedEnemies = new List<EnemyStats>();
    protected override bool Attack(int attackCount = 1)
    {
        if (!currentStats.hitEffect)
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
            allSelectedEnemies = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());
            currentCooldown += data.baseStats.cooldown;
            currentAttackCount = attackCount;
        }

        EnemyStats target = PickEnemy();
        if (target)
        {
            DamageArea(target.transform.position, currentStats.area, GetDamage());

            Instantiate(currentStats.hitEffect, target.transform.position, Quaternion.identity);
        }
        if (attackCount>0)
        {
            currentAttackCount = attackCount - 1;
            currentAttackInterval = currentStats.projectTileInterval;
        }
        return true;
    }
    EnemyStats PickEnemy()
    {
        EnemyStats target = null;
        while (!target && allSelectedEnemies.Count >0)
        {
            int index = UnityEngine.Random.Range(0, allSelectedEnemies.Count);
            target = allSelectedEnemies[index];

            if (!target)
            {
                allSelectedEnemies.RemoveAt(index);
                continue;
            }

            Renderer r = target.GetComponent<Renderer>();
            if(!r || !r.isVisible)
            {
                allSelectedEnemies.Remove(target);
                target = null;
                continue;
            }
        }
        allSelectedEnemies.Remove(target);
        return target;
    }

    //Deals damage in an area
    void DamageArea(Vector2 position, float radius, float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D c in targets)
        {
            EnemyStats es = c.GetComponent<EnemyStats>();
            if (es)
            {
                es.TakeDamage(damage, transform.position);
            }
        }
    }

}
