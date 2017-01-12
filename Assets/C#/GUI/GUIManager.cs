using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    // Выход из игры.
    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // Загрузка уровня
    public void LoadLevel(int num) {
        string s = string.Format("level_{0,3:000}", num);
        Debug.Log(s);
        SceneManager.LoadScene(s, LoadSceneMode.Single);
    }
}
