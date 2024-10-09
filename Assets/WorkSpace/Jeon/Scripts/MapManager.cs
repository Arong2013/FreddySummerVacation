using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class MapManager : SerializedMonoBehaviour
{
    public Image fadeImage;

    [OdinSerialize]
    [DictionaryDrawerSettings(KeyLabel = "맵 이름", ValueLabel = "맵 오브젝트")]
    public Dictionary<string, GameObject> Maps = new Dictionary<string, GameObject>();
    private GameObject currentMap;

    private void Start()
    {
        InitializeMaps();
    }

    private void InitializeMaps()
    {
        if (Maps != null && Maps.Count > 0)
        {
            bool firstMap = true;
            foreach (var map in Maps.Values)
            {
                if (firstMap)
                {
                    currentMap = map;
                    firstMap = false;
                }
                else
                {
    
                    map.SetActive(false);
                }
            }
        }
    }

    public void ChangeMap(string mapName)
    {
        if (!Maps.ContainsKey(mapName))
        {
            Debug.LogError("Invalid map name");
            return;
        }

        StartCoroutine(FadeAndChangeMap(mapName));
    }

   private IEnumerator FadeAndChangeMap(string mapName)
    {
        // Fade Out
        yield return StartCoroutine(FadeImage(1f, 1f));

        // Change map
        currentMap?.SetActive(false);
        currentMap = Maps[mapName];
        currentMap?.SetActive(true);

        // Fade In
        yield return StartCoroutine(FadeImage(0f, 1f));
    }

    private IEnumerator FadeImage(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }

    public List<string> GetAvailableMapNames()
    {
        return new List<string>(Maps.Keys);
    }
}