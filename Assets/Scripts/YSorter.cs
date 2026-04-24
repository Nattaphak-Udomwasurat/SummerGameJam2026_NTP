using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset = 0;
    [SerializeField] private bool isBG = false;
    [SerializeField] private int bgSortingOrder = -9999;

    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        if (isBG)
        {
            spriteRenderer.sortingOrder = bgSortingOrder;
            return;
        }


        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 1000) + offset;
    }
}
