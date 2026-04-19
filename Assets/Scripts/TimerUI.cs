using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// TimerUI — แสดงเวลานับถอยหลังเป็นตัวเลข
/// Subscribe จาก GameManager.OnTimeChanged
/// รองรับทั้ง TextMeshPro และ Legacy Text
/// </summary>
public class TimerUI : MonoBehaviour
{
    // ─── References ───────────────────────────────────────────
    [Header("Text Reference (ใส่อันใดอันหนึ่ง)")]
    [SerializeField] private TMP_Text tmpText;   // TextMeshPro (แนะนำ)
    [SerializeField] private Text legacyText; // Legacy UI Text

    // ─── Display Format ───────────────────────────────────────
    [Header("Format")]
    [SerializeField] private TimerFormat format = TimerFormat.MMSS;

    public enum TimerFormat
    {
        Seconds,    // 42
        SecondsF1,  // 42.3
        MMSS,       // 00:42
        MMSSf,      // 00:42.3
    }

    // ─── Color Warning ────────────────────────────────────────
    [Header("Warning Color (เมื่อเวลาใกล้หมด)")]
    [SerializeField] private bool useWarningColor = true;
    [SerializeField] private float warningThreshold = 10f;   // วินาที
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    // ══════════════════════════════════════════════════════════
    //  Unity Lifecycle
    // ══════════════════════════════════════════════════════════

    private void OnEnable()
    {
        GameManager.OnTimeChanged += HandleTimeChanged;
    }

    private void OnDisable()
    {
        GameManager.OnTimeChanged -= HandleTimeChanged;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            // ✅ FIX: ดึงค่าจาก GameManager ทันที (timeRemaining = totalTime แล้วตั้งแต่ Awake)
            HandleTimeChanged(GameManager.Instance.GetTimeRemaining(),
                              GameManager.Instance.GetTotalTime());
        }
        else
        {
            SetText("--:--");
            Debug.LogWarning("[TimerUI] GameManager.Instance is null — ตรวจสอบว่า GameManager อยู่ใน Scene แรกและ DontDestroyOnLoad ทำงานแล้ว");
        }
    }

    // ══════════════════════════════════════════════════════════
    //  Handler
    // ══════════════════════════════════════════════════════════

    private void HandleTimeChanged(float timeRemaining, float totalTime)
    {
        string display = FormatTime(timeRemaining);
        SetText(display);

        if (useWarningColor)
            SetColor(timeRemaining <= warningThreshold ? warningColor : normalColor);
    }

    // ══════════════════════════════════════════════════════════
    //  Format Helper
    // ══════════════════════════════════════════════════════════

    private string FormatTime(float t)
    {
        t = Mathf.Max(t, 0f);

        switch (format)
        {
            case TimerFormat.Seconds:
                return Mathf.CeilToInt(t).ToString();

            case TimerFormat.SecondsF1:
                return t.ToString("F1");

            case TimerFormat.MMSS:
                int min = Mathf.FloorToInt(t / 60f);
                int sec = Mathf.FloorToInt(t % 60f);
                return $"{min:00}:{sec:00}";

            case TimerFormat.MMSSf:
                int minF = Mathf.FloorToInt(t / 60f);
                int secF = Mathf.FloorToInt(t % 60f);
                int ms = Mathf.FloorToInt((t % 1f) * 10f);
                return $"{minF:00}:{secF:00}.{ms}";

            default:
                return Mathf.CeilToInt(t).ToString();
        }
    }

    // ══════════════════════════════════════════════════════════
    //  Text / Color Setter
    // ══════════════════════════════════════════════════════════

    private void SetText(string value)
    {
        if (tmpText != null) tmpText.text = value;
        if (legacyText != null) legacyText.text = value;
    }

    private void SetColor(Color color)
    {
        if (tmpText != null) tmpText.color = color;
        if (legacyText != null) legacyText.color = color;
    }
}