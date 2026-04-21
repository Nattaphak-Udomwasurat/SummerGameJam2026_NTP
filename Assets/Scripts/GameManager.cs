using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum EndingType
    {
        None,
        WetGameOver,      // Scene 1
        NormalWorker,     // Scene 2
        LateAndPoor,      // Scene 3
        Promotion,        // Scene 4
        CatLover,         // Scene 5
        StayHome          // Scene 6
    }
    public static GameManager Instance { get; private set; }

    public EndingType currentEnding = EndingType.None;

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

    [Header("Gallery")]
    [SerializeField] private GameObject galleryPanel;

    [Header("Credit")]
    [SerializeField] private GameObject creditPanel;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentState = GameState.MainMenu; 

        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if(CurrentState != GameState.Playing) return;

        if (isClockRunning)
            TickClock();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuScene)
        {
            galleryPanel = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(obj => obj.name == "GalleryPanel");

            creditPanel = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(obj => obj.name == "CreditPanel");
        }
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
        if (CurrentState != GameState.Playing) return; 

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
        currentEnding = EndingType.None;

        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene(mainMenuScene);
    }

    /// <summary>ปุ่ม Start — เริ่มเกม</summary>
    public void StartGame()
    {
        currentEnding = EndingType.None; // 
        ChangeState(GameState.Playing);
        SceneManager.LoadScene(gameScene);
        StartClock();
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
        if (CurrentState != GameState.Playing) return; 

        if (CurrentState == GameState.GameOver) return;

        ChangeState(GameState.GameOver);
        isClockRunning = false;

        UnlockEnding(currentEnding);

        EndingController ending = FindObjectOfType<EndingController>();
        if (ending != null)
            ending.ShowEnding(currentEnding);
    }

    // ══════════════════════════════════════════════════════════
    //  ④ GALLERY — บันทึก Ending ที่ได้รับ
    // ══════════════════════════════════════════════════════════

    public void UnlockEnding(EndingType ending)
    {
        if (ending == EndingType.None) return;
        PlayerPrefs.SetInt("Ending_" + ending.ToString(), 1);
        PlayerPrefs.Save();
        Debug.Log($"[GameManager] Unlocked Ending: {ending}");
    }

    public bool IsEndingUnlocked(EndingType ending)
    {
        return PlayerPrefs.GetInt("Ending_" + ending.ToString(), 0) == 1;
    }

    // (optional) ใช้ตอน dev เพื่อ reset
    public void ClearAllEndings()
    {
        foreach (EndingType e in System.Enum.GetValues(typeof(EndingType)))
            PlayerPrefs.DeleteKey("Ending_" + e.ToString());
        PlayerPrefs.Save();
    }
    public void OpenGallery()
    {
        if (galleryPanel == null) return;
        galleryPanel.SetActive(true);
    }

    public void CloseGallery()
    {
        if (galleryPanel == null) return;
        galleryPanel.SetActive(false);
    }


    public void OpenCreditWindow()
    {
        if (creditPanel == null) return;
        creditPanel.SetActive(true);
    }

    public void CloseCreditWindow()
    {
        if (creditPanel == null) return;
        creditPanel.SetActive(false);
    }
    /// <summary>เล่น End Scene แล้วไป Over Menu</summary>


    /// <summary>จาก Game Over → กลับ Main Menu</summary>
    public void ReturnToMainMenuFromGameOver()
    {
        GoToMainMenu();
    }

    // ══════════════════════════════════════════════════════════
    //  Helper
    // ══════════════════════════════════════════════════════════

    public void SetEnding(EndingType ending)
    {
        if (currentEnding != EndingType.None) return;
        currentEnding = ending;
    }

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