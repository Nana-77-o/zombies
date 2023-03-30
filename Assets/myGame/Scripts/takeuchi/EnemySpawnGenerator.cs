using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnGenerator : MonoBehaviour
{
    [Tooltip("�X�|�[���|�C���g")]
    [SerializeField]
    private SpawnController[] _spawnPoints = default;
    /// <summary> �N���t���O </summary>
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
    /// �G�̃X�|�[�����J�n����
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
    /// �G�̃X�|�[�����~����
    /// </summary>
    public void StopGenerator()
    {
        _isStart = false;
    }
}
