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

    [SerializeField] private Player player;

    [SerializeField] private EnergyDrinkInventory inventory;
    [SerializeField] private PangHaamYard blockSystem;
    [SerializeField] private int restoreAmount = 2;

    [SerializeField] private AudioClip drinkSFX;

    [SerializeField] private float drinkDuration = 0.3f;

    public void OnDrink(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsDrinking) return;

        // 🔥 สำคัญ: ล็อค state จาก Player
        if (player != null && player.IsBusy()) return;

        if (inventory == null || !inventory.UseDrink())
            return;

        StartCoroutine(DrinkRoutine());
    }

    private IEnumerator DrinkRoutine()
    {
        IsDrinking = true;
        OnDrinkStart?.Invoke();

        if (AudioManager.Instance != null && drinkSFX != null)
            AudioManager.Instance.PlaySFX(drinkSFX);

        blockSystem.RestoreCharge(restoreAmount);

        yield return new WaitForSeconds(drinkDuration);

        IsDrinking = false;
        OnDrinkEnd?.Invoke();
    }
}
