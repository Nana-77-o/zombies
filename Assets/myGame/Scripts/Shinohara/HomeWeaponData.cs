using UnityEngine;
using UnityEngine.UI;

/// <summary>武器選択のボタンがもつ情報を保持するクラス </summary>
public class HomeWeaponData : MonoBehaviour
{
    /// <summary>ランスの回転値</summary>
    readonly Quaternion LANCE_RORATEION = Quaternion.Euler(0, 0, 50f);
    /// <summary>武器イラストの標準サイズ</summary>
    readonly Vector2 STANDARD_SIZE = new Vector2(80f, 80f);
    /// <summary>マシンガン（シャボン玉）イラストサイズ</summary>
    readonly Vector2 MACHINEGUN_IMAGE_SIZE = new Vector2(60f, 60f);

    [SerializeField, Header("武器タイプ")]
    WeaponType _weaponType = WeaponType.Knife;
    [SerializeField, Header("属性")]
    AttributeType _attributeType = AttributeType.Wut;
    [SerializeField, Header("武器のスプライト")]
    Sprite _weaponSprite = default;
    [SerializeField, Header("武器イラストを表示させる為のImage")]
    Image _weaponImage = default;
    [SerializeField, Header("フレーム(枠)")]
    Image _weaponFrame = default;
    [SerializeField, Header("武器選択時に戻るボタン")]
    Button _attributeBackButton = default;
    [SerializeField, Header("現在の属性ボタン(自分の属性)")]
    Animator _currentAttributeAnimator = default;

    /// <summary>武器の種類 </summary>
    public WeaponType Weapon { get => _weaponType; }

    /// <summary>属性 </summary>
    public AttributeType Attribute { get => _attributeType; }

    /// <summary>武器のスプライト </summary>
    public Sprite WeaponSprite { get => _weaponSprite; }

    /// <summary>武器選択時に戻るボタン</summary>
    public Button AttributeBackButton { get => _attributeBackButton; }

    /// <summary>現在の属性ボタン(自分の属性) </summary>
    public Animator CurrentAttributeAnimator { get => _currentAttributeAnimator; }

    /// <summary>武器の選択・選択解除にUIの色を変更する </summary>
    /// <param name="deSelectColor">選択されていない時の色</param>
    public void ChangeWeaponImageColor(SelectState state, Color deSelectColor)
    {
        if (state == SelectState.select)
        {
            _weaponImage.color = new Color(1f, 1f, 1f, 1f);    //武器イラストの色を変更する
            _weaponFrame.color = new Color(1f, 1f, 1f, 1f);   //フレームの色を変更する
        }
        else
        {
            _weaponImage.color = deSelectColor;
            _weaponFrame.color = deSelectColor;
        }
    }

    /// <summary>
    /// 武器選択時にイラストを変更する 
    /// 武器によって大きさや回転値を変更
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

        targetImage.sprite = _weaponSprite;     //イラスト変更
    }
}
