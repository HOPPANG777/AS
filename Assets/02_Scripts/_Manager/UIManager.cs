using TMPro;
using UnityEngine;
using UnityEngine.UI;
// TMPro를 쓴다면 using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject unitInfoPanel; // 우측 하단 정보창 패널 전체

    [Header("UI Texts")]

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI troopsText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;

    private void Awake()
    {
        Instance = this;
        unitInfoPanel.SetActive(false); // 시작할 땐 숨김
    }

    // 유닛을 클릭했을 때 호출할 함수
    public void ShowUnitInfo(int hp, int maxHp, int troops, int atk, int def, int spd)
    {
        unitInfoPanel.SetActive(true);
        hpText.text = $"체  력 {hp} / {maxHp}";
        troopsText.text = $"병  력 {troops}명";
        attackText.text = $"공격력 {atk}";
        defenseText.text = $"방어력 {def}";
        speedText.text = $"이동력 {spd}";
    }

    // 빈 땅을 클릭하거나 선택 해제 시 호출
    public void HideUnitInfo()
    {
        unitInfoPanel.SetActive(false);
    }
}