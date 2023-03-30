using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField, Header("UŒ‚ˆÊ’u")]
    private Transform _attackCenter = default;
    [SerializeField, Header("UŒ‚”¼Œa")]
    private float _attackRadius = 5f;
    [SerializeField, Header("UŒ‚ƒf[ƒ^")]
    private WeaponParameter _enemyAttackParameter;
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    private Damage _damage = default;
    public float AttackRadius => _attackRadius;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackCenter.position, _attackRadius / 2);
    }
    public void Attack()
    {
        if (_damage == null) { return; }
        if (Vector3.Distance(_attackCenter.position, NavigationManager.Instance.DefaultTarget.position) <= _attackRadius)
        {
            if(NavigationManager.Instance.DefaultTarget.gameObject.TryGetComponent(out IDamageApplicable player))
            {
                player.AddDamage(_damage);
                _hitEffect?.Play();
            }
        }
    }
    public void SetDamage(CharacterParameter parameter)
    {
        _damage = new Damage(parameter, _enemyAttackParameter);
    }
}
