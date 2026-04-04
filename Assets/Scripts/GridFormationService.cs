using System.Collections.Generic;
using UnityEngine;

public sealed class GridFormationService : IFormationService
{
    // 유닛 간 최소 간격
    private const float Spacing = 1.5f;

    public void CalculatePositions(IReadOnlyList<IUnit> units, Vector3 destination, List<Vector3> outPositions)
    {
        outPositions.Clear(); // 이전 결과 제거

        int count = units.Count; // 유닛 수
        int width = Mathf.CeilToInt(Mathf.Sqrt(count)); // 한 줄에 배치할 수

        for (int i = 0; i < count; i++)
        {
            int x = i % width;   // 가로 인덱스
            int z = i / width;   // 세로 인덱스

            // 중앙 정렬을 위한 오프셋 계산
            float offsetX = (x - width / 2f) * Spacing;
            float offsetZ = (z - width / 2f) * Spacing;

            // 최종 위치 계산
            Vector3 pos = destination + new Vector3(offsetX, 0f, offsetZ);

            outPositions.Add(pos);
        }
    }
}