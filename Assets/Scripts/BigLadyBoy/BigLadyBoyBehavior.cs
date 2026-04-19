using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLadyBoyBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private Transform player;

    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject noticePrefab;
    private GameObject currentNotice;

    [Header("Attack")]
    [SerializeField] private float noticeTime = 0.3f;
    [SerializeField] private float splashForce = 7f;
    [SerializeField] private Transform splashPoint;
    [SerializeField] private Transform noticePoint;

    [SerializeField] private float chaseSpeedMultiplier = 1.5f;

    private Vector2 patrolTarget;
    private StatePunk currentState;

    private Transform currentTarget;

    private PangHaamYard playerBlock;

    private bool hasAttacked = false;
    private bool isAttacking = false;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = pointA;

        if (player != null)
            playerBlock = player.GetComponent<PangHaamYard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // 🎯 1. คำนวณ State ก่อน
        if (dist <= enemy.awarenessRadian)
            currentState = StatePunk.Chase;
        else
            currentState = StatePunk.Patrol;

        // 🎯 2. จัดการ Notice ตาม state
        HandleNotice();

        // 🎯 3. อัปเดตตำแหน่ง notice
        if (currentNotice != null)
        {
            currentNotice.transform.position = noticePoint.position;
        }

        // 🎮 4. ทำพฤติกรรม
        switch (currentState)
        {
            case StatePunk.Patrol:
                Patrol();
                break;

            case StatePunk.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
        MoveTo(currentTarget.position);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.2f)
        {

            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void Chase()
    {
        MoveTo(player.position);
    }

    void MoveTo(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        // 🔥 หันตามทิศ
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }
        float dist = Vector2.Distance(transform.position, player.position);

        float currentSpeed = enemy.moveSpeed;

        // 🔥 ถ้าเข้า splashingRadian → เร็วขึ้น
        if (dist <= enemy.splashingRadian)
        {
            currentSpeed *= chaseSpeedMultiplier;
        }

        transform.position += (Vector3)(dir * currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player!");

            // ตัวอย่าง:
            var wet = collision.gameObject.GetComponent<PlayerWet>();
            if (wet != null)
            {
                wet.TakeWaterHit(); // หรือเปลี่ยนเป็น damage ก็ได้
            }
            Destroy(currentNotice);
            enabled = false;
        }
    }
    void HandleNotice()
    {

        if (currentNotice == null)
        {
            currentNotice = Instantiate(noticePrefab, noticePoint.position, Quaternion.identity, transform);
        }

        else
        {
            if (currentNotice != null)
            {
                Destroy(currentNotice);
                currentNotice = null;
            }
        }
    }

    private Transform FindPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemy.splashingRadian);

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

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemy.awarenessRadian);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy.splashingRadian);
    }
}
