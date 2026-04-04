using System.Collections.Generic;
using UnityEngine;

public interface ISelectionService
{
    IReadOnlyList<IUnit> SelectedUnits { get; }
    void Select(Rect screenRect);
    void SelectSingle(IUnit unit); // 단일 선택
    void Clear();
}
