using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Freeze playerFreeze;
    [SerializeField] private Player player;
    [SerializeField] private GameObject shadowPrefab;

    public Vector2 MoveInput => moveInput;

    private float moveSpeed = 5f;
    private Vector2 moveInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetShadow(shadowPrefab);
    }

    private void FixedUpdate()
    {
        if (playerFreeze != null && playerFreeze.IsStunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // ✅ เดินได้ตลอด ไม่สน IsBusy
        rb.velocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void SetShadow(GameObject effect)
    {
        SpriteRenderer playerSR = GetComponent<SpriteRenderer>();
        SpriteRenderer effectSR = effect.GetComponent<SpriteRenderer>();

        if (playerSR != null && effectSR != null)
        {
            effectSR.sortingOrder = playerSR.sortingOrder - 1;
        }
    }
}
