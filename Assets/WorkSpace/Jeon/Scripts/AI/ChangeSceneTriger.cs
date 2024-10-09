using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] AudioClip audioClip,audioClip2;
    private CinemachineVirtualCamera virtualCamera;
    private Volume volume;

    public TextMeshProUGUI fadeText; // 날짜를 표시할 텍스트

    private DateTime startDate = new DateTime(2024, 8, 7); // 시작 날짜 설정

    // Start is called before the first frame update
    void Start()
    {
        // 자식 오브젝트에서 CinemachineVirtualCamera 찾기
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(true); // 비활성화된 상태에서도 찾기 위해 true로 설정
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera not found in children.");
            return;
        }
        virtualCamera.gameObject.SetActive(false); // 초기에는 비활성화

        // Volume 컴포넌트 찾기
        volume = GetComponent<Volume>();
        if (volume == null)
        {
            Debug.LogError("Volume component not found.");
            return;
        }

        // 초기에는 텍스트 비활성화
        fadeText.transform.parent.gameObject.SetActive(false);
        fadeText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 테스트를 위해 스페이스 키를 눌러서 트리거 실행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeAndChangeScene("NightScene"));
        }
    }

    private IEnumerator FadeAndChangeScene(string sceneName)
    {
        Sound_Manager.Instance.PlaySFX(audioClip);
        // CinemachineVirtualCamera와 Volume 활성화
        virtualCamera.gameObject.SetActive(true);
        volume.gameObject.SetActive(true);

        // 렌즈 사이즈 서서히 줄이기
        yield return StartCoroutine(ZoomCamera(2.0f, 0f));

        // 텍스트 표시 및 날짜 증가
        fadeText.transform.parent.gameObject.SetActive(true);
        fadeText.gameObject.SetActive(true);
        yield return StartCoroutine(DisplayDateText(1f)); // 날짜를 하루 증가시키면서 표시

        // 씬 전환
        SceneManager.LoadScene(sceneName);

        // 텍스트 비활성화
        fadeText.transform.parent.gameObject.SetActive(false);
        fadeText.gameObject.SetActive(false);
    }

    private IEnumerator ZoomCamera(float duration, float targetOrthographicSize)
    {
        float initialOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetOrthographicSize;
    }

    private IEnumerator DisplayDateText(float displayDuration)
    {
                Sound_Manager.Instance.PlaySFX(audioClip2);
        DateTime originalDate = startDate;
        DateTime nextDate = startDate.AddDays(1);
        string originalDateText = originalDate.ToString("M월 d일");
        string nextDateText = nextDate.ToString("M월 d일");
        // 원래 날짜 텍스트 표시
        yield return StartCoroutine(TypeText(originalDateText, displayDuration));
        yield return new WaitForSeconds(0.5f); // 약간의 텀

        // 뒤에서부터 텍스트 지우기
        yield return StartCoroutine(DeleteText(originalDateText, displayDuration));
        yield return new WaitForSeconds(0.5f); // 약간의 텀

        // 하루 지난 날짜 텍스트 표시
        yield return StartCoroutine(TypeText(nextDateText, displayDuration));

        // 텍스트가 완료되면 일정 시간 대기
        yield return new WaitForSeconds(displayDuration);
    }

    private IEnumerator TypeText(string text, float duration)
    {
        fadeText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            fadeText.text += text[i];
            yield return new WaitForSeconds(duration / text.Length);
        }
    }

    private IEnumerator DeleteText(string text, float duration)
    {
        for (int i = text.Length - 1; i >= 0; i--)
        {
            fadeText.text = text.Substring(0, i);
            yield return new WaitForSeconds(duration / text.Length);
        }
    }
}
