using System;
using UnityEngine;

public class ValueCounter : MonoBehaviour
{
    public int TargetValue { get { return _targetValue; } set { _targetValue = value; _inTween = true; } }

    public Action<int> OnTargetValueReached;

    public bool Animating => _inTween;

    [SerializeField]
    private TMPro.TextMeshProUGUI Text;

    private float _currentDelay;

    private int _currentValue, _targetValue;

    private bool _inTween;
    private void Update()
    {
        if (_currentValue != TargetValue)
        {
            _inTween = true;
            _currentDelay -= Time.deltaTime;
            if (_currentDelay < 0)
            {
                _currentDelay = GlobalParams.Instance.CardCounterDelay;
                _currentValue -= Math.Sign(_currentValue - TargetValue);
                Text.text = _currentValue.ToString();
            }
        }
        else
            if (_inTween)
        {
            _inTween = false;
            OnTargetValueReached?.Invoke(_currentValue);
        }
    }
}