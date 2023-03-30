using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField, Tooltip("�G��Prefab")]
    private GameObject[] _enemysPrefab = default;
    [SerializeField, Tooltip("�o���ꏊ")]
    private RandomSpawnPoint[] spawnPoints = default;
    [SerializeField, Tooltip("��x�ɏo������ŏ���")]
    private int _minSpawnCount = 1;
    [SerializeField, Tooltip("��x�ɏo������ő吔")]
    private int _maxSpawnCount = 5;
    [SerializeField]
    private int _maxGeneratCount = 100;
    [Tooltip("�o���J�n����")]
    [SerializeField]
    private float _spawnStartTime = 1f;
    [Tooltip("�o���I������(���̐��͏I�����Ȃ�)")]
    [SerializeField]
    private float _spawnEndTime = -1f;
    [Tooltip("�o������")]
    [SerializeField]
    private float _spawnTime = 2f;
    [Tooltip("���x�㏸�o����")]
    [SerializeField]
    private int _speedUpCount = 10;
    [Tooltip("�o�����x�㏸���x")]
    [SerializeField]
    private float _speedUpSpeed = 1.1f;
    private float _spawnSpeed = 1f;
    private float _startTimer = 0;
    private float _endTimer = 0;
    private float _popTimer = 0;
    private int _popCount = 0;
    private bool _stopSpawn = false;
    private void Start()
    {
        foreach (var enemy in _enemysPrefab)
        {
            ObjectPoolManager.Instance.CreatePool(enemy, _maxGeneratCount);
        }
        _popCount = _speedUpCount;
        _startTimer = _spawnStartTime;
    }
    public void GeneratorUpdate()
    {
        if (_stopSpawn == true) { return; }
        if (_startTimer > 0)
        {
            _startTimer -= Time.deltaTime;
            return;
        }
        _popTimer -= _spawnSpeed * Time.deltaTime;
        if (_popTimer <= 0)
        {
            _popTimer = _spawnTime;
            _popCount--;
            int popNum = Random.Range(_minSpawnCount, _maxSpawnCount + 1);
            for (int i = 0; i < popNum; i++)
            {
                Spawn();
            }
            if (_popCount <= 0)//�J�E���g���Ƃɑ��x�㏸������
            {
                _popCount = _speedUpCount;
                _spawnSpeed *= _speedUpSpeed;
            }
        }
        if (_spawnEndTime >= 0)
        {
            _endTimer += Time.deltaTime;
            if (_endTimer > _spawnEndTime)
            {
                _stopSpawn = true;
                gameObject.SetActive(false);
            }
        }
    }
    public void Spawn()
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].GetSpawnPos();
        ObjectPoolManager.Instance.LimitUse(_enemysPrefab[Random.Range(0, _enemysPrefab.Length)], spawnPoint, _maxGeneratCount);
    }
}
