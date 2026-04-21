using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kidanimation : MonoBehaviour
{
    public List<Sprite> AnimationSprite; 
    public SpriteRenderer KidSprite;
    public float delayBefore = 0.3f;     
    public float delayAfter = 0.3f;      
    
    private KidBehavior kidBehavior;
    private bool isAnimating = false;

    void Start()
    {
        KidSprite = gameObject.GetComponent<SpriteRenderer>();
        kidBehavior = gameObject.GetComponent<KidBehavior>();
    }

    void Update()
    {
        // isAnimating flag prevents coroutine from spamming every frame
        if (kidBehavior.hasAttacked && !isAnimating)
        {
            StartCoroutine(SplashAnimation());
        }
    }

    IEnumerator SplashAnimation()
    {
        isAnimating = true;

        yield return new WaitForSeconds(delayBefore);
        KidSprite.sprite = AnimationSprite[1];

        yield return new WaitForSeconds(delayAfter);
        KidSprite.sprite = AnimationSprite[0];

        // Reset so animation can trigger again next attack
        kidBehavior.hasAttacked = false;
        isAnimating = false;
    }
}

