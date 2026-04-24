using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwing : MonoBehaviour
{
    public bool IsThrowing { get; private set; }

    public event Action OnThrowStart;
    public event Action OnThrowEnd;

    [SerializeField] private Player player;

    [SerializeField] private AudioClip throwSFX;

    [SerializeField] private Freeze freeze;

    [SerializeField] private EnergyDrinkInventory inventory;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 8f;
    [SerializeField] private float searchRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Transform FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyLayer);

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsThrowing) return;

        // เพิ่มตรงนี้
        if (freeze != null && freeze.IsStunned) return;

        if (player != null && player.IsBusy()) return;

        Transform target = FindClosestEnemy();
        if (target == null) return;

        if (inventory == null || !inventory.UseDrink())
            return;

        StartCoroutine(ThrowRoutine(target));
    }

    private IEnumerator ThrowRoutine(Transform target)
    {
        IsThrowing = true;
        OnThrowStart?.Invoke();

        if (AudioManager.Instance != null && throwSFX != null)
            AudioManager.Instance.PlaySFX(throwSFX);

        yield return new WaitForSeconds(0.1f);

        ThrowProjectile(target);

        yield return new WaitForSeconds(0.2f); // timing animation

        IsThrowing = false;
        OnThrowEnd?.Invoke();
    }

    private void ThrowProjectile(Transform target)
    {
        GameObject proj = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);

        Vector2 dir = (target.position - throwPoint.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * throwForce;
        }
    }
}
