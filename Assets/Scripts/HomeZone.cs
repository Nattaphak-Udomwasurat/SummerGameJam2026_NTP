using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float radius = 3f;

    private bool isPlayerInside = true;
    private bool everLeft = false;

    private void OnEnable()
    {
        GameManager.OnTimeUp += CheckEnding;
    }

    private void OnDisable()
    {
        GameManager.OnTimeUp -= CheckEnding;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= radius)
        {
            isPlayerInside = true;
        }
        else
        {
            isPlayerInside = false;
            everLeft = true;
        }
    }

    void CheckEnding()
    {
        if (GameManager.Instance.currentEnding != GameManager.EndingType.None)
            return;

        if (isPlayerInside)
        {
            Debug.Log("🏠 Stay Home Ending");
            GameManager.Instance.SetEnding(GameManager.EndingType.StayHome);
        }
        else
        {
            GameManager.Instance.SetEnding(GameManager.EndingType.LateAndPoor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
