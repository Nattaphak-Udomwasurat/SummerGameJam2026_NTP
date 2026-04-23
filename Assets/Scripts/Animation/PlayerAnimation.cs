using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private enum AnimState
    {
        Idle,
        Walk,
        Busy
    }

    [Header("Refs")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Player player;
    [SerializeField] private Freeze freeze;
    [SerializeField] private PlayerController controller;

    [Header("Walk")]
    [SerializeField] private List<Sprite> walkSprites; // 2 frame
    [SerializeField] private float walkDelay = 0.3f;

    [Header("Idle")]
    [SerializeField] private List<Sprite> idleSprites;
    [SerializeField] private float idleDelay = 0.5f;

    [Header("SFX")]
    [SerializeField] private AudioClip moveSFX;

    private AnimState currentState;

    private Coroutine currentRoutine;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        ChangeState(AnimState.Idle);
    }

    void Update()
    {
        // ถ้า player กำลังทำ action → override ทุกอย่าง
        if (player != null && player.IsBusy())
        {
            ChangeState(AnimState.Busy);
            return;
        }

        Vector3 delta = transform.position - lastPosition;
        bool isMoving = controller.MoveInput.sqrMagnitude > 0.01f;

        // flip
        float x = controller.MoveInput.x;

        if (x < 0) playerSprite.flipX = false;
        else if (x > 0) playerSprite.flipX = true;

        if (isMoving)
            ChangeState(AnimState.Walk);
        else
            ChangeState(AnimState.Idle);

        lastPosition = transform.position;
    }

    void ChangeState(AnimState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // stop ของเก่า
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        // start ใหม่
        switch (currentState)
        {
            case AnimState.Idle:
                currentRoutine = StartCoroutine(IdleRoutine());
                break;

            case AnimState.Walk:
                currentRoutine = StartCoroutine(WalkRoutine());
                break;

            case AnimState.Busy:
                // ไม่ทำ animation (ให้ Player.cs คุม sprite)
                break;
        }
    }

    IEnumerator WalkRoutine()
    {
        int index = 0;

        while (true)
        {
            if (AudioManager.Instance != null && moveSFX != null)
                AudioManager.Instance.PlaySFX(moveSFX);

            playerSprite.sprite = walkSprites[index];
            index = (index + 1) % walkSprites.Count;

            yield return new WaitForSeconds(walkDelay);
        }
    }

    IEnumerator IdleRoutine()
    {
        int index = 0;

        while (true)
        {
            playerSprite.sprite = idleSprites[index];
            index = (index + 1) % idleSprites.Count;

            yield return new WaitForSeconds(idleDelay);
        }
    }
}