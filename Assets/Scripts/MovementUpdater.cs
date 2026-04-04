using UnityEngine;

public sealed class MovementUpdater : MonoBehaviour
{
    private IMovementService _movement;

    public void Construct(IMovementService movement)
    {
        _movement = movement;
    }

    private void Update()
    {
        _movement.Tick();

        if (_movement.IsAllArrived())
        {
            _movement.Clear();
        }
    }
}