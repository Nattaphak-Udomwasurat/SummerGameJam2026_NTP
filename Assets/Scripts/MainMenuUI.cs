using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnStart()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickGallery()
    {
        GameManager.Instance.OpenGallery();
    }

    public void OnCloseGallery()
    {
        GameManager.Instance.CloseGallery();
    }

    public void OnClickCredit()
    {
        GameManager.Instance.OpenCreditWindow();
    }
    public void OnCloseCredit()
    {
        GameManager.Instance.CloseCreditWindow();
    }
}