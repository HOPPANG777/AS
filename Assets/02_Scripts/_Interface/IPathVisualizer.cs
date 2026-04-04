using UnityEngine;

public interface IPathVisualizer
{
    // 경로 시작 표시
    void Show(Vector3 start, Vector3 end);

    // 현재 진행 위치 반영 (선의 시작점 갱신)
    void UpdateProgress(Vector3 current);

    // 경로 숨김
    void Hide();
}
