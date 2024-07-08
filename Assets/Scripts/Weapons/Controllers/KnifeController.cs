using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position;
        Vector3 mousePos = shootingDirection.transform.position-transform.position;
        Vector3 rotation = transform.position - shootingDirection.transform.position;
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(mousePos,rotation);

    }
}
