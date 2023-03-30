using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChangeScene : MonoBehaviour
{
    // ボタンから呼び出す想定のメソッド
    public void ChangeScene(string nextSceneName)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneControl.ChangeTargetScene(nextSceneName);
            GameData.Instance.ResetCountData();
        }
        else
        {
            Debug.LogWarning("引数が指定されてません");
        }
    }
}
