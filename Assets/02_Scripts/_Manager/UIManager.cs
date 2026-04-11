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

    public bool isMoveTargetingMode = false;

    // 사운드 재생을 위한 변수들
    [Header("Audio")]
    public AudioClip buttonClickSound; // 인스펙터에서 연결할 버튼 클릭 소리
    private AudioSource audioSource;   // UI의 스피커 역할

    private void Awake()
    {
        Instance = this;
        unitInfoPanel.SetActive(false); // 시작할 땐 숨김

        // 스피커(AudioSource) 컴포넌트를 찾거나 없으면 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void ShowUnitInfo(int hp, int maxHp, int troops, int atk, int def, int spd)
    {
        unitInfoPanel.SetActive(true);
        hpText.text = $"체  력 {hp} / {maxHp}";
        troopsText.text = $"병  력 {troops}명";
        attackText.text = $"공격력 {atk}";
        defenseText.text = $"방어력 {def}";
        speedText.text = $"이동력 {spd}";

        isMoveTargetingMode = false;
    }

    public void HideUnitInfo()
    {
        unitInfoPanel.SetActive(false);
        isMoveTargetingMode = false;
    }

    public void OnClickMoveButton()
    {
        PlayClickSound(); //  효과음 재생!
        isMoveTargetingMode = true;
        Debug.Log("이동 버튼 클릭됨! 이제 마우스를 드래그해서 경로를 그리세요.");
    }

    public void OnClickAttackButton()
    {
        PlayClickSound(); //  효과음 재생!
        Debug.Log("공격 버튼 클릭됨! (현재 기능 없음)");
    }

    // 소리를 재생하는 공통 도우미 함수
    private void PlayClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            // 소리가 겹쳐도 자연스럽게 재생되도록 PlayOneShot 사용
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}