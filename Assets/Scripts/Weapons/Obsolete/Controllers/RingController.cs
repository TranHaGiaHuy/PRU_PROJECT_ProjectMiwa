using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : WeaponController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spwanedRing = Instantiate(weaponData.Prefab);
        spwanedRing.transform.position = transform.position;
        spwanedRing.transform.parent = transform;
    }
}
