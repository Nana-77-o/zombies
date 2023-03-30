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
    [Header("Sランクパラメータ")]
    [SerializeField]
    private CharaParamData _paramRankS = default;
    [Header("Aランクパラメータ")]
    [SerializeField]
    private CharaParamData _paramRankA = default;
    [Header("Bランクパラメータ")]
    [SerializeField]
    private CharaParamData _paramRankB = default;
    [Header("Cランクパラメータ")]
    [SerializeField]
    private CharaParamData _paramRankC = default;
    [Header("Dランクパラメータ")]
    [SerializeField]
    private CharaParamData _paramRankD = default;
    [Header("Sランクパラメータの最小数")]
    [SerializeField]
    private int _rankSMin = 1;
    [Header("Sランクパラメータの最大数")]
    [SerializeField]
    private int _rankSMax = 2;
    [Header("Aランクパラメータの最小数")]
    [SerializeField]
    private int _rankAMin = 4;
    [Header("Aランクパラメータの最大数")]
    [SerializeField]
    private int _rankAMax = 5;
    [Header("Cランクパラメータの最小数")]
    [SerializeField]
    private int _rankCMin = 2;
    [Header("Cランクパラメータの最大数")]
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
    /// パラメータランクをランダムに振り分ける
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
    /// パラメータランクの配列を返す
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
    /// ボスのパラメータを返す
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
