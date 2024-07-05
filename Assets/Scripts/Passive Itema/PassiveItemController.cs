using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemController : MonoBehaviour
{
    protected  PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {
        
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

}
