using UnityEngine;

/// <summary>シャボン玉銃のクラス </summary>
public class BubbleGun : ShotWeapon
{
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
}
