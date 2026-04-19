using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDrinkUI : MonoBehaviour
{
    [Header("Reference")]
    public EnergyDrinkInventory inventory;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI amountText;

    void OnEnable()
    {
        inventory.OnAmountChanged += UpdateUI;
    }

    void OnDisable()
    {
        inventory.OnAmountChanged -= UpdateUI;
    }

    void Start()
    {
        UpdateUI(inventory.currentAmount, inventory.maxAmount);
    }

    void UpdateUI(int current, int max)
    {
        amountText.text = "x" + current;

        // ถ้าอยากให้ icon จางตอน 0
        icon.color = (current > 0) ? Color.white : new Color(1, 1, 1, 0.3f);
    }
}
