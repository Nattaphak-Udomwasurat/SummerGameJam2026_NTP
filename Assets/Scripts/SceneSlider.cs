using System.Collections;
using UnityEngine;

public class SceneSlider : MonoBehaviour
{
    [SerializeField] private GameObject slide;
    [SerializeField] private float fadeDuration = 0.8f;

    private bool fadeComplete = false;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (fadeComplete && Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.GoToMainMenu();
        }
    }

    private IEnumerator FadeIn()
    {
        CanvasGroup cg = slide.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = slide.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
        fadeComplete = true; // ✅ รอให้ fade จบก่อนถึงรับคลิก
    }
}