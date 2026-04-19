using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PHY_UI : MonoBehaviour
{
    [Header("Reference")]
    public PangHaamYard playerBlock;

    [Header("UI")]
    public Transform iconParent;
    public GameObject iconPrefab;

    [Header("Sprite")]
    public Sprite fullSprite;
    public Sprite emptySprite;

    private List<Image> icons = new List<Image>();

    void Start()
    {
        GenerateIcons(playerBlock.maxCharges);
        UpdateUI(playerBlock.currentCharges, playerBlock.maxCharges);
    }

    void OnEnable()
    {
        playerBlock.OnChargeChanged += UpdateUI;
    }

    void OnDisable()
    {
        playerBlock.OnChargeChanged -= UpdateUI;
    }

    void GenerateIcons(int max)
    {
        // ลบของเก่า
        foreach (Transform child in iconParent)
            Destroy(child.gameObject);

        icons.Clear();

        // สร้างใหม่ตาม maxCharges
        for (int i = 0; i < max; i++)
        {
            GameObject obj = Instantiate(iconPrefab, iconParent);
            Image img = obj.GetComponent<Image>();
            icons.Add(img);
        }
    }

    void UpdateUI(int current, int max)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].sprite = (i < current) ? fullSprite : emptySprite;
        }
    }
}
