using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Freeze playerFreeze;
    [SerializeField] private Player player;

    private float moveSpeed = 5f;
    private Vector2 moveInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
}
