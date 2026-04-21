using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    [SerializeField] private float laneTolerance = 2f; // ปรับได้

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnergyDrinkInventory inv = collision.GetComponent<EnergyDrinkInventory>();

        if (inv != null)
        {
            float yDiff = Mathf.Abs(transform.position.y - collision.transform.position.y);

            if (yDiff > laneTolerance)
                return;

            bool success = inv.AddDrink();

            if (success)
            {
                Destroy(gameObject);
            }
        }
    }
}
