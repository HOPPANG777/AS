using UnityEngine;

public sealed class GameBootstrap : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Scene References")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private MovementUpdater _updater;
    [SerializeField] private PathVisualizer _visualizer;
    [SerializeField] private DragBoxRenderer _dragBox;

    [Header("Units")]
    [SerializeField] private IUnit[] _units;

    private void Awake()
    {
        var registry = new UnitRegistry();
        var formation = new GridFormationService(); // 포메이션 생성
        var movement = new MovementService(_visualizer, formation);
        var selection = new SelectionService(registry, _camera);
        var command = new CommandService(movement);

        _input.Construct(selection, command, _dragBox, _camera, _groundLayer);
        _updater.Construct(movement);

        for (int i = 0; i < _units.Length; i++)
        {
            _units[i].Construct(registry);
            movement.Register(_units[i]);
        }
    }
}
