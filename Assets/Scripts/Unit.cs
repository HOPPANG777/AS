using UnityEditor.Experimental.GraphView;
using UnityEngine;

public sealed class Unit : MonoBehaviour, IUnit, ISelectable
{
    // ===== 인터페이스 구현 =====

    // 현재 Transform 반환 (외부에서 위치 접근용)
    public Transform Transform => transform;

    // 현재 이동 중인지 여부 반환
    public bool IsMoving => _isMoving;

    // 목표 지점까지 남은 거리 반환
    public float RemainingDistance => (_target - transform.position).magnitude;

    // ===== 설정값 =====

    [SerializeField] private float _speed = 5f;          // 이동 속도
    [SerializeField] private float _rotationSpeed = 10f; // 회전 속도
    [SerializeField] private float _stopDistance = 0.05f;// 도착 판정 거리

    [SerializeField] private GameObject _highlight;      // 선택 표시 오브젝트

    // ===== 내부 상태 =====

    private Vector3 _target;     // 이동 목표 위치
    private bool _isMoving;      // 이동 중 여부

    private IUnitRegistry _registry; // 유닛 등록 시스템 참조

    // ===== 초기화 =====

    // 외부에서 Registry 주입
    public void Construct(IUnitRegistry registry)
    {
        _registry = registry;

        // 유닛 등록
        _registry.Register(this);

        // 시작 시 선택 표시 꺼둠
        if (_highlight != null)
            _highlight.SetActive(false);
    }

    private void OnDestroy()
    {
        // 파괴 시 Registry에서 제거
        _registry?.Unregister(this);
    }

    // ===== 이동 로직 =====

    // 외부에서 이동 명령 호출
    public void MoveTo(Vector3 position)
    {
        _target = position; // 목표 위치 설정
        _isMoving = true;   // 이동 시작
    }

    private void Update()
    {
        if (!_isMoving) return; // 이동 중 아닐 경우 종료

        Vector3 current = transform.position; // 현재 위치
        Vector3 dir = _target - current;      // 목표 방향

        float dist = dir.magnitude;           // 남은 거리

        // 도착 판정
        if (dist <= _stopDistance)
        {
            transform.position = _target; // 정확히 위치 보정
            _isMoving = false;            // 이동 종료
            return;
        }

        // 이동 처리
        Vector3 move = dir.normalized * _speed * Time.deltaTime;
        transform.position = current + move;

        // 회전 처리 (이동 방향 바라보기)
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                _rotationSpeed * Time.deltaTime
            );
        }
    }

    // ===== 선택 처리 =====

    // 선택 시 호출
    public void OnSelect()
    {
        if (_highlight != null)
            _highlight.SetActive(true);
    }

    // 선택 해제 시 호출
    public void OnDeselect()
    {
        if (_highlight != null)
            _highlight.SetActive(false);
    }
}