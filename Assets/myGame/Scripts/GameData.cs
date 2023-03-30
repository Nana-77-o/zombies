using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームシーン中のデータを扱う
/// </summary>
public class GameData
{
    private static GameData instance;
    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
                instance.CountData = new Dictionary<int, int>();
                instance.CountData.Add(0, 0);
            }
            return instance;
        }
    }
    /// <summary> ゲーム中の全カウント数データ </summary>
    public Dictionary<int,int> CountData { get; private set; }
    /// <summary> ゲーム中の時間 </summary>
    public float GameTime { get=> Time.time; }
    public event Action DelCountUpdate = default;
    /// <summary> 外部からのInstance化を防ぐ </summary>
    private GameData() { }
    /// <summary>
    /// 指定IDのカウントを増やす
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void AddCount(int id,int count = 1)
    {
        if (!CountData.ContainsKey(id))
        {
            CountData.Add(id, count);
        }
        else
        {
            CountData[id] += count;
        }
        DelCountUpdate?.Invoke();
    }
    /// <summary>
    /// 全カウントをリセットする
    /// </summary>
    public void ResetCountData()
    {
        CountData.Clear(); 
        CountData.Add(0, 0);
        DelCountUpdate?.Invoke();
    }
}
