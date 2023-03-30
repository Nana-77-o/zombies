using UnityEngine;
using UnityEngine.UI;

/// <summary>����UI�ŕ\������摜�𑀍삷��N���X </summary>
public class AchievementController : MonoBehaviour
{
    [SerializeField, Header("�I���t���[���F")]
    Color _selectFrameColor = Color.yellow;
    [SerializeField, Header("���I���t���[���F")]
    Color _noSelectFrameColor = Color.red;

    [SerializeField, Tooltip("���уX�v���C�g")]
    Image _achievementImage = default;
    [SerializeField, Tooltip("�t���[��")]
    Image _frameImage = default;
    [SerializeField, Tooltip("���т��B���摜")]
    Image _hideImage = default;
    [SerializeField, Tooltip("���щ����̃e�L�X�g")]
    Text _conditionText = default;
    [SerializeField, Header("ID")]
    int _id = 0;

    /// <summary>�J���Ă��邩(�擾)�ǂ��� </summary>
    bool _isOpen = true;

    /// <summary>ID</summary>
    public int ID { get => _id; }

    /// <summary>�J���Ă��邩(�擾)�ǂ��� </summary>
    public bool IsOpen
    {
        get
        {
            SelectAchievement();    //�t���[���̐F��ύX����
            return _isOpen;
        }
    }

    /// <summary>���уX�v���C�g </summary>
    public Image AchievementImage { get => _achievementImage; }

    /// <summary>���у��X�g��UI���J�������̏��� </summary>
    public void OpenAchievementUI(bool isOpen, string conditionText)
    {
        _isOpen = isOpen;
        var selectColor = Color.white;
        selectColor.a = 0;

        if (isOpen)     //���т��擾���Ă���
        {
            _hideImage.color = selectColor;
            _conditionText.color = selectColor;
            _frameImage.color = _noSelectFrameColor;
            return;
        }

        _frameImage.color = Color.white;
        _conditionText.text = conditionText;
    }

    /// <summary>���ёI�����̏��� </summary>
    public void SelectAchievement()
    {
        _frameImage.color = _selectFrameColor;
    }

    /// <summary>���т��痣�ꂽ���̏��� </summary>
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
