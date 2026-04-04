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

    private Vector3 _lastMouseWorld; // 마지막 마우스 월드 위치

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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                IUnit unit = hit.collider.GetComponent<IUnit>();

                if (unit != null)
                {
                    _selection.SelectSingle(unit); // 단일 선택
                    return;
                }
            }

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
            Vector3 worldPos = GetMouseWorldPosition(); // 클릭 위치
            Vector3 dir = (worldPos - _lastMouseWorld); // 마우스 이동 방향

            if (dir.sqrMagnitude < 0.001f)
                dir = Vector3.forward; // 최소 방향 보정

            _command.IssueMoveCommand(_selection.SelectedUnits, worldPos, dir);

            _lastMouseWorld = worldPos; // 갱신
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
