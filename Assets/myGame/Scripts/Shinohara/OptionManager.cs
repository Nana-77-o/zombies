using UnityEngine;
using UnityEngine.UI;

/// <summary>�ݒ�֘A�̏������Ǘ�����N���X</summary>
public class OptionManager : MonoBehaviour
{
    [SerializeField, Tooltip("BGM�̉��ʂ�ύX����X���C�_�[")]
    Slider _bgmSlider = default;
    [SerializeField, Tooltip("BGM�X���C�_�[�̃n���h��")]
    Image _bgmHandleImage = null;
    [SerializeField, Tooltip("BGM�\�L�̃t���[��")]
    Image _bgmFrameImage = null;
    [SerializeField, Tooltip("SE�̉��ʂ�ύX����X���C�_�[")]
    Slider _seSlider = default;
    [SerializeField, Tooltip("SE�X���C�_�[�̃n���h��")]
    Image _seHandleImage = default;
    [SerializeField, Tooltip("SE�\�L�̃t���[��")]
    Image _seFrameImage = default;
    [SerializeField, Tooltip("�N���W�b�g�{�^���̃t���[��")]
    Image _creditFrame = default;
    [SerializeField, Tooltip("�N���W�b�g")]
    Canvas _creditUI = default;
    [SerializeField, Tooltip("�N���W�b�g���J�������ɑI�������{�^��")]
    Button _creditSelectButton = default;
    [SerializeField, Tooltip("�ݒ��ʂ�UI")]
    Canvas _optionUI = default;

    /// <summary>BGM�X���C�_�[�I�����̏��� </summary>
    public void SelectBGMSlider()
    {
        _bgmHandleImage.color = Color.yellow;
        _bgmFrameImage.color = Color.yellow;
    }

    /// <summary>BGM�̉��ʂ�ύX���鏈�� </summary>
    public void ChangeBGMVolume()
    {
        SoundManager.InstanceSound.BGMVolumeSeT(_bgmSlider.value);
    }

    /// <summary>SE�X���C�_�[�I�����̏��� </summary>
    public void SelectSESlider()
    {
        _seHandleImage.color = Color.yellow;
        _seFrameImage.color = Color.yellow;
    }

    /// <summary>SE�̉��ʂ�ύX���鏈�� </summary>
    public void ChangeSEVolume() 
    {
        SoundManager.InstanceSound.SEVolumeSeT(_seSlider.value);
    }

    /// <summary>BGM�X���C�_�[�I���������̏��� </summary>
    public void DeselectBGMSlider()
    {
        _bgmHandleImage.color = Color.white;
        _bgmFrameImage.color = Color.white;
    }
    /// <summary>SE�X���C�_�[�I���������̏��� </summary>
    public void DeselectSESlider()
    {
        _seHandleImage.color = Color.white;
        _seFrameImage.color = Color.white;
    }

    /// <summary>�N���W�b�g�\���{�^����I�����̏��� </summary>
    public void SelectCredit()
    {
        _creditFrame.color = Color.yellow;
    }

    /// <summary>�N���W�b�g�\���{�^����I���������̏��� </summary>
    public void DeslectCredit()
    {
        _creditFrame.color = Color.white;
    }

    /// <summary>�N���W�b�g��\���E��\���𑀍삷��</summary>
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

    /// <summary>�ݒ�UI���J��</summary>
    public void Open()
    {
        _optionUI.enabled = true;
        _bgmSlider.Select();
    }

    /// <summary>�ݒ�UI�����</summary>
    /// <param name="selectButton">�������ɑI������鏈��</param>
    public void Close(Button selectButton)
    {
        _optionUI.enabled = false;
        selectButton.Select();
    }
}
