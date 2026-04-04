using UnityEngine;

public interface ISelectable
{
    // 선택되었을 때 호출
    void OnSelect();

    // 선택 해제되었을 때 호출
    void OnDeselect();
}
