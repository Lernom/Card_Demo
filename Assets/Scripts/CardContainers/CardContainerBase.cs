using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardContainerBase : MonoBehaviour
{
    protected List<Card> _registeredCards = new List<Card>();

    public int count => _registeredCards.Count;
    public Card this[int i] => _registeredCards[i]; 

    public void RegisterCard(Card c)
    {
        if (!_registeredCards.Contains(c))
        {
            _registeredCards.Add(c);
           RefreshCardsPositions();
        }
    }
    public void UnregisterCard(Card c)
    {
        if (_registeredCards.Remove(c))
            RefreshCardsPositions();
    }
    public abstract bool IsPointInsidePlayArea(Vector3 point);

    protected void RefreshCardsPositions()
    {
        var i = 0;
        foreach (var item in _registeredCards)
        {
            item.SetReturnPose(GetPoseForId(i++, _registeredCards.Count));
            item.ReturnToPosition();
        }
    }

    protected abstract Pose GetPoseForId(int id, int total);

}
