using UnityEngine;

/// <summary> �n�� </summary>
public class Mine : WeaponBase
{
    [SerializeField, Header("�U���̒��S")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("�U�����a")]
    float _attackRadius = 1f;
    [SerializeField, Tooltip("�A�j���[�V����")]
    Animation _spearAnim;
    [SerializeField, Header("�����G�t�F�N�g")]
    GameObject[] _explosionEffects = default;
    [SerializeField]
    private LayerMask _hitLayer = default;

    bool _isExplosion = false;
    private Damage _exDamage = default;
    private void LateUpdate()
    {
        if (_isExplosion)
        {
            gameObject.SetActive(false);
            _isExplosion = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageApplicable target) && !other.TryGetComponent(out PlayerMoveController playerMove))
        {
            Attack();
        }
    }

    public void StartShot(Damage damage)
    {
        _exDamage = damage;
        ActiveEffect();
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

    public override void Attack()
    {
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius, _hitLayer);   //���������I�u�W�F�N�g��S�Ď擾����

        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(_exDamage);
                }
            }
        }
        _isExplosion = true;
        ObjectPoolManager.Instance.Use(_explosionEffects[(int)_exDamage.Attribute]).transform.position = transform.position;
    }
    protected override void ActiveEffect()
    {
        foreach (var effect in _attributeEffects)
        {
            effect.SetActive(false);
        }
        _attributeEffects[(int)_exDamage.Attribute].SetActive(true);
    }
}
