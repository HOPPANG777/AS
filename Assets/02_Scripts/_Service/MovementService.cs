using System.Collections.Generic;
using UnityEngine;

public sealed class MovementService : IMovementService
{
    private readonly List<IUnit> _movingUnits = new();
    private readonly Dictionary<IUnit, Vector3> _targets = new();

    private readonly IPathVisualizer _visualizer;

    private readonly IFormationService _formation;
    private readonly List<Vector3> _positions = new(); // 포메이션 결과 저장

    public MovementService(IPathVisualizer visualizer, IFormationService formation)
    {
        _visualizer = visualizer;
        _formation = formation;
    }

    public void Register(IUnit unit)
    {
        if (!_movingUnits.Contains(unit))
            _movingUnits.Add(unit);
    }

    public void Unregister(IUnit unit)
    {
        _movingUnits.Remove(unit);
        _targets.Remove(unit);
    }

    public void SetDestination(IReadOnlyList<IUnit> units, Vector3 dest, Vector3 forward)
    {
        _targets.Clear(); // 기존 목표 제거

        _formation.CalculatePositions(units, dest, forward, _positions); // 포메이션 계산

        for (int i = 0; i < units.Count; i++)
        {
            _targets[units[i]] = _positions[i]; // 각 유닛 목표
            units[i].MoveTo(_positions[i]);
        }

        _visualizer.Show(GetCenter(units), dest);
    }

    public void Tick()
    {
        if (_targets.Count == 0) return;

        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (var pair in _targets)
        {
            center += pair.Key.Transform.position;
            count++;
        }

        center /= count;
        _visualizer.UpdateProgress(center);
    }

    // MovementService.cs 내부

    public bool IsAllArrived()
    {
        if (_targets.Count == 0) return false;

        foreach (var pair in _targets)
        {
            Vector3 unitPos = pair.Key.Transform.position;
            Vector3 targetPos = pair.Value;

            // [핵심] 높이(Y축) 때문에 거리가 멀게 계산되는 것을 막기 위해 둘 다 0으로 맞춥니다.
            unitPos.y = 0;
            targetPos.y = 0;

            // X, Z 축 평면 거리만 계산
            float dist = Vector3.Distance(unitPos, targetPos);

            // 넉넉하게 1.5f 이내로 들어오면 도착한 것으로 인정
            if (dist > 1.5f)
                return false;
        }
        return true;
    }

    public void Clear()
    {
        _targets.Clear();
        if (_visualizer != null)
        {
            _visualizer.Hide();
        }
    }

    private Vector3 GetCenter(IReadOnlyList<IUnit> units)
    {
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < units.Count; i++)
            sum += units[i].Transform.position;

        return sum / units.Count;
    }
}