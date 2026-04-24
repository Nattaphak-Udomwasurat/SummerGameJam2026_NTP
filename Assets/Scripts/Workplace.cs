using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [SerializeField] private PlayerWet playerWet;
    [SerializeField] private GameObject noticePrefab;
    [SerializeField] private Transform noticePoint;
    private GameObject currentNotice;

    private PangHaamYard playerBlock;
    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerBlock = collision.GetComponent<PangHaamYard>();

            ShowNotice();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerBlock = null;

            HideNotice();
        }
    }

    void Update()
    {
        if (!isPlayerInside || playerBlock == null) return;

        if (playerBlock.IsBlocking)
        {
            EnterWork();
        }
    }

    void ShowNotice()
    {
        if (currentNotice == null)
        {
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

    void EnterWork()
    {
        Debug.Log("Enter Work!");

        if (playerWet.currentWet > 0)
        {
            GameManager.Instance.SetEnding(GameManager.EndingType.NormalWorker);
        }
        else
        {
            GameManager.Instance.SetEnding(GameManager.EndingType.Promotion);
        }

        GameManager.Instance.TriggerGameOver();

        enabled = false; 
    }
}
