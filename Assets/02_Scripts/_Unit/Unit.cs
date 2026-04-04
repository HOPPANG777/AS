using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private GameObject _highlight;      // 선택 표시 오브젝트

    // ===== 내부 상태 =====

    private Vector3 _target;     // 이동 목표 위치
    private bool _isMoving;      // 이동 중 여부

   
    private NavMeshAgent _agent;  // NavMesh 이동 컴포넌트
    private ISeparationService _separation;
    private IUnitRegistry _registry; // 유닛 등록 시스템 참조

    // ===== 초기화 =====

    // 외부에서 Registry 주입
    public void Construct(IUnitRegistry registry)
    {
        _registry = registry;

        // 유닛 등록
        _registry.Register(this);

        // NavMeshAgent 캐싱
        _agent = GetComponent<NavMeshAgent>(); 

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

    private void Update()
    {
        if (!_agent.hasPath) return;

        Vector3 force = _separation.CalculateSeparation(this, _registry.Units);

        if (force.sqrMagnitude > 0.0001f)
        {
            _agent.Move(force * Time.deltaTime); // NavMesh 위에서 보정 이동
        }
    }

    public void InjectSeparation(ISeparationService separation, IUnitRegistry registry)
    {
        _separation = separation;
        _registry = registry;
    }

    // 외부에서 이동 명령 호출
    public void MoveTo(Vector3 position)
    {
        SetDestination(position); // 내부 통일
    }

    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position); // NavMesh 이동
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