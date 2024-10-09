using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class URPController : MonoBehaviour
{
    [SerializeField] UniversalRenderPipelineAsset URP;
    private void Start()
    {
        GraphicsSettings.renderPipelineAsset = URP;
        QualitySettings.renderPipeline = URP;
    }
}
