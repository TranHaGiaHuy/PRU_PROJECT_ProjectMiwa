using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaponScriptableOpject", menuName ="ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{


    [SerializeField]
    new string name; 
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description; // 
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    float pierce; // do ben vu khi
    public float Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level; // 
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // 
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite icon; // 
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField]
    int evolvedIndexToRemove; // 
    public int EvolvedIndexToRemove { get => evolvedIndexToRemove; private set => evolvedIndexToRemove = value; }

}
