using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;


/// <summary>Json関連の処理を行う</summary>
public class JsonProcessClass : MonoBehaviour
{
    /// <summary>倒した数の実績数</summary>
    const int KILL_ACHIEVEMENR_COUNT = 16;
    /// <summary>救出数の実績数</summary>
    const int HELP_ACHIEVEMENR_COUNT = 11;
    /// <summary>階級の実績数</summary>
    const int RANK_ACHIEVEMENR_COUNT = 20;

    /// <summary1プレイを保存するファイル名 </summary>
    public static readonly string PLAY_DATA_FILE_NAME = "PlayResult.json";
    /// <summary>実績リストを保存するファイル名</summary>
    public static readonly string ACHIEVEMENT_FILE_NAME = "AchievementList.json";


    /// <summary> AES暗号化 </summary>
    static byte[] AesEncrypt(byte[] byteText)
    {
        // AES設定値　aesIv・aesKeyは暗号化と複合化で同じものを使用する
        //===================================
        int aesKeySize = 128;
        int aesBlockSize = 128;
        string aesIv = "6KGhH66PeU3cSLS7";
        string aesKey = "R38FYEzPyjxv0HrE";
        //===================================

        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // 暗号化
        byte[] encryptText = aes.CreateEncryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return encryptText;
    }

    /// <summary> AES復号化 </summary>
    static byte[] AesDecrypt(byte[] byteText)
    {
        // AES設定値
        //===================================
        int aesKeySize = 128;
        int aesBlockSize = 128;
        string aesIv = "6KGhH66PeU3cSLS7";
        string aesKey = "R38FYEzPyjxv0HrE";
        //===================================

        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // 復号化
        byte[] decryptText = aes.CreateDecryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return decryptText;
    }

    /// <summary> AesManagedを取得 </summary>
    /// <param name="keySize">暗号化鍵の長さ</param>
    /// <param name="blockSize">ブロックサイズ</param>
    /// <param name="iv">初期化ベクトル(半角X文字（8bit * X = [keySize]bit))</param>
    /// <param name="key">暗号化鍵 (半X文字（8bit * X文字 = [keySize]bit）)</param>
    static AesManaged GetAesManager(int keySize, int blockSize, string iv, string key)
    {
        AesManaged aes = new AesManaged();
        aes.KeySize = keySize;
        aes.BlockSize = blockSize;
        aes.Mode = CipherMode.CBC;
        aes.IV = Encoding.UTF8.GetBytes(iv);
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.Padding = PaddingMode.PKCS7;

        return aes;
    }

    //====================以下テストデータをやり取りする為の関数 (暗号化は行わない)================================

    /// <summary>テスト プレイ結果を保存する</summary>
    /// <param name="killCount">敵撃破数</param>
    /// <param name="helpCount">村人救出数</param>
    /// <param name="score">スコア</param>
    public static async UniTask TestSavePlayResult(int killCount, int helpCount, int score)
    {
        var savePath = TestGetSavePath(DataType.PlayData);

        if (!File.Exists(savePath))
        {
            var newData = new PlayResult(0, 0, 0);
            await TestDataSave(savePath, newData);
            Debug.Log("新しくデータを作成 PlayResult");
        }

        var lastJson = await TestLoadData<PlayResult>(savePath);
        var saveData = new PlayResult(killCount, helpCount, score + lastJson._score);

        await TestDataSave(savePath, saveData);
    }

    /// <summary>取得した実績をセーブする 全ての実績データを保存する</summary>
    /// <param name="killIndex">撃破数 実績</param>
    /// <param name="helpIndex">救出数 実績</param>
    /// <param name="scoreIndex">階級 実績</param>
    public static async UniTask TestSaveAchievements(int killIndex, int helpIndex, int scoreIndex)
    {
        var savePath = TestGetSavePath(DataType.Achievement);

        if (!File.Exists(savePath))
        {
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("新しくデータを作成 Achievement");
        }

        var achievements = await TestLoadData<GetAchievementList>(savePath);      //データを読み込む

        achievements.OpenKillList(killIndex);
        achievements.OpenHelpList(helpIndex);
        achievements.OpenRankList(scoreIndex);

        Debug.Log("呼ばれる");
        await TestDataSave(savePath, achievements);
    }

    /// <summary>
    /// テスト用 実績リストを保存する
    /// <para>※階級実績を変更時は指定した実績まで取得できる　複数取得の可能性がある</para>
    /// </summary>
    /// <param name="type">実績の種類</param>
    /// <param name="index">獲得した実績の添え字</param>
    public static async UniTask TestSaveAchievements(AchievementType type, int index)
    {
        var savePath = GetSavePath(DataType.Achievement);

        if (!File.Exists(savePath))
        {
            Debug.Log("ない");
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("新しくデータを作成 Achievement");
        }

        var achievements = await TestLoadData<GetAchievementList>(savePath);      //データを読み込む

        switch (type)   //実績別に更新を行う
        {
            case AchievementType.Kill:
                achievements.OpenKillList(index);
                break;
            case AchievementType.Help:
                achievements.OpenHelpList(index);
                break;
            case AchievementType.Rank:
                achievements.OpenRankList(index);
                break;
        }

        await TestDataSave(savePath, achievements);
    }

    /// <summary>テスト データを保存する </summary>
    /// <param name="savePath">保存先のパス</param>
    /// <param name="data">保存データ</param>
    public static async UniTask TestDataSave<T>(string savePath, T data)
    {
        var saveData = JsonConvert.SerializeObject(data);
        File.WriteAllText(savePath, saveData);
    }

    /// <summary>テスト データを読む込む</summary>
    /// <param name="savePath">保存先のパス</param>
    public static async UniTask<T> TestLoadData<T>(DataType type)
    {
        var savePath = TestGetSavePath(type);

        if (!File.Exists(savePath))
        {
            if (type == DataType.Achievement)
            {
                var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
                await TestDataSave(savePath, newData);
                Debug.Log("新しくデータを作成 Achievement");
            }
            else
            {
                var newData = new PlayResult(0, 0, 0);
                await TestDataSave(savePath, newData);
                Debug.Log("新しくデータを作成 PlayResult");
            }
        }

        var json = File.ReadAllText(savePath);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static async UniTask<T> TestLoadData<T>(string savePath)
    {
        var json = File.ReadAllText(savePath);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>セーブ先のパスを取得する </summary>
    /// <param name="type">どのデータを保存するか</param>
    public static string TestGetSavePath(DataType type)
    {
        if (type == DataType.PlayData)
        {
            return Application.dataPath + "/myGame/JsonData/" + PLAY_DATA_FILE_NAME;
        }

        return Application.dataPath + "/myGame/JsonData/" + ACHIEVEMENT_FILE_NAME;
    }

    /// <summary>テスト用のセーブデータを初期化する </summary>
    public void TestResetPlayResult()
    {
        var saveDataPath = Application.dataPath + "/myGame/JsonData/" + PLAY_DATA_FILE_NAME;
        var saveData = JsonConvert.SerializeObject(new PlayResult(0, 0, 0));
        File.WriteAllText(saveDataPath, saveData);
    }

    //=================以下マスターデータをやり取りする為の関数======================

    /// <summary>データを保存する</summary>
    /// <param name="killCount">敵撃破数</param>
    /// <param name="helpCount">村人救出数</param>
    /// <param name="score">スコア</param>
    public static async UniTask SavePlayResult(int killCount, int helpCount, int score)
    {
        var savePath = Application.persistentDataPath + PLAY_DATA_FILE_NAME;
        var lastJson = await LoadData<PlayResult>(savePath);
        var saveData = JsonConvert.SerializeObject(new PlayResult(killCount, helpCount, score + lastJson._score));

        var jsonByte = Encoding.UTF8.GetBytes(saveData);
        jsonByte = AesEncrypt(jsonByte);                //暗号化を行う
        File.WriteAllBytes(savePath, jsonByte);         //データを書き込む
    }

    /// <summary>取得した実績をセーブする 全ての実績データを保存する</summary>
    /// <param name="killIndex">撃破数 実績</param>
    /// <param name="helpIndex">救出数 実績</param>
    /// <param name="scoreIndex">階級 実績</param>
    public static async UniTask SaveAchievements(int killIndex, int helpIndex, int scoreIndex)
    {
        var savePath = Application.dataPath + "/myGame/JsonData/" + ACHIEVEMENT_FILE_NAME;

        if (!File.Exists(savePath))
        {
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("新しくデータを作成 Achievement");
        }

        var achievements = await LoadData<GetAchievementList>(savePath);      //データを読み込む

        achievements.OpenKillList(killIndex);
        achievements.OpenHelpList(helpIndex);
        achievements.OpenRankList(scoreIndex);

        await TestDataSave(savePath, achievements);
    }

    /// <summary>
    /// テスト用 実績リストを保存する
    /// <para>※階級実績を変更時は指定した実績まで取得できる　複数取得の可能性がある</para>
    /// </summary>
    /// <param name="type">実績の種類</param>
    /// <param name="index">獲得した実績の添え字</param>
    public static async UniTask SaveAchievements(AchievementType type, int index)
    {
        var savePath = Application.persistentDataPath + ACHIEVEMENT_FILE_NAME;
        var achievements = await LoadData<GetAchievementList>(savePath);      //データを読み込む

        switch (type)   //実績別に更新を行う
        {
            case AchievementType.Kill:
                achievements.OpenKillList(index);
                break;
            case AchievementType.Help:
                achievements.OpenHelpList(index);
                break;
            case AchievementType.Rank:
                achievements.OpenRankList(index);
                break;
        }

        var saveData = JsonConvert.SerializeObject(achievements);
        var jsonByte = Encoding.UTF8.GetBytes(saveData);
        jsonByte = AesEncrypt(jsonByte);                //暗号化を行う
        File.WriteAllBytes(savePath, jsonByte);         //データを書き込む
    }

    /// <summary>テスト データを読む込む</summary>
    /// <param name="savePath">保存先のパス</param>
    public static async UniTask<T> LoadData<T>(DataType type)
    {
        var savePath = GetSavePath(type);
        var byteData = File.ReadAllBytes(savePath);
        byteData = AesDecrypt(byteData);        //複合を行う

        var json = Encoding.UTF8.GetString(byteData);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summaryJsonからのデータを読み込む</summary>
    public static async UniTask<T> LoadData<T>(string savePath)
    {
        var byteData = File.ReadAllBytes(savePath);
        byteData = AesDecrypt(byteData);        //複合を行う

        var json = Encoding.UTF8.GetString(byteData);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>セーブ先のパスを取得する </summary>
    /// <param name="type">どのデータを保存するか</param>
    public static string GetSavePath(DataType type)
    {
        if (type == DataType.PlayData)
        {
            return Application.persistentDataPath + PLAY_DATA_FILE_NAME;
        }

        return Application.persistentDataPath + ACHIEVEMENT_FILE_NAME;
    }
}

/// <summary>
/// JSONでやり取りするためのクラス 
/// 1プレイの結果を保持する
/// </summary>
[Serializable]
public class PlayResult
{
    /// <summary>倒した数 </summary>
    public int _killCount;
    /// <summary>助けた数 </summary>
    public int _helpCount;
    /// <summary>スコア </summary>
    public int _score;

    public PlayResult(int killCount, int helpCount, int score)
    {
        _killCount = killCount;
        _helpCount = helpCount;
        _score = score;
    }
}

/// <summary>取得している実績のリストを保持するクラス </summary>
[Serializable]
public class GetAchievementList
{
    /// <summary>倒した数リスト</summary>
    public bool[] _killAchievements;
    /// <summary>救出数リスト</summary>
    public bool[] _helpAchievements;
    /// <summary>階級リスト</summary>
    public bool[] _rankAchievements;

    /// <summary>開いている階級の添え字 </summary>
    int _openRankIndex = 0;

    /// <summary>各実績リストを初期化する</summary>
    public GetAchievementList(int killCount, int helpCount, int rankCount)
    {
        _killAchievements = new bool[killCount];
        _helpAchievements = new bool[helpCount];
        _rankAchievements = new bool[rankCount];
    }

    /// <summary>倒した数の実績を取得する </summary>
    /// <param name="openIndex">取得した実績の添え字</param>
    public void OpenKillList(int openIndex)
    {
        _killAchievements[openIndex] = true;
    }

    /// <summary>救出数の実績を取得する </summary>
    /// <param name="openIndex">取得した実績の添え字</param>
    public void OpenHelpList(int openIndex)
    {
        _helpAchievements[openIndex] = true;
    }

    /// <summary>階級の実績を取得</summary>
    /// <param name="openLastIndex">どこまで取得するのか</param>
    public void OpenRankList(int openLastIndex)
    {
        if (openLastIndex == 0)
        {
            _rankAchievements[0] = true;
            return;
        }

        for (var i = _openRankIndex; i < openLastIndex; i++)
        {
            _rankAchievements[i] = true;
        }

        _openRankIndex = openLastIndex;
    }
}


public enum DataType
{
    /// <summary>プレイ結果 </summary>
    PlayData,
    /// <summary>実績 </summary>
    Achievement,
}