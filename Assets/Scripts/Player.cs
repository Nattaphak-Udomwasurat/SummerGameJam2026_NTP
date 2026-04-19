using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PangHaamYard pangHaamYard;
    [SerializeField] private Drinking drinking;
    [SerializeField] private Throwing throwing;
    [SerializeField] private PlayerWet playerTakeWet;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Coroutine wetRoutine;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite pangHaamYardSprite;
    public Sprite wetSprite;
    public Sprite drinkingSprite;
    public Sprite throwingSprite;


    private void OnEnable()
    {
        pangHaamYard.OnBlockStart += HandlePangHaamYardStart;
        pangHaamYard.OnBlockEnd += HandleNormal;
        playerTakeWet.OnTakeWet += HandleTakeWet;

        drinking.OnDrinkStart += HandleDrinkStart;
        drinking.OnDrinkEnd += HandleNormal;

        throwing.OnThrowStart += HandleThrowStart;
        throwing.OnThrowEnd += HandleNormal;
    }

    private void OnDisable()
    {
        pangHaamYard.OnBlockStart -= HandlePangHaamYardStart;
        pangHaamYard.OnBlockEnd -= HandleNormal;
        playerTakeWet.OnTakeWet -= HandleTakeWet;

        drinking.OnDrinkStart -= HandleDrinkStart;
        drinking.OnDrinkEnd -= HandleNormal;

        throwing.OnThrowStart -= HandleThrowStart;
        throwing.OnThrowEnd -= HandleNormal;
    }

    private void HandlePangHaamYardStart()
    {
        spriteRenderer.sprite = pangHaamYardSprite;
    }

    private void HandleNormal()
    {
        spriteRenderer.sprite = normalSprite;
    }

    private void HandleTakeWet()
    {
        if (wetRoutine != null)
            StopCoroutine(wetRoutine);

        wetRoutine = StartCoroutine(WetFlash());
    }

    private void HandleDrinkStart()
    {
        spriteRenderer.sprite = drinkingSprite;
    }

    private void HandleThrowStart()
    {
        spriteRenderer.sprite = throwingSprite;
    }

    private IEnumerator WetFlash()
    {
        spriteRenderer.sprite = wetSprite;

        yield return new WaitForSeconds(0.8f); // ปรับเวลาได้

        spriteRenderer.sprite = normalSprite;
    }
}
