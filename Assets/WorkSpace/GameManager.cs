using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UniversalRenderPipelineAsset DayURP, NightURP;

    // 캐릭터 ID와 호감도를 저장하는 딕셔너리
    private Dictionary<string, int> characterAffinities = new Dictionary<string, int>();

    private void Start()
    {
        StartCoroutine(UiUtils.GetUI<DialogueManager>().LoadDialoguesFromGoogleSheet());

        //     GraphicsSettings.renderPipelineAsset = DayURP;
        //     QualitySettings.renderPipeline = DayURP;

        // 예시로 캐릭터의 호감도를 딕셔너리에 추가 (이 부분은 실제 대화 로드 후 업데이트될 수 있음)
        characterAffinities.Add("Va", 0); // 빌런 캐릭터 ID Va, 호감도 0
        characterAffinities.Add("Vb", 0); // 빌런 캐릭터 ID Va, 호감도 0
        characterAffinities.Add("Vc", 0); // 빌런 캐릭터 ID Va, 호감도 0
        characterAffinities.Add("Vd", 0); // 빌런 캐릭터 ID Va, 호감도 0
        characterAffinities.Add("Ve", 0); // 빌런 캐릭터 ID Va, 호감도 0
    }
    private void Update()
    {
        // ESC 키를 눌렀을 때 ExitUI의 활성화 상태를 확인


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var exitUI = UiUtils.GetUI<ExitUI>();
            if (exitUI.gameObject.activeSelf)
            {
                // ExitUI가 켜져 있으면 비활성화
                exitUI.gameObject.SetActive(false);
            }
            else
            {
                // ExitUI가 꺼져 있으면 활성화
                exitUI.gameObject.SetActive(true);
            }
        }
    }
    private void OnDestroy()
    {
        //   GraphicsSettings.renderPipelineAsset = NightURP;
        //    QualitySettings.renderPipeline = NightURP;
    }

    // 특정 캐릭터 ID에 대한 호감도 조회
    public int GetAffinity(string characterId)
    {
        if (characterAffinities.ContainsKey(characterId))
        {
            return characterAffinities[characterId];
        }
        else
        {
            Debug.LogWarning($"Character ID {characterId} not found.");
            return 0; // 기본 호감도
        }
    }
    public void UpdateAffinity(string characterId, int affinityChange)
    {
        if (characterAffinities.ContainsKey(characterId))
        {
            characterAffinities[characterId] += affinityChange;
            Debug.Log($"Updated affinity for {characterId}: {characterAffinities[characterId]}");

            // UI에서 DialogueManager를 통해 캐릭터의 UI 요소에 접근
            DialogueColor dialogueColor = UiUtils.GetUI<DialogueColor>();
            dialogueColor.UpdateAffinityEffect(affinityChange);
        }
        else
        {
            Debug.LogWarning($"Character ID {characterId} not found.");
        }
    }

    public void ShowAllAffinities()
    {
        foreach (var entry in characterAffinities)
        {
            Debug.Log($"Character ID: {entry.Key}, Affinity: {entry.Value}");
        }
    }
}
