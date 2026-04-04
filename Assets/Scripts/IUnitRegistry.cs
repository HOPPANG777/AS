using System.Collections.Generic;
using UnityEngine;

public interface IUnitRegistry
{
    IReadOnlyList<IUnit> Units { get; }

    public void Register(IUnit unit);

    public void Unregister(IUnit unit);
}
