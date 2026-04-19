using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public List<Sprite> AnimationSprite;  // [0] = idle, [1] = walk
    public SpriteRenderer PlayerSprite;
    public float delay = 0.3f;
    private Vector3 lastPosition;
    private float timer = 0f;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        bool isMoving = transform.position != lastPosition;

        if (isMoving)
        {
            timer += Time.deltaTime;
  
            if (timer > delay)
            {
                PlayerSprite.sprite = AnimationSprite[1];
                Debug.Log("Sprite1");
            }

            // flip based on horizontal direction
            if (transform.position.x < lastPosition.x)
            {
                PlayerSprite.flipX = false;   // moving left
            }
            else if (transform.position.x > lastPosition.x)
            {
                PlayerSprite.flipX = true;  // moving right
            }
        }
        else
        {
            timer = 0f;
            PlayerSprite.sprite = AnimationSprite[0];
            Debug.Log("Sprite0");
        }

        lastPosition = transform.position;
    }
}
    
