using UnityEngine;

public sealed class InputHandler : MonoBehaviour
{
    private ISelectionService _selection;
    private ICommandService _command;

    private Vector2 _dragStart;
    private bool _isDragging;

    private Camera _camera;
    private LayerMask _groundLayer;

    private IDragBoxRenderer _dragBox;

    public void Construct(
        ISelectionService selection,
        ICommandService command,
        IDragBoxRenderer dragBox,
        Camera camera,
        LayerMask groundLayer
    )
    {
        _selection = selection;
        _command = command;
        _dragBox = dragBox;
        _camera = camera;
        _groundLayer = groundLayer;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragStart = Input.mousePosition;
            _isDragging = true;
            _dragBox.Begin(_dragStart);
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            _dragBox.UpdateDrag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isDragging)
            {
                Rect rect = GetScreenRect(_dragStart, Input.mousePosition);
                _selection.Select(rect);
            }

            _dragBox.End();
            _isDragging = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = GetMouseWorldPosition();
            _command.IssueMoveCommand(_selection.SelectedUnits, worldPos);
        }
    }

    private Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        Vector2 min = Vector2.Min(start, end);
        Vector2 max = Vector2.Max(start, end);
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _groundLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
