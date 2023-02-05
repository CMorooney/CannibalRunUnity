using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;

    private void Awake()
    {
        StartButton.onClick.AddListener(StartButtonClicked);
        ExitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void StartButtonClicked() => SceneManager.LoadScene("Scenes/Game");

    private void ExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        StartButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }
}

