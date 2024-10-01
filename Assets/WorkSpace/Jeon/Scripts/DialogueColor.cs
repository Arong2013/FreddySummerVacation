using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogueColor : MonoBehaviour
{
    private Image image;
    private Color originalColor;
    private bool isFlashing = false;
    private float flashTimer;

    // Odin Inspector를 활용한 인스펙터 조정 가능 변수들
    [FoldoutGroup("Flash Settings"), SerializeField, Tooltip("호감도 상승 시의 목표 색상")]
    private Color positiveColor = new Color(0f, 1f, 0f, 0.5f); // 기본: 반투명 초록색

    [FoldoutGroup("Flash Settings"), SerializeField, Tooltip("호감도 하락 시의 목표 색상")]
    private Color negativeColor = new Color(1f, 0f, 0f, 0.5f); // 기본: 반투명 빨간색

    [FoldoutGroup("Flash Settings"), SerializeField, Tooltip("빛나는 시간 (초 단위)")]
    private float flashDuration = 0.5f; // 기본: 0.5초

    private Color targetColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color; // 원래 색상 저장
    }

    // 호감도 변화에 따라 색상 빛나기 효과 적용
    public void UpdateAffinityEffect(int affinityChange)
    {
        if (affinityChange > 0)
        {
            FlashPositive();  // 호감도 상승 시
        }
        else if (affinityChange < 0)
        {
            FlashNegative();  // 호감도 하락 시
        }
    }

    // 호감도 상승 시 빛나게
    private void FlashPositive()
    {
        if (!isFlashing)
        {
            targetColor = positiveColor; // Odin Inspector에서 설정 가능한 색상
            StartCoroutine(FlashEffect());
        }
    }

    // 호감도 하락 시 빛나게
    private void FlashNegative()
    {
        if (!isFlashing)
        {
            targetColor = negativeColor; // Odin Inspector에서 설정 가능한 색상
            StartCoroutine(FlashEffect());
        }
    }

    private IEnumerator FlashEffect()
    {
        isFlashing = true;
        flashTimer = 0f;
        Color startColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0); // 투명한 시작 색상

        // 투명 -> 목표 색상으로 변화
        while (flashTimer < flashDuration)
        {
            flashTimer += Time.deltaTime;
            float t = flashTimer / flashDuration;

            // 색상을 투명 -> 목표 색상으로 Lerp
            image.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // 잠시 유지 후 원래 색상으로 복구
        yield return new WaitForSeconds(0.2f);

        flashTimer = 0f;
        while (flashTimer < flashDuration)
        {
            flashTimer += Time.deltaTime;
            float t = flashTimer / flashDuration;

            // 목표 색상 -> 원래 색상으로 Lerp
            image.color = Color.Lerp(targetColor, originalColor, t);

            yield return null;
        }

        image.color = originalColor;
        isFlashing = false;
    }
}
