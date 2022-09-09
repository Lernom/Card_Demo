using UnityEngine;

public class HorizontalCardContainer : CardContainerBase
{
    private RectTransform _containerRect;

    public float Padding = 100;

    private void Awake()
    {
        _containerRect = GetComponent<RectTransform>();
    }
    protected override Pose GetPoseForId(int id, int total)
    {
        return new Pose()
        {
            position = _containerRect.TransformPoint(Padding * (id - total / 2),
            _containerRect.rect.center.y, 0),
            rotation = transform.rotation
        };
    }
    public override bool IsPointInsidePlayArea(Vector3 point) => _containerRect.rect.Contains(_containerRect.InverseTransformPoint(point));
}
