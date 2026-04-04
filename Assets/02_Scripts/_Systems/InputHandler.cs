using UnityEngine;

public sealed class InputHandler : MonoBehaviour
{
    private ISelectionService _selection;
    private ICommandService _command;

    private Vector2 _dragStart;
    private bool _isDragging;

    // 드래그 이동 미리보기용 변수
    private bool _isMoveTargeting;
    private IPathVisualizer _visualizer;

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

        // 씬에서 PathVisualizer(미리보기 선과 원)를 찾아옵니다.
        _visualizer = Object.FindAnyObjectByType<PathVisualizer>();
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // ==========================================
        // 1. 유닛 선택 및 드래그 (우클릭: 1)
        // ==========================================
        if (Input.GetMouseButtonDown(1))
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

            // 빈 땅 우클릭 시 선택 드래그 상자 시작
            _dragStart = Input.mousePosition;
            _isDragging = true;
            _dragBox.Begin(_dragStart);
        }

        if (Input.GetMouseButton(1) && _isDragging)
        {
            _dragBox.UpdateDrag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_isDragging)
            {
                Rect rect = GetScreenRect(_dragStart, Input.mousePosition);
                _selection.Select(rect);
            }

            _dragBox.End();
            _isDragging = false;
        }

        // ==========================================
        // 2. 이동 버튼 누른 후 '드래그 이동' (좌클릭: 0)
        // ==========================================

        // UI의 이동 버튼이 눌려서 '이동 대기 상태'일 때만 작동합니다.
        if (UIManager.Instance != null && UIManager.Instance.isMoveTargetingMode)
        {
            // [드래그 시작] 좌클릭을 누르는 순간
            if (Input.GetMouseButtonDown(0))
            {
                if (_selection.SelectedUnits.Count > 0)
                {
                    _isMoveTargeting = true;
                    _lastMouseWorld = GetMouseWorldPosition(); // 시작점 기억
                }
            }

            // [드래그 중] 좌클릭을 누른 채로 마우스를 움직일 때 (경로 미리보기)
            if (Input.GetMouseButton(0) && _isMoveTargeting)
            {
                Vector3 currentMouseWorld = GetMouseWorldPosition();
                Vector3 center = GetSelectedUnitsCenter();

                // 팀원의 PathVisualizer를 이용해 선과 원을 마우스 위치에 미리 그려줍니다.
                if (_visualizer != null)
                {
                    _visualizer.Show(center, currentMouseWorld);
                }
            }

            // [드래그 끝] 좌클릭을 떼는 순간 (실제 이동 명령)
            if (Input.GetMouseButtonUp(0))
            {
                if (_isMoveTargeting)
                {
                    Vector3 worldPos = GetMouseWorldPosition(); // 최종 마우스 위치 (도착지)
                    Vector3 dir = (worldPos - _lastMouseWorld); // 드래그한 방향 (포메이션 방향)

                    if (dir.sqrMagnitude < 0.001f)
                        dir = Vector3.forward;

                    // 팀원의 시스템으로 진짜 이동 명령 하달!
                    _command.IssueMoveCommand(_selection.SelectedUnits, worldPos, dir);

                    // 드래그 및 이동 대기 상태 종료
                    _isMoveTargeting = false;
                    UIManager.Instance.isMoveTargetingMode = false;
                }
            }
        }
    }

    // 선택된 유닛들의 뭉쳐있는 중심 위치를 계산하는 도우미 함수
    private Vector3 GetSelectedUnitsCenter()
    {
        if (_selection.SelectedUnits.Count == 0) return Vector3.zero;

        Vector3 center = Vector3.zero;
        for (int i = 0; i < _selection.SelectedUnits.Count; i++)
        {
            center += _selection.SelectedUnits[i].Transform.position;
        }
        return center / _selection.SelectedUnits.Count;
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