using UnityEngine;

public class EndingController : MonoBehaviour
{
    public GameObject wetEnding;
    public GameObject normalEnding;
    public GameObject lateEnding;
    public GameObject richEnding;
    public GameObject catEnding;
    public GameObject homeEnding;

    public void ShowEnding(GameManager.EndingType ending)
    {
        // ปิดทั้งหมดก่อน
        wetEnding.SetActive(false);
        normalEnding.SetActive(false);
        lateEnding.SetActive(false);
        richEnding.SetActive(false);
        catEnding.SetActive(false);
        homeEnding.SetActive(false);

        switch (ending)
        {
            case GameManager.EndingType.WetGameOver:
                wetEnding.SetActive(true);
                break;

            case GameManager.EndingType.NormalWorker:
                normalEnding.SetActive(true);
                break;

            case GameManager.EndingType.LateAndPoor:
                lateEnding.SetActive(true);
                break;

            case GameManager.EndingType.Promotion:
                richEnding.SetActive(true);
                break;

            case GameManager.EndingType.CatLover:
                catEnding.SetActive(true);
                break;

            case GameManager.EndingType.StayHome:
                homeEnding.SetActive(true);
                break;
        }
    }
}