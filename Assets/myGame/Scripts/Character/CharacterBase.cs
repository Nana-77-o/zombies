using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター基底クラス
/// </summary>
public class CharacterBase : MonoBehaviour,IDamageApplicable
{
    [SerializeField]
    protected CharaParamData _paramData = default;
    [SerializeField]
    protected int _charaID = 0;
    [SerializeField]
    protected MissionAddCount _addCounter = default;
    protected CharacterParameter _parameter = default;
    protected bool _isInitalize = false;
    protected Damage _damageTaken = null;
    private IEnumerator Start()
    {
        yield return null;
        Initialization();
    }
    protected virtual void Initialization()
    {
        //パラメータ更新時の処理を登録する場合はここで行う
        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _isInitalize = true;
    }

    /// <summary>
    /// 死亡時の処理を行う
    /// </summary>
    protected virtual void DeadCharacter()
    {
        //Enemyの場合はIDを指定し撃破数のカウントを増やす、特定の攻撃で死亡時の処理は派生先で変数を用意し対応すること
        _addCounter.AddCount(_damageTaken, _charaID);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ダメージ処理を行う
    /// </summary>
    /// <param name="damage"></param>
    public virtual void AddDamage(Damage damage)
    {
        if (_isInitalize == false) { return; }
        _damageTaken = damage;
        _addCounter.AddDamageCount(damage);
        //ダメージを受けたときに演出等を行う場合はここに記述すること
        _parameter.CurrentHP -= DamageCalculator.GetDamage(_parameter, damage);
    }
    /// <summary>
    /// パラメータの設定を行う
    /// </summary>
    /// <param name="paramData"></param>
    public void SetParamData(CharaParamData paramData)
    {
        _paramData = paramData;
        Initialization();
    }
    public CharacterParameter GetParameter() { return _parameter; }
}
