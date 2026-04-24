using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StatePunk { Patrol, Chase}

public class MotorPunkBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private Transform player;
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject noticePrefab;
    private GameObject currentNotice;

    [SerializeField] private float soundRange = 300f;
    [SerializeField] private AudioClip patrolSFX;
    [SerializeField] private AudioClip chaseSFX;
    [SerializeField] private AudioSource patrolAudioSource;

    [Header("Attack")]
    [SerializeField] private float splashForce = 7f;
    [SerializeField] private Transform splashPoint;
    [SerializeField] private Transform noticePoint;

    [SerializeField] private float chaseSpeedMultiplier = 1.5f;

    private Vector2 patrolTarget;
    private StatePunk currentState;

    private Transform currentTarget;

    private PangHaamYard playerBlock;

    private bool isPlayingChaseSFX = false;
    private bool isPlayingPatrolSFX = false;
    private bool hasAttacked = false;
    private bool isAttacking = false;
    private bool hasHitPlayer = false;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = pointA;

        if (player != null)
            playerBlock = player.GetComponent<PangHaamYard>();

        if (patrolAudioSource != null && patrolSFX != null)
        {
            patrolAudioSource.clip = patrolSFX;
            patrolAudioSource.loop = true;
            patrolAudioSource.volume = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        //  1. คำนวณ State ก่อน
        if (dist <= enemy.awarenessRadian)
            currentState = StatePunk.Chase;
        else
            currentState = StatePunk.Patrol;

        // 2. จัดการ Notice ตาม state
        HandleNotice();

        // 3. อัปเดตตำแหน่ง notice
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
        if (!patrolAudioSource.isPlaying)
            patrolAudioSource.Play();

        float dist = Vector2.Distance(transform.position, player.position);
        float volumeRatio = 1f - Mathf.Clamp01(dist / soundRange);
        patrolAudioSource.volume = volumeRatio * AudioManager.Instance.GetSFXVolume();

        MoveTo(currentTarget.position);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.2f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void Chase()
    {
        if (patrolAudioSource.isPlaying)
            patrolAudioSource.Stop();

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= soundRange)
        {
            if (!isPlayingChaseSFX)
            {
                AudioManager.Instance.PlaySFX(chaseSFX);
                isPlayingChaseSFX = true;
            }
        }
        else
        {
            isPlayingChaseSFX = false;
        }

        MoveTo(player.position);
    }

    void MoveTo(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        // หันตามทิศ
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }
        float dist = Vector2.Distance(transform.position, player.position);

        float currentSpeed = enemy.moveSpeed;

        // ถ้าเข้า splashingRadian → เร็วขึ้น
        if (dist <= enemy.splashingRadian)
        {
            currentSpeed *= chaseSpeedMultiplier;
        }

        transform.position += (Vector3)(dir * currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitPlayer) return; // กันซ้ำ

        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;

            var wet = collision.gameObject.GetComponent<PlayerWet>();
            if (wet != null)
            {
                wet.TakeWaterHit();
            }

            if (currentNotice != null)
                Destroy(currentNotice);

            // ปิด script กันทำงานซ้ำ
            enabled = false;
        }
    }
    void HandleNotice()
    {
        if (currentState == StatePunk.Chase)
        {
            if (currentNotice == null)
            {
                currentNotice = Instantiate(noticePrefab, noticePoint.position, Quaternion.identity, transform);
            }
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

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemy.awarenessRadian);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy.splashingRadian);
    }
}
