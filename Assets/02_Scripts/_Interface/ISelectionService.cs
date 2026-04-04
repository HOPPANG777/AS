using System.Collections.Generic;
using UnityEngine;

public interface ISelectionService
{
    IReadOnlyList<IUnit> SelectedUnits { get; }
    void Select(Rect screenRect);
    void Clear();
}
