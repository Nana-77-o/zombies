using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttackWeapon : ShotWeapon
{
    [SerializeField, Header("生成オブジェクト")]
    EnemyLongRangeAttack _attackObject = default;
    [SerializeField]
    protected float _range = 10f;
    [SerializeField]
    private Animator _animator = default;
    [SerializeField]
    private string _animeName = "Throw";
    private void OnEnable()
    {
        ActiveEffect();
        PlayerInput.SetStayInput(InputType.Fire1, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftStayInput(InputType.Fire1, this.Attack);
    }

    private void LateUpdate()
    {
        if (_shotInterval > _shotTimer)
        {
            _shotTimer += Time.deltaTime;
        }
        _isShooting = false;
    }

    /// <summary>敵遠距離攻撃を発生させる </summary>
    /// <param name="generatePosition">生成位置</param>
    /// /// <param name="arrivalPosition">着弾位置</param>
    public void GenerateAttack(Vector3 generatePosition, Vector3 arrivalPosition)
    {
        if (_animator != null)
        {
            _animator.Play(_animeName);
        }
        var go = BulletPool.Instance.GetBullet(_attackObject);
        go.EnemyLongAttack(GetDamage(), generatePosition, arrivalPosition);
    }

    public override void Attack()
    {
        if (_shotInterval > _shotTimer) { return; }
        if (IsCharge == true || UseCharge() == false)
        {
            return;
        }
        if (_playerAnime != null)
        {
            _playerAnime.Attack();
        }
        _isShooting = true;
        _shotTimer = 0;
        var hitObjects = Physics.OverlapSphere(_muzzle.position, _attackRadius);
        if (0 < hitObjects.Length)
        {
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(GetDamage());
                }
            }
        }
        GenerateAttack(_muzzle.position, Diffusivity(_muzzle.position + _muzzle.forward * _range));
    }
    protected Vector3 Diffusivity(Vector3 target)
    {
        if (_diffusivity > 0)
        {
            target.x += Random.Range(-_diffusivity, _diffusivity);
            target.y += Random.Range(-_diffusivity, _diffusivity);
            target.z += Random.Range(-_diffusivity, _diffusivity);
        }
        return target;
    }
}
