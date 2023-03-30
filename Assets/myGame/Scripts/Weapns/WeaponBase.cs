using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 武装の基底クラス
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected WeaponParameter _parameter = default;
    [SerializeField]
    private CharacterBase _character = default;
    [SerializeField]
    protected GameObject[] _attributeEffects = default;
    protected PlayerAnim _playerAnime = default;
    protected CharacterParameter _owner = new CharacterParameter();
    protected int _attackCount = default;
    protected float _charge = 1;
    private bool _initialized = false;
    public bool IsCharge { get; protected set; }
    public float CurrentCharge
    {
        get
        {
            return _charge;
        }
    }
    public int CurrentCount
    {
        get
        {
            if (_initialized == false) 
            {
                _parameter.AddCount = 0;
                return _parameter.MaxCount;
            }
            return _attackCount;
        }
    }
    public WeaponParameter WeaponParameter { get => _parameter; }
    public CharacterBase Character { get => _character; set => _character = value; }


    public PlayerAnim PlayerAnime { get => _playerAnime; set => _playerAnime = value; }
    protected bool UseCharge()
    {
        if (IsCharge == true)
        {
            return false;
        }
        _attackCount--;
        if (_attackCount <= 0)
        {
            _charge = 0;
            IsCharge = true;
            return true;
        }
        _charge = (float)_attackCount / _parameter.MaxCount;
        return true;
    }
    public void ChargeImpl()
    {
        if (IsCharge == false)
        {
            return;
        }
        _charge += 1 / _parameter.ChargeTime * (1 + _owner.AttackSpeed) * Time.deltaTime;
        if (_charge < 1)
        {
            return;
        }
        _charge = 1;
        _attackCount = _parameter.MaxCount;
        IsCharge = false;
    }
    /// <summary>
    /// 所有者を設定し、初期化する
    /// </summary>
    /// <param name="owner"></param>
    public virtual void Initialize(CharacterParameter owner)
    {
        _owner = owner;
        _parameter.AddCount = 0;
        _attackCount = _parameter.MaxCount;
        _initialized = true;
    }
    /// <summary>
    /// この武器のダメージを返す
    /// </summary>
    /// <returns></returns>
    protected virtual Damage GetDamage()
    {
        //Ownerのパラメータ変動でダメージが変動する為newする
        return new Damage(_owner, _parameter, (AttributeType)PlayerGans.SelectedIndex);
    }
    /// <summary>
    /// 攻撃を行う
    /// </summary>
    public abstract void Attack();
    protected virtual void ActiveEffect()
    {
        foreach (var effect in _attributeEffects)
        {
            effect.SetActive(false);
        }
        if (PlayerGans.SelectedIndex >= _attributeEffects.Length)
        {
            return;
        }
        _attributeEffects[PlayerGans.SelectedIndex].SetActive(true);
    }
}
