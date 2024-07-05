using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : PickupItem
{
    public int exp;
    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExp(exp);
    }
   
}
