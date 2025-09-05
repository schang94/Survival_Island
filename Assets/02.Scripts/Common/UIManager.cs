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
            // �� ��ȯ�� ���콺 Ŀ�� ���̰� �ؼ� ��ư�� Ŭ���� �����ϰ� ����.
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
