using System;
using UnityEngine;

public class EnergyDrinkInventory : MonoBehaviour
{
    public int maxAmount = 2;
    public int currentAmount { get; private set; }

    public event Action<int, int> OnAmountChanged; // current, max

    void Start()
    {
        OnAmountChanged?.Invoke(currentAmount, maxAmount);
    }

    public bool AddDrink(int amount = 1)
    {
        if (currentAmount >= maxAmount)
            return false;

        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);

        OnAmountChanged?.Invoke(currentAmount, maxAmount);

        return true;
    }

    public bool UseDrink()
    {
        if (currentAmount <= 0)
            return false;

        currentAmount--;

        OnAmountChanged?.Invoke(currentAmount, maxAmount);

        return true;
    }
}
