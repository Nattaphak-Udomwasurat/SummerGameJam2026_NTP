using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset = 0;

    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        // 🔥 แกนหลักของระบบ
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100) + offset;
    }
}
