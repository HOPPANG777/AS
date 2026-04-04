using System.Collections.Generic;
using UnityEngine;

public interface ISeparationService
{
    // 주변 유닛과의 거리 기반으로 밀어내는 힘을 계산한다
    Vector3 CalculateSeparation(IUnit self, IReadOnlyList<IUnit> units);
}
