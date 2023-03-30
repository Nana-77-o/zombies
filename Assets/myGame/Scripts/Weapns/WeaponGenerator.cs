using UnityEngine;

/// <summary>ホーム画面で選択した武器を生成するクラス </summary>
public class WeaponGenerator : MonoBehaviour
{
    [SerializeField, Header("プレイヤーパラメーター")]
    PlayerParameter _playerParameter = default;
    [SerializeField, Header("プレイヤーアニメーション")]
    PlayerAnim _playerAnim = default;
    [SerializeField, Header("武器切り替えを管理するクラス")]
    PlayerGans _playerGans = default;
    [SerializeField, Header("チャージを行うクラス")]
    ChargeGun _chargeGun = default;
    [SerializeField, Header("武器プレパブ, 0=ナイフ 1=ランス 2=地雷 3=シャボン玉")]
    WeaponBase[] _weapons = default;
    [SerializeField, Header("武器生成位置, 0=ナイフ 1=ランス 2=地雷 3=シャボン玉")]
    Transform[] _generatePoints = default;

    private void Awake()
    {
        GenerateWeapon();
    }

    /// <summary>ホーム画面で選択した武器を生成する</summary>
    void GenerateWeapon()
    {
        for (var i = 0; i < UserData.Data._selectWeapos.Length; i++)
        {
            var weaponInfo = UserData.Data._selectWeapos[i];
            var weapon = Instantiate(_weapons[(int)weaponInfo], transform);
            weapon.transform.position = new Vector3(_generatePoints[(int)weaponInfo].position.x, _generatePoints[(int)weaponInfo].position.y, _generatePoints[(int)weaponInfo].position.z);
            WeaponInitialize(weaponInfo, weapon);
            _playerGans.Weapons[i] = weapon.gameObject;
            _chargeGun.WeaponBase[i] = weapon;
            _chargeGun.WeaponTypes[i] = weaponInfo;
        }
    }

    /// <summary>各武器の初期化処理を行う </summary>
    void WeaponInitialize(WeaponType type, WeaponBase weapon)
    {
        weapon.Character = _playerParameter;

        switch (type)   //アニメーションの設定
        {
            case WeaponType.Lance:
            case WeaponType.Knife:
                weapon.PlayerAnime = _playerAnim;
                break;
        }
    }
}
