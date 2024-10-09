using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    [SerializeField] CanvasGroup Scene_canvasGroup;//씬 전환할때 쓰는 UI 전체 그룹
    [SerializeField] CanvasGroup DayScene_Change_canvasGroup;//클리어시 씬 전환할때 쓰는 UI 전체 그룹
    [SerializeField] CanvasGroup GameOver_Change_canvasGroup;//게임오버시 씬 전환할때 쓰는 UI 전체 그룹
    [SerializeField] Image sceneChange_BackGround;//배경 검은 화면
    [SerializeField] GameObject DayScene_Change;//낮씬으로 바꿀때 띄울것들
    [SerializeField] GameObject GameOver_Change;//게임오버일때 띄울것들
    [SerializeField] TextMeshProUGUI gameOver_text;//게임오버 텍스트
    [SerializeField] TextMeshProUGUI gameOver_Continue_text;//게임오버일때 띄울 설명 텍스트
    [SerializeField] float gameOver_Continue_text_blink_delay;//텍스트 깜빡임 딜레이
    [SerializeField] TextMeshProUGUI moving_text;//씬 전환할때 움직이는 시간 텍스트
    [SerializeField] Vector2 moving_text_target_pos;//시간 텍스트가 움직일 위치
    [SerializeField] Vector2 moving_text_origin_pos;//시간 텍스트 원래 위치
    [SerializeField] float time_text_move_duration;//움직이는 텍스트 움직이는 시간
    [SerializeField] float fadeinDuration = 1.0f;//페이드 아웃하는데 걸리는 시간
    [SerializeField] float fadeOutDuration = 1.0f;//페이드 아웃하는데 걸리는 시간

    [SerializeField] AudioClip clear_Clip;
    bool  isEnterClicked = false;//엔터키 눌렸는지

    public void Initialize()
    {
        DayScene_Change.gameObject.SetActive(false);
        GameOver_Change.gameObject.SetActive(false);
        isEnterClicked = false;
    }
    public void GameStart()
    {
        Initialize();
        StartCoroutine(FadeInOut(false, fadeOutDuration, Scene_canvasGroup));
    }
    public IEnumerator ChangeScene(string sceneName)
    {
        //화면 페이드 인
        sceneChange_BackGround.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInOut(true, fadeinDuration, Scene_canvasGroup));
        if(sceneName == "DayScene")
        {

            DayScene_Change.gameObject.SetActive(true);
            yield return StartCoroutine(FadeInOut(true, fadeinDuration, DayScene_Change_canvasGroup));
            Sound_Manager.Instance.PlaySFX(clear_Clip);
            yield return StartCoroutine(Moving_Text());
            //텍스트 페이드 아웃
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FadeInOut(false, fadeOutDuration, DayScene_Change_canvasGroup));
        }
        else if(sceneName == "TitleScene")
        {
            GameOver_Change.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);//게임오버 텍스트 띄우고 잠시 대기
            yield return StartCoroutine(BlinkText(gameOver_Continue_text));//대기후 엔터 누르라는 텍스트 깜빡임
            yield return StartCoroutine(FadeInOut(false, fadeOutDuration, GameOver_Change_canvasGroup));//엔터를 누르면 페이드아웃
        }
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Moving_Text()
    {
        moving_text.rectTransform.anchoredPosition = moving_text_origin_pos;
        float timeElapsed = 0f;
        while (timeElapsed < time_text_move_duration)
        {
            moving_text.rectTransform.anchoredPosition = Vector2.Lerp(moving_text_origin_pos, moving_text_target_pos, timeElapsed / time_text_move_duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        moving_text.rectTransform.anchoredPosition = moving_text_target_pos;
    }
    public IEnumerator BlinkText(TextMeshProUGUI text)//텍스트 깜빡이는 효과
    {
        StartCoroutine(CheckEnterClick());
        while (!isEnterClicked)
        {
            text.gameObject.SetActive(!gameOver_Continue_text.gameObject.activeSelf);
            yield return new WaitForSeconds(gameOver_Continue_text_blink_delay);
        }
    }
    public IEnumerator CheckEnterClick()//엔터키 눌렀는지 확인
    {
        while(!isEnterClicked)
        {
            if(Input.GetKeyDown(KeyCode.Return))
                isEnterClicked = true;
            yield return null;
        }
    }
    public IEnumerator FadeInOut(bool fadeIn, float fadeDuration, CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = fadeIn ? 0 : 1;

        float timeElapsed = 0;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(fadeIn ? 0 : 1, fadeIn ? 1 : 0, timeElapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = fadeIn ? 1 : 0;  // 마지막에 정확히 목표 알파 설정
    }
    
    /* public IEnumerator FadeOut_Text(Color color)
    {
        color.a = 1;

        float timeElapsed = 0;
        while (timeElapsed < fadeOutDuration)
        {
            timeElapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timeElapsed / fadeOutDuration);
            yield return null;
        }

        color.a = 0;  // 마지막에 정확히 목표 알파 설정
    } */
}
