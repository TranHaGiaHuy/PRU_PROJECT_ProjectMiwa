using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UsingAnvil();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void UsingAnvil()
    {
        if (inventory.GetPossibleEvolutions().Count<=0)
        {
            Debug.LogWarning("No Available Evolution");
            return;
        }
        WeaponEvolution toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
