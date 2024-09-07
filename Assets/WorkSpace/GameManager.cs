using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] UniversalRenderPipelineAsset DayURP, NightURP;
    private void Start()
    {
        StartCoroutine(UiUtils.GetUI<DialogueManager>().LoadDialoguesFromGoogleSheet());

        GraphicsSettings.renderPipelineAsset = DayURP;
        QualitySettings.renderPipeline = DayURP;

    }
    private void OnDestroy() 
    {
        GraphicsSettings.renderPipelineAsset = NightURP;
         QualitySettings.renderPipeline = NightURP;    
    }
    // private void OnDisable()
    // {
    //     
    // }
}
