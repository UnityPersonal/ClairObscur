using UnityEngine;

public static class UIUtils
{
    // 월드 포지션을 캔버스 로컬 포지션으로 변환
    public static Vector2 WorldToCanvasPosition(Canvas canvas, Vector3 worldPosition, Camera camera = null)
    {
        if (camera == null)
            camera = Camera.main;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldPosition);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out localPoint);

        return localPoint;
    }
}