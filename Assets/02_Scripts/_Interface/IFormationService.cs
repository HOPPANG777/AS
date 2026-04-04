using System.Collections.Generic;
using UnityEngine;

public interface IFormationService
{
    // 선택된 유닛들을 목적지 기준으로 겹치지 않게 바라볼 방향을 기준으로 포메이션 배치 좌표를 계산한다
    void CalculatePositions(IReadOnlyList<IUnit> units, Vector3 destination, Vector3 forward, List<Vector3> outPositions);
}
