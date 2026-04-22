using System.Collections;
using UnityEngine;

public class KidBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Transform player;
    [SerializeField] public Transform noticePoint;
    [SerializeField] private Transform splashPoint;
    [SerializeField] private float splashForce = 7f;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject noticePrefab;
    [SerializeField] private float attackChance = 0.7f; 
    [SerializeField] private Vector2 splashDelayRange = new Vector2(0.2f, 1.2f);

    [SerializeField] private float noticeTime = 0.2f;

    public bool hasAttacked = false;
    private bool isChecking = false;
    private bool decidedToAttack = false; 
    private bool willAttack = false;
    private bool isWaitingToSplash = false;
    public bool didSplash = false;

    private PangHaamYard playerBlock;

    private void Start()
    {
        if (player != null)
            playerBlock = player.GetComponent<PangHaamYard>();
    }

    private void Update()
    {
        FacePlayer();

        if (enemy == null || player == null || hasAttacked) return;


        float dist = Vector2.Distance(transform.position, player.position);

        // 👀 Phase 1
        if (dist <= enemy.awarenessRadian && !decidedToAttack)
        {
            DecideAttack();
        }

        // 💣 Phase 2
        if (willAttack && dist <= enemy.splashingRadian && !isChecking && !isWaitingToSplash)
        {
            StartCoroutine(PrepareSplash());
        }
    }

    private IEnumerator SplashCheck()
    {
        isChecking = true;

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
            didSplash = false; //  ไม่ยิง
            hasAttacked = true;
            yield break;
        }

        SplashWater();
        didSplash = true; //  ยิงจริง
        hasAttacked = true;
    }

    private void SplashWater()
    {
        Vector2 dir = (player.position - splashPoint.position).normalized;

        GameObject water = Instantiate(waterPrefab, splashPoint.position, Quaternion.identity);

        // 🔥 หมุนให้หันไปทาง player
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

        StartCoroutine(SplashCheck());
    }

    void FacePlayer()
    {
        if (player == null || spriteRenderer == null) return;

        float dirX = player.position.x - transform.position.x;

        // กัน jitter ตอนอยู่ตรงกลางพอดี
        if (Mathf.Abs(dirX) > 0.05f)
        {
            spriteRenderer.flipX = dirX > 0;
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