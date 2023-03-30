using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[���N���X
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
        //�p�����[�^�X�V���̏�����o�^����ꍇ�͂����ōs��
        _parameter = new CharacterParameter(_paramData);
        _parameter.DelDead += DeadCharacter;
        _isInitalize = true;
    }

    /// <summary>
    /// ���S���̏������s��
    /// </summary>
    protected virtual void DeadCharacter()
    {
        //Enemy�̏ꍇ��ID���w�肵���j���̃J�E���g�𑝂₷�A����̍U���Ŏ��S���̏����͔h����ŕϐ���p�ӂ��Ή����邱��
        _addCounter.AddCount(_damageTaken, _charaID);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �_���[�W�������s��
    /// </summary>
    /// <param name="damage"></param>
    public virtual void AddDamage(Damage damage)
    {
        if (_isInitalize == false) { return; }
        _damageTaken = damage;
        _addCounter.AddDamageCount(damage);
        //�_���[�W���󂯂��Ƃ��ɉ��o�����s���ꍇ�͂����ɋL�q���邱��
        _parameter.CurrentHP -= DamageCalculator.GetDamage(_parameter, damage);
    }
    /// <summary>
    /// �p�����[�^�̐ݒ���s��
    /// </summary>
    /// <param name="paramData"></param>
    public void SetParamData(CharaParamData paramData)
    {
        _paramData = paramData;
        Initialization();
    }
    public CharacterParameter GetParameter() { return _parameter; }
}
