using UnityEngine;
using UnityEngine.UI;

public class ChargeGun : MonoBehaviour
{
    /// <summary>武器イラストの標準サイズ</summary>
    const float WEAPON_IMAGE_STANDARD_SIZE = 80f;
    /// <summary>マシンガン（シャボン玉）イラストサイズ</summary>
    const float MACHINEGUN_IMAGE_SIZE = 60f;
    /// <summary>ランスの回転値</summary>
    const float LANCE_ROTATEION_VALUE = 50f;

    [SerializeField, Header("チャージのImage")]
    Image[] _chargeImage = default;
    [SerializeField, Header("武器イラストを表示するImage")]
    Image[] _weaponImage = default;
    [SerializeField, Header("各武器のイラスト"), Tooltip("0=ナイフ 1=ランス 2=地雷 3=バブル")]
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

        targetImage.sprite = weaponSprite;     //イラスト変更
    }
}
