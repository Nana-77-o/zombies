using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("敵出現地点")]
    [SerializeField]
    private Transform[] _spawnPoints = default;
    [Tooltip("敵のPrefab")]
    [SerializeField]
    private GameObject[] _enemyPrefab = default;
    [Tooltip("出現時間")]
    [SerializeField]
    private float _spawnTime = 2f;

    [Tooltip("速度上昇出現数")]
    [SerializeField]
    private int _speedUpCount = 10;
    [Tooltip("出現速度上昇速度")]
    [SerializeField]
    private float _speedUpSpeed = 1.1f;
    [SerializeField]
    private int _maxGeneratCount = 100;
    private float _spawnRange = 10f;
    private float _spawnSpeed = 1f;
    /// <summary> 起動フラグ </summary>
    private bool _isStart = false;
    int _number;
    private void Start()
    {
        foreach (var enemy in _enemyPrefab)
        {
            ObjectPoolManager.Instance.CreatePool(enemy, _maxGeneratCount);
        }
    }
    /// <summary>
    /// ランダムな出現位置に敵をスポーンさせる
    /// </summary>
    private void SpawnEnemy()
    {
        int r = Random.Range(0, _spawnPoints.Length);
        _number = Random.Range(0, _enemyPrefab.Length);
        float x = Random.Range(-_spawnRange, _spawnRange);
        float y = Random.Range(-_spawnRange, _spawnRange);
        var pos = _spawnPoints[r].position;
        pos.x += x;
        pos.z += y;
        ObjectPoolManager.Instance.LimitUse(_enemyPrefab[_number], pos, _maxGeneratCount);
    }
    private IEnumerator GeneratorUpdate()
    {
        float timer = _spawnTime;
        int count = _speedUpCount;
        while (_isStart)
        {
            timer -= _spawnSpeed *Time.deltaTime;
            if (timer <= 0)
            {
                timer = _spawnTime;
                SpawnEnemy();
                count--;
                if (count <= 0)//カウントごとに速度上昇させる
                {
                    count = _speedUpCount;
                    _spawnSpeed *= _speedUpSpeed;
                }
            }
            yield return null;
        }
    }
    /// <summary>
    /// 敵のスポーンを開始する
    /// </summary>
    public void StartGenerator()
    {
        if (_isStart)
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
