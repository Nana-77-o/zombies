using System;
using UnityEngine;

public static class SceneControl 
{
    private static bool sceneChange = false;
    public static Action OnSceneChange = default;
    public static void ChangeTargetScene(string sceneName)
    {
        if (!sceneChange)
        {
            sceneChange = true;
            FadeController.StartFadeOutIn(() =>
            {
                OnSceneChange?.Invoke();
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                sceneChange = false;
            });
        }
    }
}
