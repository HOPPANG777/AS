using UnityEngine;
using System.Collections.Generic;

public sealed class SeparationService : ISeparationService
{
    private const float Radius = 1.5f; // 영향 반경
    private const float Force = 2f;    // 밀어내는 힘

    public Vector3 CalculateSeparation(IUnit self, IReadOnlyList<IUnit> units)
    {
        Vector3 result = Vector3.zero; // 누적 힘

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == self) continue;

            Vector3 diff = self.Transform.position - units[i].Transform.position;
            float dist = diff.magnitude;

            if (dist < Radius && dist > 0.001f)
            {
                result += diff.normalized * (Force / dist); // 가까울수록 강하게 밀어냄
            }
        }

        return result;
    }
}