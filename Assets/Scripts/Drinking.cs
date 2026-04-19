using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drinking : MonoBehaviour
{
    public bool IsDrinking { get; private set; }

    public event Action OnDrinkStart;
    public event Action OnDrinkEnd;

    [SerializeField] private EnergyDrinkInventory inventory;
    [SerializeField] private PangHaamYard blockSystem;
    [SerializeField] private int restoreAmount = 2;


    [SerializeField] private float drinkDuration = 0.3f;

    public void OnDrink(InputAction.CallbackContext context)
    {
        // ✅ สำหรับ Unity Event ใช้ performed
        if (!context.performed) return;

        if (IsDrinking) return;

        if (inventory == null || !inventory.UseDrink())
            return;

        StartCoroutine(DrinkRoutine());
    }

    private IEnumerator DrinkRoutine()
    {
        IsDrinking = true;
        OnDrinkStart?.Invoke();

        blockSystem.RestoreCharge(restoreAmount);

        yield return new WaitForSeconds(drinkDuration);

        IsDrinking = false;
        OnDrinkEnd?.Invoke();
    }
}
