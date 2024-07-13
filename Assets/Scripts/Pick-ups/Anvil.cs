using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
		PlayerInventory pi = collision.GetComponent<PlayerInventory>();
		if (pi)
		{
            Debug.LogWarning("DA NHAN CAI DE");
            bool randomBool = Random.Range(0, 2) == 0;
			UsingAnvil(pi, randomBool);
			Destroy(gameObject);
		}
	}

    // Update is called once per frame
    public void UsingAnvil(PlayerInventory inventory, bool isHigherTier)
    {
        foreach (PlayerInventory.Slot s in inventory.weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w == null)
            {
                break; ; // pass to other weapon slots if current weapon empty

            }
            if (w.data.evolutionData == null)
            {
                continue; // pass to other weapon slots if current weapon dont have any evolution
            }
            //loop for every posisible evolution that have weapon
            foreach (ItemData.Evolution e in w.data.evolutionData)
            {
                if (e.condition == ItemData.Evolution.Condition.anvil)
                {
                    bool attemp = w.AttempEvolution(e, 0);
                    if (attemp)
                    {
						return;
					}
                }
            }
        }
        GameManager.instance.StartLevelUp();
    }
}
