using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HambugerPassiveItem : PassiveItemController
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multipler / 100f;
    }
}
