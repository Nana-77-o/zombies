using UnityEngine;
using UnityEngine.UI;

/// <summary>設定関連の処理を管理するクラス</summary>
public class OptionManager : MonoBehaviour
{
    [SerializeField, Tooltip("BGMの音量を変更するスライダー")]
    Slider _bgmSlider = default;
    [SerializeField, Tooltip("BGMスライダーのハンドル")]
    Image _bgmHandleImage = null;
    [SerializeField, Tooltip("BGM表記のフレーム")]
    Image _bgmFrameImage = null;
    [SerializeField, Tooltip("SEの音量を変更するスライダー")]
    Slider _seSlider = default;
    [SerializeField, Tooltip("SEスライダーのハンドル")]
    Image _seHandleImage = default;
    [SerializeField, Tooltip("SE表記のフレーム")]
    Image _seFrameImage = default;
    [SerializeField, Tooltip("クレジットボタンのフレーム")]
    Image _creditFrame = default;
    [SerializeField, Tooltip("クレジット")]
    Canvas _creditUI = default;
    [SerializeField, Tooltip("クレジットを開いた時に選択されるボタン")]
    Button _creditSelectButton = default;
    [SerializeField, Tooltip("設定画面のUI")]
    Canvas _optionUI = default;

    /// <summary>BGMスライダー選択時の処理 </summary>
    public void SelectBGMSlider()
    {
        _bgmHandleImage.color = Color.yellow;
        _bgmFrameImage.color = Color.yellow;
    }

    /// <summary>BGMの音量を変更する処理 </summary>
    public void ChangeBGMVolume()
    {
        SoundManager.InstanceSound.BGMVolumeSeT(_bgmSlider.value);
    }

    /// <summary>SEスライダー選択時の処理 </summary>
    public void SelectSESlider()
    {
        _seHandleImage.color = Color.yellow;
        _seFrameImage.color = Color.yellow;
    }

    /// <summary>SEの音量を変更する処理 </summary>
    public void ChangeSEVolume() 
    {
        SoundManager.InstanceSound.SEVolumeSeT(_seSlider.value);
    }

    /// <summary>BGMスライダー選択解除時の処理 </summary>
    public void DeselectBGMSlider()
    {
        _bgmHandleImage.color = Color.white;
        _bgmFrameImage.color = Color.white;
    }
    /// <summary>SEスライダー選択解除時の処理 </summary>
    public void DeselectSESlider()
    {
        _seHandleImage.color = Color.white;
        _seFrameImage.color = Color.white;
    }

    /// <summary>クレジット表示ボタンを選択時の処理 </summary>
    public void SelectCredit()
    {
        _creditFrame.color = Color.yellow;
    }

    /// <summary>クレジット表示ボタンを選択解除時の処理 </summary>
    public void DeslectCredit()
    {
        _creditFrame.color = Color.white;
    }

    /// <summary>クレジットを表示・非表示を操作する</summary>
    public void CreditControl(bool isEnabled)
    {
        _creditUI.enabled = isEnabled;
        _optionUI.enabled = !isEnabled;

        if (isEnabled)
        {
            _creditSelectButton.Select();
            return;
        }

        _bgmSlider.Select();
    }

    /// <summary>設定UIを開く</summary>
    public void Open()
    {
        _optionUI.enabled = true;
        _bgmSlider.Select();
    }

    /// <summary>設定UIを閉じる</summary>
    /// <param name="selectButton">閉じた時に選択される処理</param>
    public void Close(Button selectButton)
    {
        _optionUI.enabled = false;
        selectButton.Select();
    }
}
