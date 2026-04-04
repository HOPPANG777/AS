using System.Collections.Generic;
using UnityEngine;

public sealed class CommandService : ICommandService
{
    private readonly IMovementService _movement;

    public CommandService(IMovementService movement)
    {
        _movement = movement;
    }

    public void IssueMoveCommand(IReadOnlyList<IUnit> units, Vector3 destination)
    {
        if (units.Count == 0) return;

        _movement.SetDestination(units, destination);
    }
}
