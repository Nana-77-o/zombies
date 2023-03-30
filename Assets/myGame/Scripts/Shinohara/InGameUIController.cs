using UnityEngine;
using UnityEngine.UI;

/// <summary>InGame��UI���X�V���s���N���X </summary>
public class InGameUIController : MonoBehaviour
{
    [SerializeField, Tooltip("�c�莞�Ԃ�\������e�L�X�g")]
    Text _timeText = default;
    [SerializeField, Tooltip("HP�Q�[�W")]
    Image _hpGauge = default;
    [SerializeField, Tooltip("������@UI")]
    GameObject _operationUI;
    [SerializeField, Tooltip("�~�b�V����UI")]
    MissonUI _missionUI;
    [SerializeField]
    Image _hpIcon = default;
    [SerializeField]
    RectTransform _hpBase = default;
    [SerializeField]
    RectTransform _hpBaseBack = default;
    [SerializeField]
    Color _backIconColor = default;
    private GameObject[] _hpIcons = default;
    private int _hpCount = 0;
    private float _timer = 1;
    private void Start()
    {
        _missionUI.gameObject.SetActive(false);
        PlayerInput.SetEnterInput(InputType.Menu, ViewMissionUI);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Menu, ViewMissionUI);
    }
    private void FixedUpdate()
    {
        if (!PlayerInput.Instance.IsInputting)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0) { _operationUI.SetActive(true); }
            return;
        }
        _operationUI.SetActive(false);
        _timer = 1;
    }
    private void ViewMissionUI()
    {
        _missionUI.gameObject.SetActive(!_missionUI.gameObject.activeInHierarchy);
    }
    /// <summary>�c�莞�Ԃ̃e�L�X�g���X�V���� </summary>
    /// <param name="time">�c�莞��</param>
    public void UpdateTimeText(float time)
    {
        _timeText.text = time.ToString("F2");
    }

    /// <summary>Hp�Q�[�W���X�V����</summary>
    /// <param name="currentHp">���݂̗̑�</param>
    /// <param name="maxHp">�ő�̗�</param>
    public void UpdateHpGauge(float currentHp, float maxHp)
    {
        _hpGauge.fillAmount = currentHp / maxHp;
    }
    public void SetHPView(int maxCount)
    {
        _hpIcons = new GameObject[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            var icon = Instantiate(_hpIcon, _hpBase);
            icon.gameObject.SetActive(true);
            _hpIcons[i] = icon.gameObject;
            icon = Instantiate(_hpIcon, _hpBaseBack);
            icon.color = _backIconColor;
            icon.gameObject.SetActive(true);
        }
        _hpCount = maxCount;
    }
    public void ShowHPView(int current)
    {
        if (_hpCount == 0) { return; }
        foreach (var icon in _hpIcons)
        {
            icon.SetActive(true);
        }
        int count = _hpCount - current;
        for (int i = 0; i < count; i++)
        {
            _hpIcons[i].SetActive(false);
        }
    }
}
