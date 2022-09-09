using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private CardContainerBase _handContainer;

    private CardContainerBase[] _registeredContainers;

    public CardContainerBase PlayerHand => _handContainer;

    [HideInInspector]
    public GameState GameState = GameState.Init;

    private IEnumerator Start()
    {
        _registeredContainers = GetComponentsInChildren<CardContainerBase>();
        yield return new WaitForSeconds(1f);
        var amountOfCards = Random.Range(GlobalParams.Instance.MinAmountOfCards, GlobalParams.Instance.MaxAmountOfCards + 1);
        for (int i = 0; i < amountOfCards; i++)
        {
            yield return CardBuilder.Instance.RequestCard(CardComplete);           
        }
        GameState = GameState.Plan;
    }

    private void CardComplete(Card card)
    {
        card.RegisterToContainer(_handContainer);
    }

    public void CheckForOtherPlayAreas(Card c)
    {
        for (int i = 0; i < _registeredContainers.Length; i++)
        {
            if (_registeredContainers[i].IsPointInsidePlayArea(c.transform.position))
            {
                c.RegisterToContainer(_registeredContainers[i]);
                break;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(gameObject.scene.buildIndex);
    }
}

public enum GameState
{
    Init,
    Plan,
    Battle
}