using System.Collections.Generic;
using UnityEngine;

public interface ICommandService
{
    // 이동 명령 + 방향 전달
    void IssueMoveCommand(IReadOnlyList<IUnit> units, Vector3 destination, Vector3 forward);
}