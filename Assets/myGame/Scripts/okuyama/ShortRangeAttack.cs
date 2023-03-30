using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����
/// </summary>
public class ShortRangeAttack : WeaponBase
{
    [SerializeField, Tooltip("�A�j���[�V����")]
    Animation _spearAnim;
    [SerializeField, Tooltip("�_���[�W�N���X")]
    Damage _damage = default;
    [SerializeField, Tooltip("�C���^�[�o��")]
    float _interval = 0.5f;
    [Tooltip("�C���^�[�o��bool")]
    bool _intervalBool = false;

    private void OnEnable()
    {
        PlayerInput.SetEnterInput(InputType.Fire1,this.Attack);
    }

    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire1, this.Attack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IDamageApplicable target))
        {
            target.AddDamage(_damage);
        }
    }

    public override void Attack()
    {
        if(_intervalBool) { return; }
        StartCoroutine(Interval());
    }

    public void StartShot(Damage damage)
    {
        //_spearAnim.Play();
        _damage = damage;
    }

    IEnumerator Interval()
    {
       // _playerGans.ChargeMax(_chargeConsumption);
        _intervalBool = true; 
        yield return new WaitForSeconds(_interval);
        _intervalBool = false;
        StartShot(GetDamage());
    }
}
