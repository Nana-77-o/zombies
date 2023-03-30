using UnityEngine;
using UnityEngine.UI;

/// <summary>実績UIで表示する画像を操作するクラス </summary>
public class AchievementController : MonoBehaviour
{
    [SerializeField, Header("選択フレーム色")]
    Color _selectFrameColor = Color.yellow;
    [SerializeField, Header("未選択フレーム色")]
    Color _noSelectFrameColor = Color.red;

    [SerializeField, Tooltip("実績スプライト")]
    Image _achievementImage = default;
    [SerializeField, Tooltip("フレーム")]
    Image _frameImage = default;
    [SerializeField, Tooltip("実績を隠す画像")]
    Image _hideImage = default;
    [SerializeField, Tooltip("実績解除のテキスト")]
    Text _conditionText = default;
    [SerializeField, Header("ID")]
    int _id = 0;

    /// <summary>開いているか(取得)どうか </summary>
    bool _isOpen = true;

    /// <summary>ID</summary>
    public int ID { get => _id; }

    /// <summary>開いているか(取得)どうか </summary>
    public bool IsOpen
    {
        get
        {
            SelectAchievement();    //フレームの色を変更する
            return _isOpen;
        }
    }

    /// <summary>実績スプライト </summary>
    public Image AchievementImage { get => _achievementImage; }

    /// <summary>実績リストのUIを開いた時の処理 </summary>
    public void OpenAchievementUI(bool isOpen, string conditionText)
    {
        _isOpen = isOpen;
        var selectColor = Color.white;
        selectColor.a = 0;

        if (isOpen)     //実績を取得している
        {
            _hideImage.color = selectColor;
            _conditionText.color = selectColor;
            _frameImage.color = _noSelectFrameColor;
            return;
        }

        _frameImage.color = Color.white;
        _conditionText.text = conditionText;
    }

    /// <summary>実績選択時の処理 </summary>
    public void SelectAchievement()
    {
        _frameImage.color = _selectFrameColor;
    }

    /// <summary>実績から離れた時の処理 </summary>
    public void DeselectAchievement()
    {
        if (_isOpen)
        {
            _frameImage.color = _noSelectFrameColor;
            return;
        }

        _frameImage.color = Color.white;
    }
}
