using UnityEngine;
using UnityEngine.UI;

public class ChargeGun : MonoBehaviour
{
    /// <summary>����C���X�g�̕W���T�C�Y</summary>
    const float WEAPON_IMAGE_STANDARD_SIZE = 80f;
    /// <summary>�}�V���K���i�V���{���ʁj�C���X�g�T�C�Y</summary>
    const float MACHINEGUN_IMAGE_SIZE = 60f;
    /// <summary>�����X�̉�]�l</summary>
    const float LANCE_ROTATEION_VALUE = 50f;

    [SerializeField, Header("�`���[�W��Image")]
    Image[] _chargeImage = default;
    [SerializeField, Header("����C���X�g��\������Image")]
    Image[] _weaponImage = default;
    [SerializeField, Header("�e����̃C���X�g"), Tooltip("0=�i�C�t 1=�����X 2=�n�� 3=�o�u��")]
    Sprite[] _weaponSprites = default;
    [SerializeField]
    Text[] _countTexts = default;
    [SerializeField]
    Color _idleColor = default;
    [SerializeField]
    Color _chargeColor = default;

    WeaponBase[] _weaponBase = new WeaponBase[4];

    WeaponType[] _weaponTypes = new WeaponType[4];
    public WeaponBase[] WeaponBase { get => _weaponBase; set => _weaponBase = value; }
    public WeaponType[] WeaponTypes { get => _weaponTypes; set => _weaponTypes = value; }
    public Image[] WeaponImage { get => _weaponImage; set => _weaponImage = value; }

    private void Start()
    {
        for (var i = 0; i < _weaponBase.Length; i++)
        {
            ChangeWeaponSprite(_weaponTypes[i], WeaponImage[i]);
        }
    }

    void Update()
    {
        for(int i = 0; i < WeaponBase.Length; i++)
        {
            _chargeImage[i].fillAmount = WeaponBase[i].CurrentCharge;
            ChangeWeaponSprite(_weaponTypes[i], WeaponImage[i]);
            WeaponBase[i].ChargeImpl();
            if (WeaponBase[i].IsCharge == true)
            {
                _chargeImage[i].color = _chargeColor;
            }
            else
            {
                _chargeImage[i].color = _idleColor;
            }
            if (_countTexts != null || _countTexts.Length > 0)
            {
                _countTexts[i].text = $"{WeaponBase[i].CurrentCount}/{WeaponBase[i].WeaponParameter.MaxCount}";
            }
        }
    }

    public void ChangeWeaponSprite(WeaponType type, Image targetImage)
    {
        var weaponSprite = targetImage.sprite;

        switch (type)
        {
            case WeaponType.Knife:
                targetImage.rectTransform.sizeDelta = new Vector2(WEAPON_IMAGE_STANDARD_SIZE, WEAPON_IMAGE_STANDARD_SIZE);
                targetImage.rectTransform.rotation = Quaternion.identity;
                weaponSprite = _weaponSprites[0];
                break;
            case WeaponType.Lance:
                targetImage.rectTransform.sizeDelta = new Vector2(WEAPON_IMAGE_STANDARD_SIZE, WEAPON_IMAGE_STANDARD_SIZE);
                targetImage.rectTransform.rotation = Quaternion.Euler(0, 0, LANCE_ROTATEION_VALUE);
                weaponSprite = _weaponSprites[1];
                break;
            case WeaponType.Mine:
                targetImage.rectTransform.sizeDelta = new Vector2(WEAPON_IMAGE_STANDARD_SIZE, WEAPON_IMAGE_STANDARD_SIZE);
                targetImage.rectTransform.rotation = Quaternion.identity;
                weaponSprite = _weaponSprites[2];
                break;
            case WeaponType.MachineGun:
                targetImage.rectTransform.sizeDelta = new Vector2(MACHINEGUN_IMAGE_SIZE, MACHINEGUN_IMAGE_SIZE);
                targetImage.rectTransform.rotation = Quaternion.identity;
                weaponSprite = _weaponSprites[3];
                break;
        }

        targetImage.sprite = weaponSprite;     //�C���X�g�ύX
    }
}
