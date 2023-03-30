using System;
using UnityEngine;

public class BossMissionManager : MonoBehaviour
{
    public static BossMissionManager Instance { get; private set; }
    [SerializeField]
    private CharacterBase _player = default;
    [SerializeField]
    private BossEnemy[] _bossEnemies = default;
    [SerializeField]
    private Transform[] _popPositions = default;
    [SerializeField]
    private BossParamData _bossParamData = default;
    [SerializeField]
    private RandomSpawnPoint[] _missionSpawnPoints = default;
    [SerializeField]
    private SpecialMission _specialMission = default;
    [SerializeField]
    private MissionManager _nomalMission = null;
    [SerializeField]
    private Transform[] _objPopPositions = default;
    [SerializeField, Header("各アイテムの出現確率\n上から継続的スピード,攻撃値, チャージスピード, 残弾数の上限値\n一時的スピード,攻撃値,チャージスピード,残弾数の上限値"), Range(0f, 100f)]
    private float[] _paweUpWeight = default;
    private int _objPopCount = default;
    private float _totalWeight = 0f;
    private PoewrUPType _type = default;
    /// <summary>
    /// プレイヤーデータ
    /// </summary>
    public CharacterBase Player { get => _player; }
    /// <summary>
    /// ボスの数
    /// </summary>
    public int BossCount { get => _bossEnemies.Length; }
    /// <summary>
    /// 現在の特殊ミッション
    /// </summary>
    public SpecialMission SPMission { get => _specialMission; }
    /// <summary>
    /// パワーアップ時のタイプ
    /// </summary>
    public PoewrUPType Type { get => _type; set => _type = value; }

    private void Awake()
    {
        Instance = this;
        for (var i = 0; i < _paweUpWeight.Length; i++)
        {
            _totalWeight += _paweUpWeight[i];
        }
        for (int i = 0; i < _objPopPositions.Length; i++)
        {
            int r = UnityEngine.Random.Range(0, _objPopPositions.Length);
            var pos = _objPopPositions[i];
            _objPopPositions[i] = _objPopPositions[r];
            _objPopPositions[r] = pos;
        }
    }
    private void OnDisable()
    {
        Instance = null;
    }
    private void Start()
    {
        _specialMission.InitializedSpecialMission();
        for (int i = 0; i < _popPositions.Length; i++)
        {
            int r = UnityEngine.Random.Range(0, _popPositions.Length);
            var pos = _popPositions[i];
            _popPositions[i] = _popPositions[r];
            _popPositions[r] = pos;
        }
        for (int i = 0; i < _bossEnemies.Length; i++)
        {
            SetBossParam(i);
            _bossEnemies[i].Body.position = _popPositions[i].position;
        }
        _specialMission.StartMission();
        _nomalMission.StartMission();
    }
    private void ShuffleMissionPoint()
    {
        for (int i = 0; i < _missionSpawnPoints.Length; i++)
        {
            var r = UnityEngine.Random.Range(0, _missionSpawnPoints.Length);
            var point = _missionSpawnPoints[i];
            _missionSpawnPoints[i] = _missionSpawnPoints[r];
            _missionSpawnPoints[r] = point;
        }
    }
    /// <summary>
    /// 指定の敵を指定数ポップさせる
    /// </summary>
    /// <param name="popEnemy"></param>
    /// <param name="popCount"></param>
    public void PopMissionEnemys(GameObject popEnemy,int popCount)
    {
        ShuffleMissionPoint();
        int pointNum = 0;
        for (int i = 0; i < popCount; i++)
        {
            var enemy = ObjectPoolManager.Instance.Use(popEnemy);
            if (pointNum + i >= _missionSpawnPoints.Length)
            {
                pointNum -= _missionSpawnPoints.Length;
            }
            enemy.transform.position = _missionSpawnPoints[pointNum + i].GetSpawnPos();
        }
    }
    /// <summary>
    /// 指定ボスのパラメータをセットアップする
    /// </summary>
    /// <param name="targetNum"></param>
    public void SetBossParam(int targetNum)
    {
        int[] rank = _bossParamData.GetParamRank();
        CharaParamData paramData = _bossParamData.GetBossParam(rank);
        _bossEnemies[targetNum].SetParamRank(rank);
        _bossEnemies[targetNum].SetParamData(paramData);
    }
    /// <summary>
    /// 指定ボスの弱体化
    /// </summary>
    /// <param name="targetNum"></param>
    public void UndermineBoss(int targetNum)
    {
        _bossEnemies[targetNum].UpdateParam(_bossParamData.ParamRankD);
    }
    public void PopObj(GameObject touchObject,int count)
    {
        if (count + _objPopCount > _objPopPositions.Length)
        {
            Debug.Log($"オブジェクトの出現場所数より出現数の設定が多いです！！");
            return;
        }        
        for (int c = _objPopCount; c < count; c++)
        {
            var obj = ObjectPoolManager.Instance.Use(touchObject);
            obj.transform.position = _objPopPositions[c].position;
            obj.GetComponent<TouchObject>().Player = _player.gameObject;
        }
        _objPopCount += count;
    }

    public void TypeSelect()
    {
        Type = (PoewrUPType)Enum.ToObject(typeof(PoewrUPType), PramProbability());
       
    }
    /// <summary>
    /// 確率計算
    /// </summary>
    int PramProbability()
    {
        var randomPoint = UnityEngine.Random.Range(0, _totalWeight);

        // 乱数値が属する要素を先頭から順に選択
        var currentWeight = 0f;
        for (var i = 0; i < _paweUpWeight.Length; i++)
        {
            // 現在要素までの重みの総和を求める
            currentWeight += _paweUpWeight[i];

            // 乱数値が現在要素の範囲内かチェック
            if (randomPoint < currentWeight)
            {
                return i;
            }
        }
        // 乱数値が重みの総和以上なら末尾要素とする
        return _paweUpWeight.Length - 1;
    }
    public enum PoewrUPType
    {
        AlwaysSpeed,
        AlwaysAttack,
        AlwaysChargeSpeed,
        AlwaysChargCount,
        TemporarySpeed,
        TemporaryAttack,
        TemporaryChargeSpeed,
        TemporaryChargCount,
    }
}
