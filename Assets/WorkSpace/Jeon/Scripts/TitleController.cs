using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class TitleController : MonoBehaviour
{
    public Button daySceneButton; // DayScene으로 넘어가는 버튼

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        daySceneButton.onClick.AddListener(OnDaySceneButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // DayScene으로 넘어가는 메서드
    void OnDaySceneButtonClick()
    {
        SceneManager.LoadScene("DayScene");
    }
}
