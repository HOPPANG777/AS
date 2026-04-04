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

    public bool IsAllArrived()
    {
        foreach (var pair in _targets)
        {
            float dist = Vector3.Distance(pair.Key.Transform.position, pair.Value);
            if (dist > 0.05f)
                return false;
        }
        return true;
    }

    public void Clear()
    {
        _targets.Clear();
        _visualizer.Hide();
    }

    private Vector3 GetCenter(IReadOnlyList<IUnit> units)
    {
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < units.Count; i++)
            sum += units[i].Transform.position;

        return sum / units.Count;
    }
}