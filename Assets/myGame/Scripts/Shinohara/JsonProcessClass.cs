using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;


/// <summary>Json�֘A�̏������s��</summary>
public class JsonProcessClass : MonoBehaviour
{
    /// <summary>�|�������̎��ѐ�</summary>
    const int KILL_ACHIEVEMENR_COUNT = 16;
    /// <summary>�~�o���̎��ѐ�</summary>
    const int HELP_ACHIEVEMENR_COUNT = 11;
    /// <summary>�K���̎��ѐ�</summary>
    const int RANK_ACHIEVEMENR_COUNT = 20;

    /// <summary1�v���C��ۑ�����t�@�C���� </summary>
    public static readonly string PLAY_DATA_FILE_NAME = "PlayResult.json";
    /// <summary>���у��X�g��ۑ�����t�@�C����</summary>
    public static readonly string ACHIEVEMENT_FILE_NAME = "AchievementList.json";


    /// <summary> AES�Í��� </summary>
    static byte[] AesEncrypt(byte[] byteText)
    {
        // AES�ݒ�l�@aesIv�EaesKey�͈Í����ƕ������œ������̂��g�p����
        //===================================
        int aesKeySize = 128;
        int aesBlockSize = 128;
        string aesIv = "6KGhH66PeU3cSLS7";
        string aesKey = "R38FYEzPyjxv0HrE";
        //===================================

        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // �Í���
        byte[] encryptText = aes.CreateEncryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return encryptText;
    }

    /// <summary> AES������ </summary>
    static byte[] AesDecrypt(byte[] byteText)
    {
        // AES�ݒ�l
        //===================================
        int aesKeySize = 128;
        int aesBlockSize = 128;
        string aesIv = "6KGhH66PeU3cSLS7";
        string aesKey = "R38FYEzPyjxv0HrE";
        //===================================

        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // ������
        byte[] decryptText = aes.CreateDecryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return decryptText;
    }

    /// <summary> AesManaged���擾 </summary>
    /// <param name="keySize">�Í������̒���</param>
    /// <param name="blockSize">�u���b�N�T�C�Y</param>
    /// <param name="iv">�������x�N�g��(���pX�����i8bit * X = [keySize]bit))</param>
    /// <param name="key">�Í����� (��X�����i8bit * X���� = [keySize]bit�j)</param>
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

    //====================�ȉ��e�X�g�f�[�^������肷��ׂ̊֐� (�Í����͍s��Ȃ�)================================

    /// <summary>�e�X�g �v���C���ʂ�ۑ�����</summary>
    /// <param name="killCount">�G���j��</param>
    /// <param name="helpCount">���l�~�o��</param>
    /// <param name="score">�X�R�A</param>
    public static async UniTask TestSavePlayResult(int killCount, int helpCount, int score)
    {
        var savePath = TestGetSavePath(DataType.PlayData);

        if (!File.Exists(savePath))
        {
            var newData = new PlayResult(0, 0, 0);
            await TestDataSave(savePath, newData);
            Debug.Log("�V�����f�[�^���쐬 PlayResult");
        }

        var lastJson = await TestLoadData<PlayResult>(savePath);
        var saveData = new PlayResult(killCount, helpCount, score + lastJson._score);

        await TestDataSave(savePath, saveData);
    }

    /// <summary>�擾�������т��Z�[�u���� �S�Ă̎��уf�[�^��ۑ�����</summary>
    /// <param name="killIndex">���j�� ����</param>
    /// <param name="helpIndex">�~�o�� ����</param>
    /// <param name="scoreIndex">�K�� ����</param>
    public static async UniTask TestSaveAchievements(int killIndex, int helpIndex, int scoreIndex)
    {
        var savePath = TestGetSavePath(DataType.Achievement);

        if (!File.Exists(savePath))
        {
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("�V�����f�[�^���쐬 Achievement");
        }

        var achievements = await TestLoadData<GetAchievementList>(savePath);      //�f�[�^��ǂݍ���

        achievements.OpenKillList(killIndex);
        achievements.OpenHelpList(helpIndex);
        achievements.OpenRankList(scoreIndex);

        Debug.Log("�Ă΂��");
        await TestDataSave(savePath, achievements);
    }

    /// <summary>
    /// �e�X�g�p ���у��X�g��ۑ�����
    /// <para>���K�����т�ύX���͎w�肵�����т܂Ŏ擾�ł���@�����擾�̉\��������</para>
    /// </summary>
    /// <param name="type">���т̎��</param>
    /// <param name="index">�l���������т̓Y����</param>
    public static async UniTask TestSaveAchievements(AchievementType type, int index)
    {
        var savePath = GetSavePath(DataType.Achievement);

        if (!File.Exists(savePath))
        {
            Debug.Log("�Ȃ�");
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("�V�����f�[�^���쐬 Achievement");
        }

        var achievements = await TestLoadData<GetAchievementList>(savePath);      //�f�[�^��ǂݍ���

        switch (type)   //���ѕʂɍX�V���s��
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

    /// <summary>�e�X�g �f�[�^��ۑ����� </summary>
    /// <param name="savePath">�ۑ���̃p�X</param>
    /// <param name="data">�ۑ��f�[�^</param>
    public static async UniTask TestDataSave<T>(string savePath, T data)
    {
        var saveData = JsonConvert.SerializeObject(data);
        File.WriteAllText(savePath, saveData);
    }

    /// <summary>�e�X�g �f�[�^��ǂލ���</summary>
    /// <param name="savePath">�ۑ���̃p�X</param>
    public static async UniTask<T> TestLoadData<T>(DataType type)
    {
        var savePath = TestGetSavePath(type);

        if (!File.Exists(savePath))
        {
            if (type == DataType.Achievement)
            {
                var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
                await TestDataSave(savePath, newData);
                Debug.Log("�V�����f�[�^���쐬 Achievement");
            }
            else
            {
                var newData = new PlayResult(0, 0, 0);
                await TestDataSave(savePath, newData);
                Debug.Log("�V�����f�[�^���쐬 PlayResult");
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

    /// <summary>�Z�[�u��̃p�X���擾���� </summary>
    /// <param name="type">�ǂ̃f�[�^��ۑ����邩</param>
    public static string TestGetSavePath(DataType type)
    {
        if (type == DataType.PlayData)
        {
            return Application.dataPath + "/myGame/JsonData/" + PLAY_DATA_FILE_NAME;
        }

        return Application.dataPath + "/myGame/JsonData/" + ACHIEVEMENT_FILE_NAME;
    }

    /// <summary>�e�X�g�p�̃Z�[�u�f�[�^������������ </summary>
    public void TestResetPlayResult()
    {
        var saveDataPath = Application.dataPath + "/myGame/JsonData/" + PLAY_DATA_FILE_NAME;
        var saveData = JsonConvert.SerializeObject(new PlayResult(0, 0, 0));
        File.WriteAllText(saveDataPath, saveData);
    }

    //=================�ȉ��}�X�^�[�f�[�^������肷��ׂ̊֐�======================

    /// <summary>�f�[�^��ۑ�����</summary>
    /// <param name="killCount">�G���j��</param>
    /// <param name="helpCount">���l�~�o��</param>
    /// <param name="score">�X�R�A</param>
    public static async UniTask SavePlayResult(int killCount, int helpCount, int score)
    {
        var savePath = Application.persistentDataPath + PLAY_DATA_FILE_NAME;
        var lastJson = await LoadData<PlayResult>(savePath);
        var saveData = JsonConvert.SerializeObject(new PlayResult(killCount, helpCount, score + lastJson._score));

        var jsonByte = Encoding.UTF8.GetBytes(saveData);
        jsonByte = AesEncrypt(jsonByte);                //�Í������s��
        File.WriteAllBytes(savePath, jsonByte);         //�f�[�^����������
    }

    /// <summary>�擾�������т��Z�[�u���� �S�Ă̎��уf�[�^��ۑ�����</summary>
    /// <param name="killIndex">���j�� ����</param>
    /// <param name="helpIndex">�~�o�� ����</param>
    /// <param name="scoreIndex">�K�� ����</param>
    public static async UniTask SaveAchievements(int killIndex, int helpIndex, int scoreIndex)
    {
        var savePath = Application.dataPath + "/myGame/JsonData/" + ACHIEVEMENT_FILE_NAME;

        if (!File.Exists(savePath))
        {
            var newData = new GetAchievementList(KILL_ACHIEVEMENR_COUNT, HELP_ACHIEVEMENR_COUNT, RANK_ACHIEVEMENR_COUNT);
            await TestDataSave(savePath, newData);
            Debug.Log("�V�����f�[�^���쐬 Achievement");
        }

        var achievements = await LoadData<GetAchievementList>(savePath);      //�f�[�^��ǂݍ���

        achievements.OpenKillList(killIndex);
        achievements.OpenHelpList(helpIndex);
        achievements.OpenRankList(scoreIndex);

        await TestDataSave(savePath, achievements);
    }

    /// <summary>
    /// �e�X�g�p ���у��X�g��ۑ�����
    /// <para>���K�����т�ύX���͎w�肵�����т܂Ŏ擾�ł���@�����擾�̉\��������</para>
    /// </summary>
    /// <param name="type">���т̎��</param>
    /// <param name="index">�l���������т̓Y����</param>
    public static async UniTask SaveAchievements(AchievementType type, int index)
    {
        var savePath = Application.persistentDataPath + ACHIEVEMENT_FILE_NAME;
        var achievements = await LoadData<GetAchievementList>(savePath);      //�f�[�^��ǂݍ���

        switch (type)   //���ѕʂɍX�V���s��
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
        jsonByte = AesEncrypt(jsonByte);                //�Í������s��
        File.WriteAllBytes(savePath, jsonByte);         //�f�[�^����������
    }

    /// <summary>�e�X�g �f�[�^��ǂލ���</summary>
    /// <param name="savePath">�ۑ���̃p�X</param>
    public static async UniTask<T> LoadData<T>(DataType type)
    {
        var savePath = GetSavePath(type);
        var byteData = File.ReadAllBytes(savePath);
        byteData = AesDecrypt(byteData);        //�������s��

        var json = Encoding.UTF8.GetString(byteData);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summaryJson����̃f�[�^��ǂݍ���</summary>
    public static async UniTask<T> LoadData<T>(string savePath)
    {
        var byteData = File.ReadAllBytes(savePath);
        byteData = AesDecrypt(byteData);        //�������s��

        var json = Encoding.UTF8.GetString(byteData);

        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>�Z�[�u��̃p�X���擾���� </summary>
    /// <param name="type">�ǂ̃f�[�^��ۑ����邩</param>
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
/// JSON�ł���肷�邽�߂̃N���X 
/// 1�v���C�̌��ʂ�ێ�����
/// </summary>
[Serializable]
public class PlayResult
{
    /// <summary>�|������ </summary>
    public int _killCount;
    /// <summary>�������� </summary>
    public int _helpCount;
    /// <summary>�X�R�A </summary>
    public int _score;

    public PlayResult(int killCount, int helpCount, int score)
    {
        _killCount = killCount;
        _helpCount = helpCount;
        _score = score;
    }
}

/// <summary>�擾���Ă�����т̃��X�g��ێ�����N���X </summary>
[Serializable]
public class GetAchievementList
{
    /// <summary>�|���������X�g</summary>
    public bool[] _killAchievements;
    /// <summary>�~�o�����X�g</summary>
    public bool[] _helpAchievements;
    /// <summary>�K�����X�g</summary>
    public bool[] _rankAchievements;

    /// <summary>�J���Ă���K���̓Y���� </summary>
    int _openRankIndex = 0;

    /// <summary>�e���у��X�g������������</summary>
    public GetAchievementList(int killCount, int helpCount, int rankCount)
    {
        _killAchievements = new bool[killCount];
        _helpAchievements = new bool[helpCount];
        _rankAchievements = new bool[rankCount];
    }

    /// <summary>�|�������̎��т��擾���� </summary>
    /// <param name="openIndex">�擾�������т̓Y����</param>
    public void OpenKillList(int openIndex)
    {
        _killAchievements[openIndex] = true;
    }

    /// <summary>�~�o���̎��т��擾���� </summary>
    /// <param name="openIndex">�擾�������т̓Y����</param>
    public void OpenHelpList(int openIndex)
    {
        _helpAchievements[openIndex] = true;
    }

    /// <summary>�K���̎��т��擾</summary>
    /// <param name="openLastIndex">�ǂ��܂Ŏ擾����̂�</param>
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
    /// <summary>�v���C���� </summary>
    PlayData,
    /// <summary>���� </summary>
    Achievement,
}