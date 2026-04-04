using UnityEngine;

public sealed class MovementUpdater : MonoBehaviour
{
    private IMovementService _movement;
    private ISelectionService _selection; // 선택 서비스를 참조하기 위해 추가

    // Construct 함수에 selection을 추가로 받도록 수정합니다.
    public void Construct(IMovementService movement, ISelectionService selection)
    {
        _movement = movement;
        _selection = selection;
    }

    private void Update()
    {
        _movement.Tick();

        // 모든 유닛이 도착했는지 확인
        if (_movement.IsAllArrived())
        {
            // 도착하는 순간 모든 유닛의 선택을 해제합니다.
            // 이 명령이 유닛의 OnDeselect()를 실행시키고, 강조 원과 UI를 비활성화합니다.
            if (_selection != null)
            {
                _selection.Clear();
            }

            _movement.Clear(); // 이동 정보 초기화
        }
    }
}