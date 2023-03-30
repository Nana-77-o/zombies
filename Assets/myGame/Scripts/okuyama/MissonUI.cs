using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissonUI : MonoBehaviour
{
    [SerializeField, Tooltip("�~�b�V�����pText")] Text[] _missonText = default;
    [SerializeField, Tooltip("�`�F�b�N�}�[�N")]GameObject[] _missonImage = null;
    [SerializeField, Tooltip("�g���t�B�[")] Image _trophy = default;
    [SerializeField, Tooltip("������")] Text _enemyCount = default;
    [SerializeField, Tooltip("����")] Text _timeCount = default;
    [SerializeField] Button _closeButton = default;
    [SerializeField] Button _exitButton = default;

    private float timeScale = 1;
    private void Start()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _exitButton.onClick.AddListener(() => SceneControl.ChangeTargetScene("TitleSample"));
    }
    private void OnEnable()
    {
        _trophy.fillAmount = 0;
        timeScale = Time.timeScale;
        UIUpdate();
    }
    private void OnDisable()
    {
        Time.timeScale = timeScale;
    }
    private void UIUpdate()
    {
        int count = 0, trophyPercent = 0, index = 0;

        foreach (var item in BossMissionManager.Instance.SPMission.MissionData())
        {
            _missonText[count++].text = $"{item.MissionName}         {item.CurrentCount}/{item.TargetCount}";
            if (item.IsClearMission)
            {
                trophyPercent++;
                _missonImage[index++].SetActive(true);
            }
            else if (!item.IsClearMission)
            {
                _missonImage[index++].SetActive(false);
            }
        }
        _trophy.fillAmount = trophyPercent / 10f;
        _timeCount.text = GameTimer.CurrentTimeText;
        _enemyCount.text = GameData.Instance.CountData[0].ToString();
        Time.timeScale = 0;
    }

    void MissonUIOpen()//�~�b�V�����̃^�u���J���ꂽ�Ƃ��ɍX�V��������
    {
        StartCoroutine(TextUpdate());
    }

    IEnumerator TextUpdate()
    {
        yield return null;
        //_enemyCount.text = $"{GameData.Instance.CountData}";
        int count = 0, trophyPercent = 0, index = 0;
        
        foreach (var item in BossMissionManager.Instance.SPMission.MissionData())
        {
            _missonText[count++].text = $"{item.MissionName}         {item.CurrentCount}/{item.TargetCount}";
            if (item.IsClearMission)
            {
                trophyPercent++;
                _missonImage[index++].SetActive(true);
            }
            else if (!item.IsClearMission)
            {
                _missonImage[index++].SetActive(false);
            }
        }
        _trophy.fillAmount = trophyPercent / 10f;
    }
}
