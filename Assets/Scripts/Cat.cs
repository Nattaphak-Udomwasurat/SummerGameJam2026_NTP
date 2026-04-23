using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] private Transform player;
    private PangHaamYard pangHaamYard;

    [SerializeField] private AudioClip catSFX;

    [SerializeField] private GameObject noticePrefab;
    [SerializeField] private Transform noticePoint;

    private GameObject currentNotice;
    void Start()
    {
        if (player != null)
            pangHaamYard = player.GetComponent<PangHaamYard>();
    }

    void Update()
    {
        if (player == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // 🟡 อยู่ในระยะ → แสดง notice
        if (dist <= enemy.splashingRadian)
        {
            ShowNotice();

            // 🔥 เช็ค block = ลูบแมว
            if (pangHaamYard != null && pangHaamYard.IsBlocking)
            {
                TriggerCatEnding();
            }
        }
        else
        {
            HideNotice();
        }
    }

    void HandleNotice(float dist)
    {
        // 🎯 อยู่ในระยะ → ต้องมี notice
        if (dist <= enemy.splashingRadian)
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

    void ShowNotice()
    {
        if (currentNotice == null)
        {
            if (AudioManager.Instance != null && catSFX != null)
                AudioManager.Instance.PlaySFX(catSFX);

            currentNotice = Instantiate(noticePrefab, noticePoint.position, Quaternion.identity, transform);
        }
    }

    void HideNotice()
    {
        if (currentNotice != null)
        {
            Destroy(currentNotice);
            currentNotice = null;
        }
    }

    void TriggerCatEnding()
    {
        Debug.Log("🐱 Cat Lover Ending!");

        GameManager.Instance.SetEnding(GameManager.EndingType.CatLover);
        GameManager.Instance.TriggerGameOver();

        enabled = false; // กันยิงซ้ำ
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