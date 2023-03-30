using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChangeScene : MonoBehaviour
{
    // �{�^������Ăяo���z��̃��\�b�h
    public void ChangeScene(string nextSceneName)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneControl.ChangeTargetScene(nextSceneName);
            GameData.Instance.ResetCountData();
        }
        else
        {
            Debug.LogWarning("�������w�肳��Ă܂���");
        }
    }
}
