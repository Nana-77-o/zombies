using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// �ߐڍU���p�̃N���X
/// �i�C�t�⃉���X�Ȃ�
/// </summary>
public class MeleeAttackWeapon : WeaponBase
{
    [SerializeField, Header("�U���̒��S")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("�U�����a")]
    float _attackRadius = 1f;
    [SerializeField, Header("�U���C���^�[�o��")]
    float _attackInterval = 0.5f;
    [SerializeField, Header("�܂Ƃ߂čU��")]
    bool _multiAttack = false;
    [SerializeField]
    Animator _attackAnime = default;
    [SerializeField]
    int _hitCount = 1;
    string _attackName = "Attack";
    float _attackTimer = 0f;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        Initialize(Character.GetParameter());
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ActiveEffect();
        PlayerInput.SetEnterInput(InputType.Fire1, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire1, this.Attack);
    }

    private void LateUpdate()
    {
        if (_attackInterval > _attackTimer)
        {
            _attackTimer += Time.deltaTime;
        }
    }

    public override void Attack()
    {
        if (_attackInterval > _attackTimer) { return; }
        if (IsCharge == true || UseCharge() == false)
        {
            return;
        }
        if (_attackAnime != null)
        {
            _attackAnime.Play(_attackName);
        }
        if (_playerAnime != null)
        {
            _playerAnime.Attack();
        }
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //���������I�u�W�F�N�g��S�Ď擾����

        if (0 < hitObjects.Length)
        {
            //�G��݂̂̂ɍU������break
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    for (int i = 0; i < _hitCount; i++)
                    {
                        target.AddDamage(GetDamage());
                    }
                    if (!_multiAttack)
                    {
                        break;
                    }
                }
            }
        }
        _attackTimer = 0;
    }

    private void OnDrawGizmosSelected() //�U���͈͂̃M�Y����\������
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenter(), _attackRadius);
    }

    /// <summary>�U���̒��S���擾����</summary>
    /// <returns>�U���̒��S</returns>
    Vector3 GetCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackCenter.z
           + this.transform.up * _attackCenter.y
           + this.transform.right * _attackCenter.x;
        return center;
    }
}
