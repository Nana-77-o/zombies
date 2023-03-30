using UnityEngine;
using UnityEngine.UI;

/// <summary>InGameのUIを更新を行うクラス </summary>
public class InGameUIController : MonoBehaviour
{
    [SerializeField, Tooltip("残り時間を表示するテキスト")]
    Text _timeText = default;
    [SerializeField, Tooltip("HPゲージ")]
    Image _hpGauge = default;
    [SerializeField, Tooltip("操作方法UI")]
    GameObject _operationUI;
    [SerializeField, Tooltip("ミッションUI")]
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
    /// <summary>残り時間のテキストを更新する </summary>
    /// <param name="time">残り時間</param>
    public void UpdateTimeText(float time)
    {
        _timeText.text = time.ToString("F2");
    }

    /// <summary>Hpゲージを更新する</summary>
    /// <param name="currentHp">現在の体力</param>
    /// <param name="maxHp">最大体力</param>
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
