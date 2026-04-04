using System.Collections.Generic;
using UnityEngine;

public sealed class UnitRegistry : IUnitRegistry
{
    private readonly List<IUnit> _units = new();

    public IReadOnlyList<IUnit> Units => _units;

    public void Register(IUnit unit)
    {
        _units.Add(unit);
    }

    public void Unregister(IUnit unit)
    {
        _units.Remove(unit);
    }
}