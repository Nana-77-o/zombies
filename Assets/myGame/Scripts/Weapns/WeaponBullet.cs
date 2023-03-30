using UnityEngine;

/// <summary>�e�ۂ̑���N���X</summary>
public class WeaponBullet : MonoBehaviour
{
    [SerializeField, Header("��������")]
    protected float _lifeTime = 2f;
    [SerializeField, Header("�ړ����x"), Range(0.01f, 1)]
    protected float _speed = 5f;
    [SerializeField, Header("�����蔻����s���܂ł̃t���[����")]
    private int _rayFrame = 5;
    [SerializeField, Header("�Փ˂��郌�C���[")]
    private LayerMask _hitLayer = default;
    [SerializeField, Header("�Փ˔͈�")]
    protected float _radius = 2f;
    [SerializeField, Header("�Փˉ������")]
    protected float _anHitTime = 0f;
    [SerializeField, Header("�Փˉ�")]
    protected int _maxHitCount = 0;
    [SerializeField]
    protected GameObject _effect = default;
    [SerializeField]
    private GameObject[] _attributeEffects = default;
    /// <summary>���݂̃t���[�� </summary>
    private int _frameCount = 0;
    private Damage _damage = default;
    /// <summary>�O�񓖂��蔻����s�����ʒu</summary>
    private Vector3 _beforePos = default;
    protected float _lifeTimer = default;
    protected int _hitCount = 0;
    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, _speed);
        if (_lifeTimer < _anHitTime)
        {
            return;
        }
        HitCheck();
    }

    /// <summary>ray���g�p���������蔻�� </summary>
    private void HitCheck()
    {
        if (gameObject.activeInHierarchy == false || _speed <= 0) { return; }

        _frameCount++;

        if (_frameCount < _rayFrame) { return; }
        _frameCount = 0;

        if (Physics.SphereCast(_beforePos, _radius, transform.forward, out RaycastHit hit, Vector3.Distance(_beforePos, transform.position), _hitLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageApplicable target))     //�G�Ƀ_���[�W��^����
            {
                target.AddDamage(_damage);
            }
            ObjectPoolManager.Instance.Use(_effect).transform.position = transform.position;
            _hitCount--;
            if (_hitCount < 0)
            {
                gameObject.SetActive(false);
            }
        }

        _beforePos = transform.position;
    }

    ///// <summary> �_���[�W��ݒ肵�A�ړ��J�n���� </summary>
    public virtual void StartShot(Damage damage)
    {
        _damage = damage;
        _lifeTimer = 0;
        ActiveEffect();
        gameObject.SetActive(true);
        _hitCount = _maxHitCount;
    }

    /// <summary>�G�������U��</summary>
    /// <param name="arrivalPoint">���e�ʒu<param>
    public virtual void EnemyLongAttack(Damage damage, Vector3 generatePoint, Vector3 arrivalPoint)
    {
        _damage = damage;
        gameObject.SetActive(true);
        _hitCount = _maxHitCount;
    }
    protected void ActiveEffect()
    {
        foreach (var effect in _attributeEffects)
        {
            effect.SetActive(false);
        }
        if ((int)_damage.Attribute >= _attributeEffects.Length)
        {
            return;
        }
        _attributeEffects[(int)_damage.Attribute].SetActive(true);
    }
}
