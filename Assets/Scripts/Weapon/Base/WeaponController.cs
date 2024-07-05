using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stas")]
    public WeaponScriptableObject weaponData;
    
    float currentCooldown;
    

    protected PlayerMovement pm;
    protected gun shootingDirection;
    // Start is called before the first frame update
   protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        shootingDirection = FindObjectOfType<gun>();
        currentCooldown = weaponData.CooldownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0) {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
