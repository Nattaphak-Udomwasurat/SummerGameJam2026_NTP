using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum PlayerState
    {
        Normal,
        Block,
        Drink,
        Throw,
        Wet
    }

    [SerializeField] private PangHaamYard pangHaamYard;
    [SerializeField] private Drinking drinking;
    [SerializeField] private Throwing throwing;
    [SerializeField] private PlayerWet playerTakeWet;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerState currentState = PlayerState.Normal;

    private Coroutine wetRoutine;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite pangHaamYardSprite;
    public Sprite drinkingSprite;
    public Sprite throwingSprite;

    private void OnEnable()
    {
        pangHaamYard.OnBlockStart += HandlePangHaamYardStart;
        pangHaamYard.OnBlockEnd += HandlePangHaamYardEnd;

        drinking.OnDrinkStart += HandleDrinkStart;
        drinking.OnDrinkEnd += HandleDrinkEnd;

        throwing.OnThrowStart += HandleThrowStart;
        throwing.OnThrowEnd += HandleThrowEnd;
    }

    private void OnDisable()
    {
        pangHaamYard.OnBlockStart -= HandlePangHaamYardStart;
        pangHaamYard.OnBlockEnd -= HandlePangHaamYardEnd;

        drinking.OnDrinkStart += HandleDrinkStart;
        drinking.OnDrinkEnd += HandleDrinkEnd;

        throwing.OnThrowStart += HandleThrowStart;
        throwing.OnThrowEnd += HandleThrowEnd;
    }

    public bool IsBusy()
    {
        return currentState == PlayerState.Block ||
               currentState == PlayerState.Drink ||
               currentState == PlayerState.Throw ||
               currentState == PlayerState.Wet;
    }


    private void HandlePangHaamYardStart()
    {
        if (currentState != PlayerState.Normal) return;

        currentState = PlayerState.Block;
        spriteRenderer.sprite = pangHaamYardSprite;
    }

    private void HandlePangHaamYardEnd()
    {
        if (currentState != PlayerState.Block) return;

        currentState = PlayerState.Normal;
        spriteRenderer.sprite = normalSprite;
    }

    private void HandleDrinkStart()
    {
        if (currentState != PlayerState.Normal) return;

        currentState = PlayerState.Drink;
        spriteRenderer.sprite = drinkingSprite;
    }

    private void HandleDrinkEnd()
    {
        if (currentState != PlayerState.Drink) return;

        currentState = PlayerState.Normal;
        spriteRenderer.sprite = normalSprite;
    }

    private void HandleThrowStart()
    {
        if (currentState != PlayerState.Normal) return;

        currentState = PlayerState.Throw;
        spriteRenderer.sprite = throwingSprite;
    }

    private void HandleThrowEnd()
    {
        if (currentState != PlayerState.Throw) return;

        currentState = PlayerState.Normal;
        spriteRenderer.sprite = normalSprite;
    }
}
