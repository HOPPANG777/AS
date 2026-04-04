using System.Collections.Generic;
using UnityEngine;

public interface ICommandService
{
    void IssueMoveCommand(IReadOnlyList<IUnit> units, Vector3 destination);
}
