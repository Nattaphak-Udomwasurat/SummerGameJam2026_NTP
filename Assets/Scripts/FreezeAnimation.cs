using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeAnimation : MonoBehaviour
{
    public List<Sprite> AnimationSprite;  // [0] = idle, [1] = walk
    public float delayBefore = 0.3f;     // wait before changing to walk
    public float delayAfter = 0.3f;      // wait after changing back to idle
    private Vector3 lastPosition;
    public Freeze freeze;
    private bool isAnimating = false;

    public SpriteRenderer freezeSprite;
    // Start is called before the first frame update
    void Start()
    {
        freezeSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze.IsStunned && !isAnimating)
        {
            StartCoroutine(PlayFreezeAnimation());
        }
    }

    IEnumerator PlayFreezeAnimation()
    {
        isAnimating = true;

        for (int i = 0; i < 2; i++) // loop 2 รอบ
        {
            freezeSprite.sprite = AnimationSprite[1];
            yield return new WaitForSeconds(delayBefore);

            freezeSprite.sprite = AnimationSprite[0];
            yield return new WaitForSeconds(delayAfter);
        }

        isAnimating = false;
    }

}
