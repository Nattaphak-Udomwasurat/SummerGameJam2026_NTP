using UnityEngine;
using UnityEngine.UI;


public class WetnessBar : MonoBehaviour
{
    [Header("Reference")]
    public PlayerWet playerWet;

    [Header("UI")]
    public Slider wetSlider;

    void Start()
    {
        // set ค่าเริ่มต้น
        wetSlider.maxValue = playerWet.maxWet;
        wetSlider.value = playerWet.currentWet;
    }

    void OnEnable()
    {
        playerWet.OnTakeWet += UpdateUI;
        playerWet.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        playerWet.OnTakeWet -= UpdateUI;
        playerWet.OnGameOver -= OnGameOver;
    }

    void UpdateUI()
    {
        wetSlider.value = playerWet.currentWet;
    }

    void OnGameOver()
    {
        Debug.Log("UI: Game Over Triggered");

        // เช่น:
        // แถบเต็มสีแดง
        // เปิด Game Over Panel
    }
}
