using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnergyDrinkInventory inv = collision.GetComponent<EnergyDrinkInventory>();

        if (inv != null)
        {
            bool success = inv.AddDrink();

            if (success)
            {
                Destroy(gameObject);
            }
        }
    }
}
