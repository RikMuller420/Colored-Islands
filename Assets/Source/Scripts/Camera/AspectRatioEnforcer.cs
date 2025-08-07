using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private RectTransform _mainUi;
    [SerializeField] private RectTransform _letterboxLeft;
    [SerializeField] private RectTransform _letterboxRight;

    private float maxAspectX = 16f;
    private float maxAspectY = 9f;
    private float maxAspect;

    private void Start()
    {
        maxAspect = maxAspectX / maxAspectY;
        UpdateViewport(new Vector2(Screen.width, Screen.height));
    }

    private void OnEnable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged += UpdateViewport;
    }

    private void OnDisable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged -= UpdateViewport;
    }

    void UpdateViewport(Vector2 screenSize)
    {
        float screenAspect = screenSize.x / screenSize.y;

        if (screenAspect > maxAspect)
        {
            float scaleWidth = maxAspect / screenAspect;
            float offsetLeft = (1f - scaleWidth) / 2f;
            float offsetRight = 1f - offsetLeft;

            FitInAnchors(_mainUi, new Vector2(offsetLeft, offsetRight));
            FitInAnchors(_letterboxLeft, new Vector2(0, offsetLeft));
            FitInAnchors(_letterboxRight, new Vector2(offsetRight, 1));
        }
        else
        {
            FitInAnchors(_mainUi, new Vector2(0, 1));
            FitInAnchors(_letterboxLeft, new Vector2(0, 0));
            FitInAnchors(_letterboxRight, new Vector2(1, 1));
        }
    }

    private void FitInAnchors(RectTransform rect, Vector2 horizontalAnchor)
    {
        rect.anchorMin = new Vector2(horizontalAnchor.x, 0f);
        rect.anchorMax = new Vector2(horizontalAnchor.y, 1f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
