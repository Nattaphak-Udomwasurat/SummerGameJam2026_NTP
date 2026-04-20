using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [SerializeField] private PlayerWet playerWet;

    private PangHaamYard playerBlock;
    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerBlock = collision.GetComponent<PangHaamYard>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
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

    void EnterWork()
    {
        Debug.Log("🏢 Enter Work!");

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
