using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class CardBuilder : SingletonBehaviour<CardBuilder>
{
    private string _imageStorage = "https://picsum.photos/100/100";
    public IEnumerator RequestCard(UnityAction<Card> CardComplete = null)
    {
        var request = UnityWebRequestTexture.GetTexture(_imageStorage);
        yield return request.SendWebRequest();
        var newCard = GameObject.Instantiate(GlobalParams.Instance.CardPrefab);
        Texture2D cardImage = null;
        if (request.result == UnityWebRequest.Result.Success)
        {
            cardImage = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        newCard.SetCardVisuals("Card Name", "Card Description", cardImage);

        var stats = Vector3Int.CeilToInt(Random.insideUnitSphere * 5) + Vector3Int.one * 5;
        
        newCard.SetCardStats(stats);
        CardComplete.Invoke(newCard);
    }
}