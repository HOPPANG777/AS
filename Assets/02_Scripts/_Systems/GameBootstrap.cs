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

        //SeparationService 생성 코드를 추가했습니다.[주현씨 수정]
        var separation = new SeparationService();

        _input.Construct(selection, command, _dragBox, _camera, _groundLayer);
        _updater.Construct(movement);

        for (int i = 0; i < _units.Length; i++)
        {
            _units[i].Construct(registry);
            movement.Register(_units[i]);

            // 유닛에게 SeparationService를 주입(전달)하는 코드를 추가했습다.[주현씨 수정]
            if (_units[i] is Unit unit)
            {
                unit.InjectSeparation(separation, registry);
            }
        }
    }
}
