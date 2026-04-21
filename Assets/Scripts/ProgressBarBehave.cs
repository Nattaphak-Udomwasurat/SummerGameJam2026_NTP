using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarBehave : MonoBehaviour
{
    public Slider Slider;
    public float distance;
    public float maxDistance; // Set this to your max expected distance
    public GameObject workplace, Player;

    void Start()
    {
        workplace = GameObject.Find("Workplace");
        Player = GameObject.Find("Player");
        distance = Vector2.Distance(Player.transform.position, workplace.transform.position);
        maxDistance = distance;

        Slider.minValue = 0f;
        Slider.maxValue = 1f;
    }

    void Update()
    {
        if (workplace == null || Player == null) return;
        distance = Vector2.Distance(Player.transform.position, workplace.transform.position);
        // Convert distance to 0-1 range (1 = close, 0 = far)
        Slider.value = 1f - Mathf.Clamp01(distance / maxDistance);
    }


}