using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가
using DG.Tweening; // DOTween을 사용하기 위해 추가

public class TitleController : MonoBehaviour
{
    public Button daySceneButton; // DayScene으로 넘어가는 버튼
    public Image fadeOutImage; // 투명해지는 이미지
    public Image fadeInImage; // 서서히 보이는 이미지
    public float fadeDuration = 1f; // 페이드 지속 시간

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        daySceneButton.onClick.AddListener(OnDaySceneButtonClick);
    }

    // DayScene으로 넘어가는 메서드
    void OnDaySceneButtonClick()
    {
        // 페이드 애니메이션 시작
        StartCoroutine(FadeImagesAndSwitchScene());
    }

    // 이미지 페이드와 씬 전환 처리
    IEnumerator FadeImagesAndSwitchScene()
    {
        // fadeInImage의 초기 투명도 설정
        fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, 0f);

        // fadeOutImage가 서서히 투명해지고, fadeInImage가 서서히 보이도록 설정
        fadeOutImage.DOFade(0f, fadeDuration); // fadeOutImage는 서서히 투명해짐
        fadeInImage.DOFade(1f, fadeDuration); // fadeInImage는 서서히 보임

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(fadeDuration);

        // 페이드 애니메이션이 끝나면 씬 이동
        SceneManager.LoadScene("DayScene");
    }
}
