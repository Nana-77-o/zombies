using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWeapon : WeaponBase
{
    [SerializeField]
    protected Transform _muzzle = default;
    [SerializeField]
    protected WeaponBullet _bullet = default;
    [SerializeField]
    protected float _shotInterval = 0.5f;
    [SerializeField]
    protected float _diffusivity = 0;
    protected float _attackRadius = 0.5f;
    protected float _shotTimer = default;
    protected bool _isShooting = false;
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

    public override void Attack()
    {
        if (_shotInterval > _shotTimer) { return; }
        if (IsCharge == true || UseCharge() == false)
        {
            return;
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
        var bullet = BulletPool.Instance.GetBullet(_bullet);
        bullet.transform.position = _muzzle.position;
        bullet.transform.forward = Diffusivity(_muzzle.forward, _diffusivity);
        bullet.StartShot(GetDamage());
    }
   
    protected Vector3 Diffusivity(Vector3 target,float diffusivity)
    {
        if (diffusivity > 0)
        {
            target.x += Random.Range(-diffusivity, diffusivity);
            target.y += Random.Range(-diffusivity, diffusivity);
            target.z += Random.Range(-diffusivity, diffusivity);
        }
        return target;
    }
}
