using UnityEngine;

/// <summary>�o�u���e�Ŕ��˂���e�ۂ̃N���X </summary>
public class BubbleBullet : WeaponBullet
{
    [SerializeField, Header("�U���̒��S")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("�U�����a")]
    float _attackRadius = 1f;
    [SerializeField, Header("���e���̃G�t�F�N�g")] 
    ParticleSystem _explosionEffect = default;
    /// <summary>�G�ł͂Ȃ������Ώۂɒ��e�������ǂ��� </summary>
    bool _isHit = false;
    /// <summary>�G�ɒ��e�������ǂ��� </summary>
    bool _isEnemyHit = false;
    Damage _damage = default;

    private void Update()
    {
        if (_explosionEffect.isStopped && _isHit)   //�����G�t�F�N�g���I��������A�N�e�B�u��Ԃ�false�ɂ���
        {
            var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //�������ɏՓ˂����G�Ƀ_���[�W��^����

            if (0 < hitObjects.Length)
            {
                foreach (var obj in hitObjects)
                {
                    if (obj.TryGetComponent(out IDamageApplicable target))  //�G�ɒ��e
                    {
                        target.AddDamage(_damage);
                        break;
                    }
                }
            }

            _isHit = false;
            gameObject.SetActive(false);
        }

        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //���������I�u�W�F�N�g��S�Ď擾����

        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))  //�G�ɒ��e
                {
                    target.AddDamage(_damage);
                    _isEnemyHit = true;
                }

                if (!obj.TryGetComponent(out BubbleBullet bullet))�@//���̒e�ۈȊO�ɒ��e
                {
                    _explosionEffect.Play();
                    _isHit = true;
                }
            }

            if (_isEnemyHit)
            {
                _isEnemyHit = false;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected() //�U���͈͂̃M�Y����\������
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenter(), _attackRadius);
    }

    public override void StartShot(Damage damage)
    {
        _damage = damage;
        base.StartShot(damage);
    }

    Vector3 GetCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackCenter.z
           + this.transform.up * _attackCenter.y
           + this.transform.right * _attackCenter.x;
        return center;
    }
}
