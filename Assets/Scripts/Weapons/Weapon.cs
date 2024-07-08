using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Class co abstract se KHONG THE gan vao GameObject
public abstract class Weapon : MonoBehaviour
{
    [System.Serializable]
    public struct Stats
    {
        public string name;
        public string description;

        [Header("Visuals")]
        public ParticleSystem hitEffect;
        public Rect spawnVariance;

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
                result.hitEffect = s2.hitEffect == null?  s1.hitEffect : s2.hitEffect;
                result.spawnVariance = s2.spawnVariance;
                result.lifeSpan = s1.lifeSpan + s2.lifeSpan;
                result.damage= s1.damage + s2.damage;
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
}
