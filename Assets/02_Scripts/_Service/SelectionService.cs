using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class SelectionService : ISelectionService
{
    private readonly IUnitRegistry _registry;
    private readonly Camera _camera;

    private readonly List<IUnit> _selected = new();

    public IReadOnlyList<IUnit> SelectedUnits => _selected;

    public SelectionService(IUnitRegistry registry, Camera camera)
    {
        _registry = registry;
        _camera = camera;
    }

    public void Select(Rect rect)
    {
        // 기존 선택 해제
        for (int i = 0; i < _selected.Count; i++)
        {
            if (_selected[i] is ISelectable selectable)
                selectable.OnDeselect();
        }

        _selected.Clear();

        var units = _registry.Units;

        for (int i = 0; i < units.Count; i++)
        {
            Vector3 screenPos = _camera.WorldToScreenPoint(units[i].Transform.position);

            if (rect.Contains(screenPos))
            {
                _selected.Add(units[i]);

                if (units[i] is ISelectable selectable)
                    selectable.OnSelect();
            }
        }
    }

    public void SelectSingle(IUnit unit)
    {
        for (int i = 0; i < _selected.Count; i++)
        {
            if (_selected[i] is ISelectable selectable)
                selectable.OnDeselect();
        }

        _selected.Clear();

        if (unit != null)
        {
            _selected.Add(unit);

            if (unit is ISelectable selectable)
                selectable.OnSelect();
        }
    }

    public void Clear()
    {
        _selected.Clear();
    }
}
