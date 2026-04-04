using UnityEngine;

public interface IUnit
{
    Transform Transform { get; }

    void Construct(IUnitRegistry registry);

    // 이동 시작
    void MoveTo(Vector3 position);

    // NavMesh 이동용
    void SetDestination(Vector3 position); 

    // 현재 이동 중인지 여부
    bool IsMoving { get; }

    // 목표 위치까지 거리
    float RemainingDistance { get; }
}
