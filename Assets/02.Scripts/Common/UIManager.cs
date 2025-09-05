using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text killTxt;

    void Start()
    {
        if (GameManager.instance != null)
        {
            killTxt.text = $"Kill : <color=#ff0000>{GameManager.instance.gameData.killCount:000}</color>";
            // 씬 전환시 마우스 커서 보이게 해서 버튼을 클릭이 가능하게 하자.
            GameManager.instance.MouseCursorVisible();
        }
    }

    
    public void PlaySceneMove()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
