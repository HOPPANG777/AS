using System.Collections.Generic;
using UnityEngine;

public sealed class GridFormationService : IFormationService
{
    private const float Spacing = 1.5f; // 유닛 간 간격

    public void CalculatePositions(IReadOnlyList<IUnit> units, Vector3 destination, Vector3 forward, List<Vector3> outPositions)
    {
        outPositions.Clear(); // 결과 초기화

        int count = units.Count; // 유닛 수
        int width = Mathf.CeilToInt(Mathf.Sqrt(count)); // 가로 개수

        // forward를 기준으로 회전 생성
        Quaternion rot = Quaternion.LookRotation(forward.normalized);

        for (int i = 0; i < count; i++)
        {
            int x = i % width; // 가로 index
            int z = i / width; // 세로 index

            float offsetX = (x - width / 2f) * Spacing; // 중심 기준 오프셋
            float offsetZ = (z - width / 2f) * Spacing;

            Vector3 local = new Vector3(offsetX, 0f, offsetZ); // 로컬 포지션
            Vector3 rotated = rot * local; // 회전 적용

            outPositions.Add(destination + rotated); // 월드 좌표
        }
    }
}