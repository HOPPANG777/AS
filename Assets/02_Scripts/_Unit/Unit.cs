using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public sealed class Unit : MonoBehaviour, IUnit, ISelectable
{
    // ===== 인터페이스 구현 =====

    public Transform Transform => transform;
    public bool IsMoving => _isMoving;
    public float RemainingDistance => (_target - transform.position).magnitude;

    // ===== 유닛 스탯 =====

    public int hp = 56;
    public int maxHp = 100;
    public int troops = 5849;
    public int atk = 267;
    public int def = 86;
    public int spd = 8;

    // ===== 설정값 =====
    [SerializeField] private GameObject _highlight;      // 선택 표시 오브젝트

    [Header("Audio")]
    [SerializeField] private AudioClip _selectSound;     // 선택 효과음
    [SerializeField] private AudioClip _moveSound;       // 이동(발소리) 효과음
    private AudioSource _audioSource;                    // 유닛의 스피커 역할

    // ===== 내부 상태 =====

    private Vector3 _target;     // 이동 목표 위치
    private bool _isMoving;      // 이동 중 여부

    private NavMeshAgent _agent;  // NavMesh 이동 컴포넌트
    private ISeparationService _separation;
    private IUnitRegistry _registry; // 유닛 등록 시스템 참조

    // ===== 초기화 =====

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Construct(IUnitRegistry registry)
    {
        _registry = registry;
        _registry.Register(this);
        _agent = GetComponent<NavMeshAgent>();

        if (_highlight != null)
            _highlight.SetActive(false);
    }

    private void OnDestroy()
    {
        _registry?.Unregister(this);
    }

    // ===== 이동 로직 =====

    private void Update()
    {
        if (_agent == null || _separation == null || _registry == null) return;

        // 이동 효과음 처리 로직
        // 유닛이 목적지를 향해 걸어가고 있다면?
        if (_agent.hasPath && _agent.remainingDistance > 0.1f)
        {
            if (_moveSound != null)
            {
                // 발소리가 안 나고 있다면 무한 반복(Loop)으로 재생
                if (_audioSource.clip != _moveSound || !_audioSource.isPlaying)
                {
                    _audioSource.clip = _moveSound;
                    _audioSource.loop = true;
                    _audioSource.Play();
                }
            }
        }
        else
        {
            // 재생 중이던 발소리를 끔
            if (_audioSource.clip == _moveSound && _audioSource.isPlaying)
            {
                _audioSource.Stop();
                _audioSource.clip = null; // 다른 소리를 위해 비워둠
            }
        }

        // 목적지가 없으면 아래의 이동 계산은 건너뜀
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

    public void MoveTo(Vector3 position)
    {
        SetDestination(position); // 내부 통일
    }

    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position); // NavMesh 이동
    }

    // ===== 선택 처리 =====

    public void OnSelect()
    {
        if (_highlight != null) _highlight.SetActive(true);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowUnitInfo(hp, maxHp, troops, atk, def, spd);
        }

        if (_audioSource != null && _selectSound != null)
        {
            _audioSource.PlayOneShot(_selectSound);
        }
    }

    public void OnDeselect()
    {
        if (_highlight != null) _highlight.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideUnitInfo();
        }
    }
}