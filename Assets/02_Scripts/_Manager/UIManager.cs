using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject unitInfoPanel; // 우측 하단 정보창 패널

    [Header("UI Texts")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI troopsText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;

    // 현재 '이동 버튼'을 누른 상태인지 확인하는 변수
    public bool isMoveTargetingMode = false;

    private void Awake()
    {
        Instance = this;
        unitInfoPanel.SetActive(false); // 시작할 땐 숨김
    }

    public void ShowUnitInfo(int hp, int maxHp, int troops, int atk, int def, int spd)
    {
        unitInfoPanel.SetActive(true);
        hpText.text = $"체  력 {hp} / {maxHp}";
        troopsText.text = $"병  력 {troops}명";
        attackText.text = $"공격력 {atk}";
        defenseText.text = $"방어력 {def}";
        speedText.text = $"이동력 {spd}";

        // 다른 유닛을 새로 클릭하면 이동 모드는 우선 취소됨
        isMoveTargetingMode = false;
    }

    public void HideUnitInfo()
    {
        unitInfoPanel.SetActive(false);
        isMoveTargetingMode = false;
    }

    // 유니티 에디터에서 '이동' 버튼을 누르면 실행될 함수
    public void OnClickMoveButton()
    {
        isMoveTargetingMode = true; // 드래그 이동 대기 상태로 전환!
        Debug.Log("이동 버튼 클릭됨! 이제 마우스를 드래그해서 경로를 그리세요.");
    }

}