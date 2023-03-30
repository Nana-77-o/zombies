using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnGenerator : MonoBehaviour
{
    [Tooltip("スポーンポイント")]
    [SerializeField]
    private SpawnController[] _spawnPoints = default;
    /// <summary> 起動フラグ </summary>
    private bool _isStart = false;
   
    private IEnumerator GeneratorUpdate()
    {
        while (_isStart == true)
        {
            foreach (var point in _spawnPoints)
            {
                point.GeneratorUpdate();
            }
            yield return null;
        }
    }
    /// <summary>
    /// 敵のスポーンを開始する
    /// </summary>
    public void StartGenerator()
    {
        if (_isStart == true)
        {
            return;
        }
        _isStart = true;
        StartCoroutine(GeneratorUpdate());
    }
    /// <summary>
    /// 敵のスポーンを停止する
    /// </summary>
    public void StopGenerator()
    {
        _isStart = false;
    }
}
