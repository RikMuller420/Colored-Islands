using TMPro;
using UnityEngine;

public class RectHeighWithTextSyncronizer : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float baseHeight = 80;

    private void Update()
    {
        Rect rect = _rectTransform.rect;
        rect.height = baseHeight + _text.preferredHeight;
        _rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
    }
}
