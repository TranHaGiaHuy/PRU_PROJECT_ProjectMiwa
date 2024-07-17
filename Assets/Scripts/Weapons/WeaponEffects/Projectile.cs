using Assets.Scripts.Weapons;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WeaponEffect
{
    public enum DamageSource { projectile, owner}
    public DamageSource damageSource = DamageSource.projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3 (0, 0, 0);
    Vector3 mousePos = new Vector3(0, 0, 0);
	Rigidbody2D rb;
    protected int piercing;
    public gun shootingDirection;

	protected virtual void Start()
    {
		shootingDirection = FindObjectOfType<gun>();
		mousePos= shootingDirection.transform.position - transform.position;
		rb = GetComponent<Rigidbody2D>();

        Weapon.Stats stats = weapon.GetStats();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.velocity = transform.right * stats.speed * weapon.Owner.Stats.projectileSpeed;
        }

        float area =weapon.GetArea();
        if (area<=0)
        {
            area = 1;
        }

        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y)
        );
        piercing = stats.pierce;

        if (stats.lifeSpan >0 ) 
        {
            Destroy(gameObject,stats.lifeSpan);
        }
        if (hasAutoAim)
        {
			AcquiredAutoAimFacing();
		}
    }
    public virtual void AcquiredAutoAimFacing()
    {
        float aimAngle;

        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();
        if (targets.Length >0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x)*Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);
        }
        transform.rotation = Quaternion.Euler(0,0,aimAngle);
    }
    protected virtual void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Stats stats = weapon.GetStats();
            transform.position += mousePos * stats.speed * weapon.Owner.Stats.projectileSpeed* Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    //Weapon hit targer
	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		EnemyStats es = other.GetComponent<EnemyStats>();
        BreakableProps bp = other.GetComponent<BreakableProps>();

        if (es) // if enemy
        {
            Vector3 source  =  damageSource == DamageSource.owner && owner ? owner.transform.position : transform.position;
            es.TakeDamage(GetDamage(),source);
            Weapon.Stats stats = weapon.GetStats();
            piercing--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }
        else if (bp) // if prop: stone, tree,...
        {
			bp.TakeDamage(GetDamage());
			piercing--;
			Weapon.Stats stats = weapon.GetStats();
			if (stats.hitEffect)
			{
				Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
			}
		}
        if (piercing <=0) Destroy(gameObject);                                     
    }

}
 
