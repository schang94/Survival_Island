using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public CanvasGroup fadeCG;

    [Range(0.5f, 2.0f)] public float fadeDuration = 1f;

    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();

    void InitSceneInfo()
    {
        loadScenes.Add("LevelScene", LoadSceneMode.Additive);
        loadScenes.Add("MainScene", LoadSceneMode.Additive);
    }

    IEnumerator Start()
    {
        InitSceneInfo();

        foreach (var scene in loadScenes)
        {
            yield return StartCoroutine(LoadScene(scene.Key, scene.Value));
        }

        StartCoroutine(Fade(0)) ;
    }


    IEnumerator LoadScene(string name, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(name, mode);

        Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(scene);
    }

    IEnumerator Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        fadeCG.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        fadeCG.blocksRaycasts = false;

        SceneManager.UnloadSceneAsync("LoadScene");
    }
}
