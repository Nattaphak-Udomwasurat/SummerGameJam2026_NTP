using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager — ควบคุมระบบหลักของเกม
/// จัดการ: Clocking (นับถอยหลัง), Main Menu, Game Over
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ─── Game State ───────────────────────────────────────────
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    // ─── Clocking (นับถอยหลัง) ────────────────────────────────
    [Header("Clocking Settings")]
    [SerializeField] private float totalTime = 60f;
    [Tooltip("ติ๊กถูก = เริ่มนับทันทีที่ Play โดยไม่ต้องกด Start (สำหรับทดสอบ)")]
    [SerializeField] private bool autoStartOnPlay = false;
    private float timeRemaining;
    private bool isClockRunning = false;

    // ─── Scene Names ──────────────────────────────────────────
    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameScene = "GameScene";
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string endCutScene = "EndCutScene";

    // ─── Events ───────────────────────────────────────────────
    public delegate void OnTimeChangedDelegate(float timeRemaining, float totalTime);
    public static event OnTimeChangedDelegate OnTimeChanged;

    public delegate void OnGameStateChangedDelegate(GameState newState);
    public static event OnGameStateChangedDelegate OnGameStateChanged;

    public static event System.Action OnTimeUp;

    // ══════════════════════════════════════════════════════════
    //  Unity Lifecycle
    // ══════════════════════════════════════════════════════════

    private void Awake()
    {
        // Singleton — คงอยู่ข้ามฉาก
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ตั้งค่าเริ่มต้นให้ timeRemaining = totalTime
        timeRemaining = totalTime;
    }

    private void Start()
    {
        if (autoStartOnPlay)
        {
            Debug.Log("[GameManager] autoStartOnPlay = true → StartClock()");
            ChangeState(GameState.Playing);
            StartClock();
        }
    }

    private void Update()
    {
        if (isClockRunning)
            TickClock();
    }

    // ══════════════════════════════════════════════════════════
    //  ① CLOCKING — นับถอยหลัง
    // ══════════════════════════════════════════════════════════

    /// <summary>เริ่มนับถอยหลัง</summary>
    public void StartClock()
    {
        timeRemaining = totalTime;
        isClockRunning = true;
        Debug.Log($"[GameManager] Clock started — {totalTime}s");
    }

    /// <summary>หยุดนับถอยหลัง (pause)</summary>
    public void PauseClock()
    {
        isClockRunning = false;
    }

    /// <summary>เดินนาฬิกาต่อ</summary>
    public void ResumeClock()
    {
        if (CurrentState == GameState.Playing)
            isClockRunning = true;
    }

    private void TickClock()
    {
        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0f);

        OnTimeChanged?.Invoke(timeRemaining, totalTime);

        if (timeRemaining <= 0f)
            TimeUp();
    }

    /// <summary>เวลาหมด → ไป Game Over</summary>
    private void TimeUp()
    {
        isClockRunning = false;
        Debug.Log("[GameManager] เวลาหมด!");
        OnTimeUp?.Invoke();
        TriggerGameOver();
    }

    // ══════════════════════════════════════════════════════════
    //  ② MAIN MENU
    // ══════════════════════════════════════════════════════════

    /// <summary>กลับไป Main Menu</summary>
    public void GoToMainMenu()
    {
        isClockRunning = false;
        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene(mainMenuScene);
    }

    /// <summary>ปุ่ม Start — เริ่มเกม</summary>
    public void StartGame()
    {
        ChangeState(GameState.Playing);
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
        StartClock();
        Debug.Log("[GameManager] Game Started");
    }

    /// <summary>ปุ่ม Credit</summary>
    public void OpenCredit()
    {
        Debug.Log("[GameManager] Open Credit");
        // TODO: เปิด Credit Panel หรือ Scene
    }

    /// <summary>ปุ่ม Option</summary>
    public void OpenOption()
    {
        Debug.Log("[GameManager] Open Option");
        // TODO: เปิด Option Panel
    }

    // ══════════════════════════════════════════════════════════
    //  ③ GAME OVER
    // ══════════════════════════════════════════════════════════

    /// <summary>เรียกเมื่อเกมจบ (เวลาหมด หรือแพ้)</summary>
    public void TriggerGameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        ChangeState(GameState.GameOver);
        isClockRunning = false;
        Debug.Log("[GameManager] Game Over — loading End Scene");

        StartCoroutine(PlayEndSceneRoutine());
    }

    /// <summary>เล่น End Scene แล้วไป Over Menu</summary>
    private IEnumerator PlayEndSceneRoutine()
    {
        // 1) โหลด End Cut Scene
        yield return SceneManager.LoadSceneAsync(endCutScene);

        Debug.Log("[GameManager] Playing End Cut Scene...");

        // 2) รอ Cut Scene จบ (ปรับเวลาตาม Cut Scene จริง)
        yield return new WaitForSeconds(3f);

        // 3) ไป Game Over / Over Menu
        yield return SceneManager.LoadSceneAsync(gameOverScene);
        Debug.Log("[GameManager] Over Menu loaded");
    }

    /// <summary>จาก Game Over → กลับ Main Menu</summary>
    public void ReturnToMainMenuFromGameOver()
    {
        GoToMainMenu();
    }

    // ══════════════════════════════════════════════════════════
    //  Helper
    // ══════════════════════════════════════════════════════════

    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
        Debug.Log($"[GameManager] State → {newState}");
    }

    /// <summary>เวลาที่เหลือ (อ่านจาก Script อื่น)</summary>
    public float GetTimeRemaining() => timeRemaining;
    public float GetTotalTime() => totalTime;
    public bool IsClockRunning() => isClockRunning;
}