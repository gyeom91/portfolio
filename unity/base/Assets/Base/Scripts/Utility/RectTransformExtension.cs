using UnityEngine;

public static class RectTransformExtension
{
    public static void SafeArea(this RectTransform rectTransform)
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    public static bool IsOverlapMarker(this RectTransform owner, RectTransform target)
    {
        Vector2 ownerSize = owner.rect.size;
        Vector2 ownerPosition = owner.position;
        var ownerRect = new Rect(ownerPosition.x - ownerSize.x / 2, ownerPosition.y - ownerSize.y / 2, ownerSize.x, ownerSize.y);

        Vector2 targetSize = target.rect.size;
        Vector2 targetPosition = target.position;
        var targetRect = new Rect(targetPosition.x - targetSize.x / 2, targetPosition.y - targetSize.y / 2, targetSize.x, targetSize.y);

        return ownerRect.Overlaps(targetRect);
    }

    public static bool IsStretched(this RectTransform rt)
    {
        return Mathf.Approximately(rt.anchorMin.x, 0f) &&
               Mathf.Approximately(rt.anchorMin.y, 0f) &&
               Mathf.Approximately(rt.anchorMax.x, 1f) &&
               Mathf.Approximately(rt.anchorMax.y, 1f);
    }
}
