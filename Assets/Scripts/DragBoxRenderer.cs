using UnityEngine;
using UnityEngine.UI;

public sealed class DragBoxRenderer : MonoBehaviour, IDragBoxRenderer
{
    [SerializeField] private RectTransform _box;

    private Vector2 _start;

    public void Begin(Vector2 start)
    {
        _start = start;
        _box.gameObject.SetActive(true);
        UpdateDrag(start);
    }

    public void UpdateDrag(Vector2 current)
    {
        Vector2 min = Vector2.Min(_start, current);
        Vector2 max = Vector2.Max(_start, current);

        _box.anchoredPosition = min;
        _box.sizeDelta = max - min;
    }

    public void End()
    {
        _box.gameObject.SetActive(false);
    }
}