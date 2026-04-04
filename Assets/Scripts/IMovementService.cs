using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface IMovementService
{
    void Register(IUnit unit);
    void Unregister(IUnit unit);
    void SetDestination(IReadOnlyList<IUnit> units, Vector3 dest);
    void Tick();
    bool IsAllArrived();
    void Clear();
}
