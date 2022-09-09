using DG.Tweening;
using UnityEngine;

public class GlobalParams : SingletonBehaviour<GlobalParams>
{
    public float CardReturnTweenDuration = 0.75f;

    public float CardBounceDuration = 0.3f;

    public float CardBouncePower = 0.4f;

    public float CardCounterDelay = 0.1f;

    public Card CardPrefab;

    public Transform TransferTransform;

    public Ease CardEasingType;

    public int MinAmountOfCards = 3;

    public int MaxAmountOfCards = 9;

}
