using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossParam")]
public class BossParamData : ScriptableObject
{
    public const int PARAM_NUMBER = 13;
    public enum RankID
    {
        S = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
    }
    #region SerializeField
    [Header("S�����N�p�����[�^")]
    [SerializeField]
    private CharaParamData _paramRankS = default;
    [Header("A�����N�p�����[�^")]
    [SerializeField]
    private CharaParamData _paramRankA = default;
    [Header("B�����N�p�����[�^")]
    [SerializeField]
    private CharaParamData _paramRankB = default;
    [Header("C�����N�p�����[�^")]
    [SerializeField]
    private CharaParamData _paramRankC = default;
    [Header("D�����N�p�����[�^")]
    [SerializeField]
    private CharaParamData _paramRankD = default;
    [Header("S�����N�p�����[�^�̍ŏ���")]
    [SerializeField]
    private int _rankSMin = 1;
    [Header("S�����N�p�����[�^�̍ő吔")]
    [SerializeField]
    private int _rankSMax = 2;
    [Header("A�����N�p�����[�^�̍ŏ���")]
    [SerializeField]
    private int _rankAMin = 4;
    [Header("A�����N�p�����[�^�̍ő吔")]
    [SerializeField]
    private int _rankAMax = 5;
    [Header("C�����N�p�����[�^�̍ŏ���")]
    [SerializeField]
    private int _rankCMin = 2;
    [Header("C�����N�p�����[�^�̍ő吔")]
    [SerializeField]
    private int _rankCMax = 3;
    #endregion
    #region Property
    public CharaParamData ParamRankS { get => _paramRankS; }
    public CharaParamData ParamRankA { get => _paramRankA; }
    public CharaParamData ParamRankB { get => _paramRankB; }
    public CharaParamData ParamRankC { get => _paramRankC; }
    public CharaParamData ParamRankD { get => _paramRankD; }
    #endregion
    private int[] _ranckIndexList = new int[PARAM_NUMBER];
    /// <summary>
    /// �p�����[�^�����N�������_���ɐU�蕪����
    /// </summary>
    private void ShuffleParamRank()
    {
        int sRank = Random.Range(_rankSMin - 1, _rankSMax);
        int aRank = Random.Range(_rankAMin - 1, _rankAMax);
        int cRank = Random.Range(_rankCMin - 1, _rankCMax);
        int rankNum = (int)RankID.S;
        for (int i = 0; i < PARAM_NUMBER; i++)
        {
            if (i > sRank + aRank + cRank)
            {
                rankNum = (int)RankID.B;
            }
            else if (i > sRank + aRank)
            {
                rankNum = (int)RankID.C;
            }
            else if (i > sRank)
            {
                rankNum = (int)RankID.A;
            }
            _ranckIndexList[i] = rankNum;
        }
        for (int k = 0; k < PARAM_NUMBER; k++)
        {
            int r = Random.Range(0, PARAM_NUMBER);
            int vaf = _ranckIndexList[k];
            _ranckIndexList[k] = _ranckIndexList[r];
            _ranckIndexList[r] = vaf;
        }
    }
    /// <summary>
    /// �p�����[�^�����N�̔z���Ԃ�
    /// </summary>
    /// <returns></returns>
    public int[] GetParamRank()
    {
        int[] rank = new int[PARAM_NUMBER];
        ShuffleParamRank();
        for (int i = 0; i < PARAM_NUMBER; i++)
        {
            rank[i] = _ranckIndexList[i];
        }
        return rank;
    }
    /// <summary>
    /// �{�X�̃p�����[�^��Ԃ�
    /// </summary>
    /// <returns></returns>
    public CharaParamData GetBossParam(int[] _ranckIdList)
    {
        CharaParamData bossParam = new CharaParamData();
        CharaParamData[] charaParams = { _paramRankS, _paramRankA, _paramRankB, _paramRankC, _paramRankD };
        for (int i = 0; i < PARAM_NUMBER; i++)
        {
            bossParam[(ParamID)i] = charaParams[_ranckIdList[i]][(ParamID)i];
        }
        return bossParam;
    }
}
