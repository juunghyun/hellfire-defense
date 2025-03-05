using UnityEngine;
using TMPro;

public class GlassPanel : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh; // 글자 변경할 TextMeshPro
    [SerializeField] private bool isBluePanel = true; // Blue 패널인지 여부 (기본값 true)
    private int currentValue; // 초기 숫자 값

    void Start()
    {
        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
        }

        // 패널 색상에 따라 초기 숫자 설정
        currentValue = isBluePanel ? 10 : -10;
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // 총알이 닿았을 때
        {
            int changeAmount = 1; // 숫자 변화량
            
            if (isBluePanel)
            {
                IncreaseValue(changeAmount); // Blue는 증가
            }
            else
            {
                DecreaseValue(changeAmount); // Red는 감소
            }
        }
    }

    private void IncreaseValue(int amount)
    {
        if (currentValue < 100) // 이미 MAX면 증가 안 함
        {
            currentValue += amount;
            if (currentValue > 100) currentValue = 100; // 최대값 제한
        }
        UpdateText();
    }

    private void DecreaseValue(int amount)
    {
        if (currentValue > -100) // 이미 -MAX면 감소 안 함
        {
            currentValue -= amount;
            if (currentValue < -100) currentValue = -100; // 최소값 제한
        }
        UpdateText();
    }

    private void UpdateText()
    {
        // MAX 제한일 경우 "MAX" 표시
        if (currentValue >= 100)
        {
            textMesh.text = "MAX";
        }else if(currentValue <= -100)
        {
            textMesh.text = "-MAX";
        }
        else
        {
            // 양수면 "+" 붙이기, 음수면 그대로
            textMesh.text = (currentValue >= 0 ? "+" : "") + currentValue.ToString();
        }
    }
}
