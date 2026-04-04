using UnityEngine;

public sealed class PathVisualizer : MonoBehaviour, IPathVisualizer
{
    [SerializeField] private LineRenderer _line; // 경로를 그리는 라인
    [SerializeField] private GameObject _circle; // 목적지 표시 원

    private Vector3 _end; // 목적지 위치

    public void Show(Vector3 start, Vector3 end)
    {
        _end = end; // 목적지 저장

        _line.positionCount = 2; // 시작/끝 2개 점
        _line.SetPosition(0, start); // 시작 위치
        _line.SetPosition(1, end);   // 끝 위치
        _line.enabled = true;        // 라인 표시

        _circle.SetActive(true);     // 원 활성화
        _circle.transform.position = end; // 원 위치 설정
    }

    public void UpdateProgress(Vector3 current)
    {
        // 시작점을 현재 위치로 갱신 → 지나간 부분 제거
        _line.SetPosition(0, current);
    }

    public void Hide()
    {
        _line.enabled = false;       // 라인 숨김
        _circle.SetActive(false);    // 원 숨김
    }
}