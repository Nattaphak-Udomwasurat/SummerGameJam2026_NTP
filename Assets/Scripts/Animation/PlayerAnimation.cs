using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public List<Sprite> AnimationSprite;  // [0] = idle, [1] = walk
    public SpriteRenderer PlayerSprite;
    public float delayBefore = 0.3f;     // wait before changing to walk
    public float delayAfter = 0.3f;      // wait after changing back to idle
    private Vector3 lastPosition;
    private bool isAnimating = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        bool isMoving = transform.position != lastPosition;

        if (isMoving)
        {
            if (transform.position.x < lastPosition.x)
                PlayerSprite.flipX = false;
            else if (transform.position.x > lastPosition.x)
                PlayerSprite.flipX = true;

            if (!isAnimating)
                StartCoroutine(WalkAnimation());
        }

        lastPosition = transform.position;
    }

    IEnumerator WalkAnimation()
    {
        isAnimating = true;

        yield return new WaitForSeconds(delayBefore);  // wait before walk
        PlayerSprite.sprite = AnimationSprite[1];

        yield return new WaitForSeconds(delayAfter);   // wait after walk
        PlayerSprite.sprite = AnimationSprite[0];

        isAnimating = false;
    }
}