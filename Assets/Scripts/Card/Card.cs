using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private ValueCounter[] _values;

    [SerializeField]
    private TMPro.TextMeshProUGUI _cardTitle;

    [SerializeField]
    private TMPro.TextMeshProUGUI _cardDescription;

    [SerializeField]
    private RawImage _cardImage;

    private CardState _state;

    private Pose _returnPose;

    private CardContainerBase _currentContainer;

    public bool PendingForDestruction {get; private set;}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_state == CardState.Resting && GameManager.Instance.GameState == GameState.Plan)
        {
            _state = CardState.Dragging;
            _animator.SetBool("Grabbed", true);
            transform.SetParent(GlobalParams.Instance.TransferTransform, true);
            transform.rotation = Quaternion.identity;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_state == CardState.Dragging)
            transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_state == CardState.Dragging)
        {
            GameManager.Instance.CheckForOtherPlayAreas(this);
            ReturnToPosition();
        }
    }

    public void ReturnToPosition()
    {
        _state = CardState.Returning;
        if (_currentContainer != null)
            transform.SetParent(_currentContainer.transform, true);
        Sequence returnSequence = DOTween.Sequence();
        returnSequence.Append(transform.DOMove(_returnPose.position, GlobalParams.Instance.CardReturnTweenDuration, true));
        returnSequence.Append(transform.DORotate(_returnPose.rotation.eulerAngles, GlobalParams.Instance.CardReturnTweenDuration));
        returnSequence.SetEase(GlobalParams.Instance.CardEasingType);
        returnSequence.onComplete += () => { _state = CardState.Resting; };
        returnSequence.Play();
        _animator.SetBool("Grabbed", false);
    }
    public void SetReturnPose(Pose pose)
    {
        _returnPose = pose;
    }
    public void RegisterToContainer(CardContainerBase newContainer)
    {
        if (_currentContainer != null)
        {
            _currentContainer.UnregisterCard(this);
        }
        _currentContainer = newContainer;
        _currentContainer.RegisterCard(this);
        transform.SetParent(_currentContainer.transform, true);
    }

    public void SetCardVisuals(string title, string description, Texture2D image)
    {
        _cardTitle.text = title;
        _cardDescription.text = description;
        if (image != null)
            _cardImage.texture = image;
    }

    public void SetCardStats(Vector3Int stats)
    {
        for (int i = 0; i < 3; i++)
        {
            _values[i].TargetValue = stats[i];
        }
    }

    public IEnumerator ChangeCardStat(int statId, int newValue)
    {
        _values[statId].TargetValue = newValue;
        while (_values[statId].Animating)
            yield return null;
    }

    private void Start()
    {
        //MANA, HP, POWER
        _values[1].OnTargetValueReached += CheckHealthValue;
    }

    private void CheckHealthValue(int newValue)
    {
        if (newValue < 1)
        {
            PendingForDestruction = true; 
            if (_currentContainer != null)
            {
                _currentContainer.UnregisterCard(this);
                Destroy(gameObject, 0.3f);
            }
        }
    }
}

public enum CardState
{
    Resting,
    Dragging,
    Returning,
    Active
}