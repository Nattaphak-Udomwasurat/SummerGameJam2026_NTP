using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public List<Sprite> AnimationSprite;  // [0] = idle, [1] = walk
    public SpriteRenderer PlayerSprite;
    public float delayBefore = 0.5f;     // wait before changing to walk
    public float delayAfter = 0.5f;
    public float timeDelay = 1f;// wait after changing back to idle
    private Vector3 lastPosition;
    private bool isAnimating = false;
    private bool isIdle = false;
    [SerializeField]  private Freeze freeze;
    private Coroutine walkRoutine;

    [SerializeField] private List<Sprite> idleSprites;
    [SerializeField] private float idleDelay = 0.5f;

    private Coroutine idleRoutine;

    [SerializeField] private AudioClip moveSFX;
    [SerializeField] private Player player;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player != null && player.IsBusy())
        {
            StopAllAnimations();
            return;
        }

        Vector3 delta = transform.position - lastPosition;
        bool isMoving = delta.sqrMagnitude > 0.0001f;

        //  Flip ตอนเดิน
        if (isMoving)
        {
            if (delta.x < 0)
                PlayerSprite.flipX = false;
            else if (delta.x > 0)
                PlayerSprite.flipX = true;


            if (!isAnimating)
            {
                walkRoutine = StartCoroutine(WalkAnimation());
            }
                

        }
        else if ( isMoving == false)
        {
            
        }
        {
           /* if (isAnimating)
            {
                StopCoroutine(walkRoutine);
            }*/
            
            if (!freeze.IsStunned && !isIdle) // ไม่โดน Stun และยังไม่ได้ Idle
            {
                if (walkRoutine != null)
                {
                    walkRoutine = null;
                    isAnimating = false;
                }
                idleRoutine = StartCoroutine(IdleAnimation());
            }

            //  ถ้าโดน Stun → หยุด Idle ด้วย
            if (freeze.IsStunned && isIdle)
            {
                if (idleRoutine != null)
                {
                    StopCoroutine(idleRoutine);
                    idleRoutine = null;
                    isIdle = false;
                }
            }

        }

        lastPosition = transform.position;
        Debug.Log(isMoving);
    }

    IEnumerator WalkAnimation()
    {
        isAnimating = true;

        while (true)
        {
            if (AudioManager.Instance != null && moveSFX != null)
                AudioManager.Instance.PlaySFX(moveSFX);

            PlayerSprite.sprite = AnimationSprite[0];
            yield return new WaitForSeconds(delayBefore);

            PlayerSprite.sprite = AnimationSprite[1];
            yield return new WaitForSeconds(delayAfter);
        }
    }

    IEnumerator IdleAnimation()
    {
        isIdle = true;
        int index = 0;

        while (true)
        {
            PlayerSprite.sprite = idleSprites[index];
            index = (index + 1) % idleSprites.Count;

            yield return new WaitForSeconds(idleDelay);
        }
    }

    IEnumerator CheckAnimation()
    {
        yield return new WaitForSeconds(timeDelay);
        isIdle = true;
    }

    void StopAllAnimations()
    {
        if (walkRoutine != null) { StopCoroutine(walkRoutine); walkRoutine = null; isAnimating = false; }
        if (idleRoutine != null) { StopCoroutine(idleRoutine); idleRoutine = null; isIdle = false; }
    }
}