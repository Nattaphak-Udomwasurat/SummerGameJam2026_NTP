using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWet : MonoBehaviour
{
    public int maxWet = 5;
    public int currentWet { get; private set; }

    public event Action OnTakeWet;
    public event Action OnGameOver;

    public void TakeWaterHit()
    {
        currentWet++;

        Debug.Log("Wet: " + currentWet);

        OnTakeWet?.Invoke();

        if (currentWet >= maxWet)
        {
            GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            TakeWaterHit();
            Destroy(collision.gameObject);
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        OnGameOver?.Invoke();

        // เช่น:
        // Time.timeScale = 0f;
    }
}
