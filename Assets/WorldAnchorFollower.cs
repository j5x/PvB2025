using UnityEngine;

public class WorldAnchorFollower : MonoBehaviour
{
    public RectTransform uiAnchor;
    public Canvas worldCanvas; // The one your game is rendering to

    void LateUpdate()
    {
        if (uiAnchor == null || worldCanvas == null) return;

        Vector3 worldPos;
        Vector2 screenPoint = RectTransformUtility.CalculateRelativeRectTransformBounds(worldCanvas.transform).center;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            worldCanvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(null, uiAnchor.position),
            null,
            out worldPos);

        transform.position = worldPos;
    }
}