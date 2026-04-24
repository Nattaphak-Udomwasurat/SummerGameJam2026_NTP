using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State { Patrol, Chase, Attack }
public class SexyGirlBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private Transform player;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject noticePrefab;

    [Header("Attack")]
    [SerializeField] private float noticeTime = 0.3f;
    [SerializeField] private float splashForce = 7f;
    [SerializeField] private Transform splashPoint;
    [SerializeField] private Transform noticePoint;

    private Vector2 patrolTarget;
    private State currentState;

    [SerializeField] private GameObject shadow;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private float attackChance = 0.7f;
    [SerializeField] private Vector2 splashDelayRange = new Vector2(0.2f, 1.2f);

    private Transform currentTarget;

    private PangHaamYard playerBlock;

    private bool decidedToAttack = false;
    private bool willAttack = false;
    private bool isWaitingToSplash = false;
    private bool hasAttacked = false;
    private bool isAttacking = false;

    void Start()
    {

        currentTarget = pointA;

        if (player != null)
            playerBlock = player.GetComponent<PangHaamYard>();
    }
    void Update()
    {
        SetShadow(shadow);

        if (enemy == null || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        //  Phase 1: Awareness → สุ่มว่าจะโจมตีไหม
        if (dist <= enemy.awarenessRadian && !decidedToAttack)
        {
            DecideAttack();
        }

        if (hasAttacked)
        {
            currentState = State.Patrol;
        }
        else if (dist <= enemy.splashingRadian && willAttack)
        {
            currentState = State.Attack;
        }
        else if (dist <= enemy.awarenessRadian && willAttack)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }

        // 🎮 State Behavior
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                if (willAttack && dist <= enemy.splashingRadian && !isAttacking && !isWaitingToSplash)
                {
                    StartCoroutine(PrepareSplash());
                }
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
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x < 0)
        {

            transform.localScale = new Vector3(1, 1, 1);
        }
        transform.position += (Vector3)(dir * enemy.moveSpeed * Time.deltaTime);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        GameObject notice = Instantiate(noticePrefab, noticePoint.position, Quaternion.identity);

        float noticeStartTime = Time.time; // 🔥 เวลาเริ่ม notice

        float timer = 0f;
        bool blocked = false;

        while (timer < noticeTime)
        {
            if (playerBlock != null && playerBlock.IsBlocking)
            {
                // ✅ ต้อง block "หลัง notice"
                if (playerBlock.lastBlockTime >= noticeStartTime)
                {
                    blocked = true;
                    break;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(notice);

        if (blocked)
        {
            Debug.Log("Perfect Block!");
            hasAttacked = true;
            yield break;
        }

        Splash();
        hasAttacked = true;
    }

    void Splash()
    {
        Vector2 dir = (player.position - splashPoint.position).normalized;

        GameObject water = Instantiate(waterPrefab, splashPoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        water.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = water.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * splashForce;
        }
    }

    private void DecideAttack()
    {
        decidedToAttack = true;

        float roll = Random.value; // 0 - 1
        willAttack = roll <= attackChance;

        Debug.Log(willAttack ? "Enemy decided to ATTACK" : "Enemy decided to IGNORE");
    }

    private IEnumerator PrepareSplash()
    {
        isWaitingToSplash = true;

        float delay = Random.Range(splashDelayRange.x, splashDelayRange.y);

        yield return new WaitForSeconds(delay);

        StartCoroutine(AttackRoutine());
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

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemy.awarenessRadian);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy.splashingRadian);
    }
}
