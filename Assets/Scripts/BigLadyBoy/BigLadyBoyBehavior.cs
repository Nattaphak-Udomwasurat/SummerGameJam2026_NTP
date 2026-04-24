using System.Collections;
using UnityEngine;

public class BigLadyBoyBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private Transform player;
    [SerializeField] private Freeze playerFreeze;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private AudioClip ladyboySFX;

    [SerializeField] private GameObject noticePrefab;
    [SerializeField] private Transform noticePoint;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 24f;
    [SerializeField] private float noticeTime = 0.3f;
    [SerializeField] private float lifeAfterDash = 3f;

    private GameObject currentNotice;
    private Vector2 dashDirection;

    private bool isPreparing = false;
    private bool isDashing = false;

    private Vector2 dashTarget;

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (player == null || enemy == null || isPreparing || isDashing) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // เข้า range → เริ่ม attack
        if (dist <= enemy.splashingRadian)
        {
            StartCoroutine(PrepareAndDash());
        }
    }

    IEnumerator PrepareAndDash()
    {
        isPreparing = true;

        currentNotice = Instantiate(noticePrefab, noticePoint.position, Quaternion.identity, transform);

        dashTarget = player.position;

        //  เปิดเส้น
        if (lineRenderer != null)
            lineRenderer.enabled = true;

        float timer = 0f;

        while (timer < noticeTime)
        {
            // อัปเดตเส้นตลอดเวลา
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, dashTarget);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (currentNotice != null)
            Destroy(currentNotice);

        //  ปิดเส้นก่อน dash
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        isPreparing = false;

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        isDashing = true;

        float timer = 0f;
        dashDirection = (dashTarget - (Vector2)transform.position).normalized;

        if (AudioManager.Instance != null && ladyboySFX != null)
            AudioManager.Instance.PlaySFX(ladyboySFX);

        if (dashDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dashDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        while (timer < lifeAfterDash)
        {
            // 🔥 ใช้ direction เดิมตลอด
            transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDashing) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Dash Hit Player!");

            if (playerFreeze != null)
            {
                playerFreeze.Stun();
            }

            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (currentNotice != null)
        {
            currentNotice.transform.position = noticePoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy.splashingRadian);
    }
}