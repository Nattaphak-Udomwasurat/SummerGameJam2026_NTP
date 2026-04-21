using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sexy_animation : MonoBehaviour
{
    public List<Sprite> AnimationSprite;  // [0] = idle, [1] = walk
    public float delayBefore = 0.3f;     // wait before changing to walk
    public float delayAfter = 0.3f;      // wait after changing back to idle
    private Vector3 lastPosition;
    private bool isAnimating = false;

    public SpriteRenderer LadySprite;
    // Start is called before the first frame update
    void Start()
    {
        LadySprite =  gameObject.GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = transform.position != lastPosition;

        if (isMoving)
        {
            if (!isAnimating)
                StartCoroutine(WalkAnimation());
        }

        lastPosition = transform.position;
    }

    IEnumerator WalkAnimation()
    {
        isAnimating = true;

        yield return new WaitForSeconds(delayBefore);  // wait before walk
        LadySprite.sprite = AnimationSprite[1];

        yield return new WaitForSeconds(delayAfter);   // wait after walk
        LadySprite.sprite = AnimationSprite[0];

        isAnimating = false;
    }
}

