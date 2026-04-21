using UnityEngine;
using UnityEngine.UI;

public class GalleryUI : MonoBehaviour
{
    [System.Serializable]
    public struct EndingSlot
    {
        public GameManager.EndingType endingType;
        public Image displayImage;   // Image component ของแต่ละช่อง
        public Sprite endingSprite;  // ภาพ Ending จริง
    }

    [SerializeField] private EndingSlot[] slots;
    [SerializeField] private Sprite lockedSprite; // ภาพดำ

    // เรียกตอนเปิด Panel Gallery
    public void RefreshGallery()
    {
        foreach (var slot in slots)
        {
            bool unlocked = GameManager.Instance.IsEndingUnlocked(slot.endingType);
            slot.displayImage.sprite = unlocked ? slot.endingSprite : lockedSprite;
        }
    }

    private void OnEnable()
    {
        // Auto refresh ทุกครั้งที่เปิด Panel
        if (GameManager.Instance != null)
            RefreshGallery();
    }
}