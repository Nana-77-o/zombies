using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルトに使用する不変のデータ群
/// </summary>
public class ResultImmutableData : MonoBehaviour
{
    [Tooltip("討伐数に関するデータ"), SerializeField]
    private Degree[] _killScoreData = default;
    [Tooltip("救出数に関するデータ"), SerializeField]
    private Degree[] _helpScoreData = default;
    [Tooltip("合計スコア（階級）に関するデータ"), SerializeField]
    private Degree[] _totalScoreData = default;
    [Tooltip("敵のID, イメージの組み合わせデータ"), SerializeField]
    private ResultEnemyData[] _enemyData = default;

    public Degree[] KillScoreData => _killScoreData;
    public Degree[] HelpScoreData => _helpScoreData;
    public Degree[] TotalScoreData => _totalScoreData;
    public ResultEnemyData[] EnemyData => _enemyData;

    [SerializeField]
    private bool _isUseTestValue = false;

    public const int HelpIndex = 12;

    public int GetRankIndex(int score, ScoreType type)
    {
        Degree[] degrees = null;
        switch (type)
        {
            case ScoreType.Kill:
                degrees = _killScoreData;
                break;
            case ScoreType.Help:
                degrees = _helpScoreData;
                break;
            case ScoreType.Total:
                degrees = _totalScoreData;
                break;
        }

        for (int i = 0; i < degrees.Length; i++)
        {
            if (degrees[i]._needScore > score)
            {
                if (i == 0) return i;
                else return i - 1;
            }
        }
        if (degrees[degrees.Length - 1]._needScore <= score)
        {
            return degrees.Length - 1;
        }
        Debug.LogError("指定されたインデックスの取得に失敗しました。");

        return -1;
    }
    public int GetScoreIndex(ScoreType scoreType)
    {
        // Dictionaryにキーが存在するとき かつ, 本番用の値を使用するとき
        if (!_isUseTestValue)
        {
            if (scoreType == ScoreType.Kill) // 倒した数の場合は異なる処理を実行する
            {
                return GetRankIndex(GetKillScore(), scoreType);
            }
            else // 救出数の場合
            {
                if (GameData.Instance.CountData.TryGetValue(HelpIndex, out int value))
                {
                    return GetRankIndex(value, scoreType);
                }
                else // キーが存在しない場合は0を返す。
                {
                    return 0;
                }
            }
        }
        else if (_isUseTestValue) // テスト値を使用するとき
        {
            int testValue = 0;
            switch (scoreType)
            {
                case ScoreType.Kill: testValue = TestVariable._testKillNumber; break;
                case ScoreType.Help: testValue = TestVariable._testHelpNumber; break;
                default: Debug.LogError("想定してない値が渡されました。"); break;
            }
            return GetRankIndex(testValue, scoreType);
        }
        else // キーが存在しないときは0を返す。
        {
            return 0;
        }
    }
    public int GetScoreValue(ScoreType scoreType, int dataIndex)
    {
        if (scoreType == ScoreType.Kill && !_isUseTestValue)
        {
            return GetKillScore();
        }
        if (GameData.Instance.CountData.ContainsKey(dataIndex) && !_isUseTestValue)
            return GameData.Instance.CountData[dataIndex];
        else if (_isUseTestValue)
        {
            switch (scoreType)
            {
                case ScoreType.Kill: return TestVariable._testKillNumber;
                case ScoreType.Help: return TestVariable._testHelpNumber;
                default: Debug.LogError("想定してない値が渡されました。"); break;
            }
        }
        return 0;
    }
    /// <summary>
    /// 倒した敵の総数を取得する
    /// </summary>
    public static int GetKillScore()
    {
        int result = 0;

        // 各エネミーに割り当てられたIDを配列として取得する。
        int[] _enemyID =
           {
             18, 19, 20, 21, 22, 23, 24, 25,
             26, 27, 28, 29, 30, 31, 32, 33,
             34, 35, 36, 37, 38, 39
           };

        for (int i = 0; i < _enemyID.Length; i++)
        {
            // 指定の値が範囲であれば、
            if (GameData.Instance.CountData.TryGetValue(_enemyID[i], out int value))
            {
                result += value;
            }
            else // 倒してない場合、あるいは間違った値が指定された場合の処理
            {
                // Debug.LogWarning($"存在しないキーが指定されました。");
            }
        }
        return result;
    }
    /// <summary>
    /// 倒した敵の総数分のシャッフル済みの画像配列を取得する。
    /// </summary>
    /// <returns> シャッフル済みの画像配列 </returns>
    public Sprite[] GetKillScoreShuffleSprites()
    {
        // 配列のセットアップ（インスタンスの確保とスプライトの割り当て）
        int counter = 0;
        // 配列のインスタンス
        Sprite[] result = null;

        if (_isUseTestValue) // テスト用の値を使用する場合
        {
            // テスト用の倒した数を取得
            int remainingNumber = TestVariable._testKillNumber;
            // 配列のインスタンスを確保
            result = new Sprite[remainingNumber];

            for (int i = 0; i < _enemyData.Length; i++)
            {
                // 倒した敵の数（ランダムな値）を取得する
                var score = 0; // 倒した数を保存する値
                if (remainingNumber <= 0)
                {
                    // のこり数がなくなったら中断する
                    break;
                }

                if (i == _enemyData.Length - 1) // 最後の要素の場合
                {
                    score = remainingNumber; // スコアを残り数にする
                }
                else
                {
                    score = Random.Range(1, remainingNumber / 2); // ランダムなスコアを割り当てる。（範囲は 1体～残りの総数の半分）
                    remainingNumber -= score;
                    Debug.Log(score);
                }

                // スプライトを割り当てる
                for (int j = 0; j < score; j++, counter++)
                {
                    result[counter] = _enemyData[i]._sprite;
                }
            }
        }
        else // 本番用の値を使用する場合
        {
            // 本番用の倒した数を取得する
            var num = GetKillScore();
            // 配列のインスタンスを確保する
            result = new Sprite[num];
            // 各ゾンビに対応する画像を割り当てる
            for (int i = 0; i < _enemyData.Length; i++)
            {
                // 指定のIDが存在する場合のみ処理を実行する
                if (GameData.Instance.CountData.ContainsKey(_enemyData[i]._id) == false)
                {
                    continue;
                }
                for (int j = 0; j < GameData.Instance.CountData[_enemyData[i]._id]; j++, counter++)
                {
                    result[counter] = _enemyData[i]._sprite;
                }
            }
        }
        // 配列の要素をシャッフルする
        System.Random rng = new System.Random();
        int n = result.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var tmp = result[k];
            result[k] = result[n];
            result[n] = tmp;
        }

        return result;
    }
}
/// <summary>
/// 敵のIDと画像の組み合わせ
/// </summary>
[System.Serializable]
public struct ResultEnemyData
{
    public int _id;
    public Sprite _sprite;
}
public enum ScoreType
{
    Kill,
    Help,
    Total
}