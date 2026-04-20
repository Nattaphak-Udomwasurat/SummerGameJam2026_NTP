using UnityEngine;
using UnityEngine.UI;

public class EnergyDrinkUI : MonoBehaviour
{
    [Header("Reference")]
    public EnergyDrinkInventory inventory;

    [Header("UI Icons")]
    public Image icon1;
    public Image icon2;

    [Header("Colors")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1, 1, 1, 0.3f);

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
        // 🔥 reset เป็นเทาก่อน
        icon1.color = inactiveColor;
        icon2.color = inactiveColor;

        // 🎯 เปิดตามจำนวน
        if (current >= 1)
            icon1.color = activeColor;

        if (current >= 2)
            icon2.color = activeColor;
    }
}
