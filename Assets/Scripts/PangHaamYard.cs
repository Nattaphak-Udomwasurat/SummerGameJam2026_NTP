using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PangHaamYard : MonoBehaviour
{
    public bool IsBlocking { get; private set; }
    public float lastBlockTime { get; private set; }

    // 🔥 Event
    public event Action OnBlockStart;
    public event Action OnBlockEnd;
    public event Action<int, int> OnChargeChanged;

    [Header("Block Settings")]
    public float blockDuration = 0.5f;

    [Header("Charge Settings")]
    public int maxCharges = 4;
    public int currentCharges = 4;
    public float rechargeTime = 15f;

    private Coroutine blockRoutine;
    private Coroutine rechargeRoutine;

    void Start()
    {
        OnChargeChanged?.Invoke(currentCharges, maxCharges);
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (currentCharges <= 0) return;

        UseCharge();

        if (blockRoutine != null)
            StopCoroutine(blockRoutine);

        blockRoutine = StartCoroutine(BlockRoutine());
    }

    private IEnumerator BlockRoutine()
    {
        IsBlocking = true;

        lastBlockTime = Time.time; // 🔥 บันทึกเวลาที่เริ่ม block

        OnBlockStart?.Invoke();

        yield return new WaitForSeconds(blockDuration);

        IsBlocking = false;
        OnBlockEnd?.Invoke();
    }

    private void UseCharge()
    {
        currentCharges--;

        OnChargeChanged?.Invoke(currentCharges, maxCharges);

        if (rechargeRoutine == null)
            rechargeRoutine = StartCoroutine(RechargeRoutine());
    }
    public void RestoreCharge(int amount)
    {
        currentCharges += amount;
        currentCharges = Mathf.Clamp(currentCharges, 0, maxCharges);

        OnChargeChanged?.Invoke(currentCharges, maxCharges);
    }

    private IEnumerator RechargeRoutine()
    {
        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(rechargeTime);
            currentCharges++;

            OnChargeChanged?.Invoke(currentCharges, maxCharges);
        }

        rechargeRoutine = null;
    }
}
