using UnityEngine;

public class UILimitRectTransform : UI
{
    [SerializeField] private RectTransform _limitRectTransform = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="camera">Canvas Render Mode is Overlay.IsNull()</param>
    public void MoveToDirection(Vector2 direction, Camera camera = null)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _limitRectTransform,
            direction,
            camera,
            out var localPoint
        );

        rectTransform.localPosition = Limit(localPoint);
    }

    private Vector2 Limit(Vector2 direction)
    {
        Vector2 limitSize = _limitRectTransform.rect.size;
        Vector2 uiSize = rectTransform.rect.size;

        direction += new Vector2(
            Mathf.Abs(0.5f - rectTransform.pivot.x) * rectTransform.rect.width,
            Mathf.Abs(0.5f - rectTransform.pivot.y) * rectTransform.rect.height
        );

        Vector2 pivotOffset = new Vector2(
            uiSize.x * rectTransform.pivot.x,
            uiSize.y * rectTransform.pivot.y
        );

        float minX = -limitSize.x / 2 + pivotOffset.x;
        float maxX = limitSize.x / 2 - (uiSize.x - pivotOffset.x);
        float minY = -limitSize.y / 2 + pivotOffset.y;
        float maxY = limitSize.y / 2 - (uiSize.y - pivotOffset.y);

        return new Vector2(
            Mathf.Clamp(direction.x, minX, maxX),
            Mathf.Clamp(direction.y, minY, maxY)
        );
    }
}
