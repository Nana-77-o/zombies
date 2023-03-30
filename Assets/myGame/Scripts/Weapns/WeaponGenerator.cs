using UnityEngine;

/// <summary>�z�[����ʂőI����������𐶐�����N���X </summary>
public class WeaponGenerator : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�p�����[�^�[")]
    PlayerParameter _playerParameter = default;
    [SerializeField, Header("�v���C���[�A�j���[�V����")]
    PlayerAnim _playerAnim = default;
    [SerializeField, Header("����؂�ւ����Ǘ�����N���X")]
    PlayerGans _playerGans = default;
    [SerializeField, Header("�`���[�W���s���N���X")]
    ChargeGun _chargeGun = default;
    [SerializeField, Header("����v���p�u, 0=�i�C�t 1=�����X 2=�n�� 3=�V���{����")]
    WeaponBase[] _weapons = default;
    [SerializeField, Header("���퐶���ʒu, 0=�i�C�t 1=�����X 2=�n�� 3=�V���{����")]
    Transform[] _generatePoints = default;

    private void Awake()
    {
        GenerateWeapon();
    }

    /// <summary>�z�[����ʂőI����������𐶐�����</summary>
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

    /// <summary>�e����̏������������s�� </summary>
    void WeaponInitialize(WeaponType type, WeaponBase weapon)
    {
        weapon.Character = _playerParameter;

        switch (type)   //�A�j���[�V�����̐ݒ�
        {
            case WeaponType.Lance:
            case WeaponType.Knife:
                weapon.PlayerAnime = _playerAnim;
                break;
        }
    }
}
