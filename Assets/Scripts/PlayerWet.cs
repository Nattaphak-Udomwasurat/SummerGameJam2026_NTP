using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWet : MonoBehaviour
{
    [SerializeField] GameObject getWet;
    [SerializeField] GameObject getDinsorpong;
    public int maxWet = 5;
    public int currentWet { get; private set; }

    public event Action OnTakeWet;
    public event Action OnGameOver;

    public void Start()
    {
        getWet.SetActive(false);
        getDinsorpong.SetActive(false);
    }

    public void TakeWaterHit()
    {
        currentWet++;

        Debug.Log("Wet: " + currentWet);

        OnTakeWet?.Invoke();

        if (currentWet >= maxWet)
        {
            GameManager.Instance.SetEnding(GameManager.EndingType.WetGameOver);
            GameManager.Instance.TriggerGameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water" || collision.gameObject.tag == "Dinsorpong")
        {
            if (collision.gameObject.tag == "Water")
            {
                StartCoroutine(GetWet());
            }
            else if (collision.gameObject.tag == "Dinsorpong")
            {
                StartCoroutine(GetDinsorpong());
            }

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

    IEnumerator GetWet()
    {
        getWet.SetActive(true);
        yield return new WaitForSeconds(1f);
        getWet.SetActive(false);
    }

    IEnumerator GetDinsorpong()
    {
        getDinsorpong.SetActive(true);
        yield return new WaitForSeconds(1f);
        getDinsorpong.SetActive(false);
    }
}
