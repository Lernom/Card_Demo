using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlourishCardContainer : CardContainerBase
{
    private RectTransform _containerRect;
    
    [SerializeField]
    private float MaxPadding = 100;

    [SerializeField]
    private float MaxOffset = 25;

    private void Awake()
    {
        _containerRect = GetComponent<RectTransform>();
    }
    protected override Pose GetPoseForId(int id, int total)
    {
        if (total > 2)
            total--;
        var rotationOffset = MaxOffset - (MaxOffset * 2 / total) * id;
        var padding = Mathf.Clamp(_containerRect.rect.width / total, 0, MaxPadding);
        var targetPosition = _containerRect.TransformPoint(new Vector3(padding * (id - total / 2),
            _containerRect.rect.center.y, 0) + Vector3.down * Mathf.Abs(rotationOffset));
        var targetRotation = Quaternion.Euler(0, 0, rotationOffset);
        return new Pose(targetPosition, targetRotation);
    }
    public override bool IsPointInsidePlayArea(Vector3 point) => _containerRect.rect.Contains(_containerRect.InverseTransformPoint(point));
}
