using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public bool IsStunned { get; private set; }

    [SerializeField] private float stunDuration = 2f;

    public void Stun()
    {
        if (IsStunned) return;
        StartCoroutine(StunRoutine());
    }

    private IEnumerator StunRoutine()
    {
        IsStunned = true;

        Debug.Log("Player Stunned!");

        //  ปิดการควบคุม (คุณไปเช็ค IsStunned ใน movement script)
        yield return new WaitForSeconds(stunDuration);

        IsStunned = false;

        Debug.Log("Player Recovered!");
    }
}
