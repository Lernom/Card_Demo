using System.Collections;
using UnityEngine;

public class BattleTurn : MonoBehaviour
{
    private Coroutine _turn;
    public void StartTurn()
    {
        if(_turn == null && GameManager.Instance.GameState == GameState.Plan)
        {
            _turn = StartCoroutine(TakeTurn());
        }
    }

    private IEnumerator TakeTurn()
    {
        GameManager.Instance.GameState = GameState.Battle;
        var hand = GameManager.Instance.PlayerHand;
        while (hand.count > 0)
        {
            var statToChange = Random.Range(0, 3);
            for (int i = 0; i < hand.count; i++)
            {
                var card = hand[i];
                var id = card.transform.GetSiblingIndex();
                card.transform.SetAsLastSibling();
                yield return card.ChangeCardStat(statToChange, Random.Range(-2, 9));
                if (card.PendingForDestruction)
                    i--;
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    card.transform.SetSiblingIndex(id);
                }
            }
        }
        _turn = null;
        GameManager.Instance.GameState = GameState.Plan;
    }
}