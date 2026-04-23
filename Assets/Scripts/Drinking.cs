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

    [SerializeField] private GameObject refreshPrefab;
    [SerializeField] private Transform refreshPoint;
    private GameObject currentRefresh;

    [SerializeField] private AudioClip drinkSFX;

    [SerializeField] private float drinkDuration = 0.3f;

    public void OnDrink(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsDrinking) return;

        // 🔥 สำคัญ: ล็อค state จาก Player
        if (player != null && player.IsBusy()) return;

        if (currentRefresh == null)
        {
            currentRefresh = Instantiate(refreshPrefab, refreshPoint.position, Quaternion.identity, transform);
        }

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

        GameObject currentRefresh = null;
        SpriteRenderer sr = null;

        if (refreshPrefab != null)
        {
            currentRefresh = Instantiate(refreshPrefab, refreshPoint.position, Quaternion.identity, transform);
        }

        float t = 0f;

        while (t < drinkDuration)
        {
            t += Time.deltaTime;

            // fade alpha 1 → 0
            if (sr != null)
            {
                float alpha = 1f - (t / drinkDuration);
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }

        // กันค่าหลุด
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }

        if (currentRefresh != null)
            Destroy(currentRefresh);

        IsDrinking = false;
        OnDrinkEnd?.Invoke();
    }
}
