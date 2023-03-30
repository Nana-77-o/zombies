using UnityEngine;
using UnityEngine.UI;

/// <summary>����I���̃{�^����������ێ�����N���X </summary>
public class HomeWeaponData : MonoBehaviour
{
    /// <summary>�����X�̉�]�l</summary>
    readonly Quaternion LANCE_RORATEION = Quaternion.Euler(0, 0, 50f);
    /// <summary>����C���X�g�̕W���T�C�Y</summary>
    readonly Vector2 STANDARD_SIZE = new Vector2(80f, 80f);
    /// <summary>�}�V���K���i�V���{���ʁj�C���X�g�T�C�Y</summary>
    readonly Vector2 MACHINEGUN_IMAGE_SIZE = new Vector2(60f, 60f);

    [SerializeField, Header("����^�C�v")]
    WeaponType _weaponType = WeaponType.Knife;
    [SerializeField, Header("����")]
    AttributeType _attributeType = AttributeType.Wut;
    [SerializeField, Header("����̃X�v���C�g")]
    Sprite _weaponSprite = default;
    [SerializeField, Header("����C���X�g��\��������ׂ�Image")]
    Image _weaponImage = default;
    [SerializeField, Header("�t���[��(�g)")]
    Image _weaponFrame = default;
    [SerializeField, Header("����I�����ɖ߂�{�^��")]
    Button _attributeBackButton = default;
    [SerializeField, Header("���݂̑����{�^��(�����̑���)")]
    Animator _currentAttributeAnimator = default;

    /// <summary>����̎�� </summary>
    public WeaponType Weapon { get => _weaponType; }

    /// <summary>���� </summary>
    public AttributeType Attribute { get => _attributeType; }

    /// <summary>����̃X�v���C�g </summary>
    public Sprite WeaponSprite { get => _weaponSprite; }

    /// <summary>����I�����ɖ߂�{�^��</summary>
    public Button AttributeBackButton { get => _attributeBackButton; }

    /// <summary>���݂̑����{�^��(�����̑���) </summary>
    public Animator CurrentAttributeAnimator { get => _currentAttributeAnimator; }

    /// <summary>����̑I���E�I��������UI�̐F��ύX���� </summary>
    /// <param name="deSelectColor">�I������Ă��Ȃ����̐F</param>
    public void ChangeWeaponImageColor(SelectState state, Color deSelectColor)
    {
        if (state == SelectState.select)
        {
            _weaponImage.color = new Color(1f, 1f, 1f, 1f);    //����C���X�g�̐F��ύX����
            _weaponFrame.color = new Color(1f, 1f, 1f, 1f);   //�t���[���̐F��ύX����
        }
        else
        {
            _weaponImage.color = deSelectColor;
            _weaponFrame.color = deSelectColor;
        }
    }

    /// <summary>
    /// ����I�����ɃC���X�g��ύX���� 
    /// ����ɂ���đ傫�����]�l��ύX
    /// </summary>
    public void ChangeSprite(Image targetImage)
    {
        switch (_weaponType)
        {
            case WeaponType.Knife:
            case WeaponType.Mine:
                targetImage.rectTransform.sizeDelta = STANDARD_SIZE;
                targetImage.rectTransform.rotation = Quaternion.identity;
                break;
            case WeaponType.Lance:
                targetImage.rectTransform.sizeDelta = STANDARD_SIZE;
                targetImage.rectTransform.rotation = LANCE_RORATEION;
                break;
            case WeaponType.MachineGun:
                targetImage.rectTransform.sizeDelta = MACHINEGUN_IMAGE_SIZE;
                targetImage.rectTransform.rotation = Quaternion.identity;
                break;
        }

        targetImage.sprite = _weaponSprite;     //�C���X�g�ύX
    }
}
