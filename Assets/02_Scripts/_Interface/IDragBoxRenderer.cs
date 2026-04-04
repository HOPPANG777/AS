using UnityEngine;

public interface IDragBoxRenderer
{
    void Begin(Vector2 start);
    void UpdateDrag(Vector2 current);
    void End();
}
