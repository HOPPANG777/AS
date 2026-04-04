using System.Collections.Generic;
using UnityEngine;

public interface IFormationService
{
    // 선택된 유닛들을 목적지 기준으로 겹치지 않게 배치 좌표를 계산한다
    void CalculatePositions(IReadOnlyList<IUnit> units, Vector3 destination, List<Vector3> outPositions);
}
