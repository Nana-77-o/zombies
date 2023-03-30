using UnityEngine;

/// <summary>����@�i�C�t </summary>
public class KnifeWeapon : WeaponBase
{
    [SerializeField, Header("�U���̒��S")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("�U�����a")]
    float _attackRadius = 5f;
    [SerializeField, Header("�U���Ԋu")]
    float _attackInterval = 0.5f;

    float _attackTimer = 0f;

    private void OnEnable()
    {
        PlayerInput.SetEnterInput(InputType.Fire2, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire2, this.Attack);
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
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //���������I�u�W�F�N�g��S�Ď擾����

        if (0 < hitObjects.Length)
        {
           
            //�G��݂̂̂ɍU�����邽�߂�break���Ă��܂�
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(GetDamage());
                    break;
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
