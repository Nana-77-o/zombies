using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveController : MonoBehaviour
{
    [SerializeField]
    private float _maxAwakeTime = 5f;
    [SerializeField]
    private Animator _animator = default;
    [SerializeField]
    private NaviEnemy _enemy = null;
    [SerializeField]
    private string[] _spawnAnime = { "Sleep", "Fly" };
    [SerializeField]
    private string[] _getUpAnime = { "GetUp", "Fall" };
    [SerializeField]
    private string _endAnime = "FireEnd";
    [SerializeField]
    private SampleEnemy _hitChecker = null;
    [SerializeField]
    private bool _isInstance = false;
    [SerializeField]
    private bool _isAnime = true;
    private void OnEnable()
    {
        if (_isAnime)
        {
            if (!_isInstance)
            {
                _isInstance = true;
                _hitChecker.DelDead += () => { _animator.Play(_endAnime); };
            }
            StartCoroutine(StartSpawn());
        }
        else
        {
            if (!_isInstance)
            {
                _isInstance = true;
                _hitChecker.DelDead += () => DeadAction();
            }
            StartCoroutine(NoneAnimeStart());
        }
    }
    private IEnumerator StartSpawn()
    {
        float time = Random.Range(0, _maxAwakeTime);
        int r = Random.Range(0, _spawnAnime.Length);
        _animator.Play(_spawnAnime[r]);
        yield return new WaitForSeconds(time);
        _animator.Play(_getUpAnime[r]);
        StartMove();
    }
    private IEnumerator NoneAnimeStart()
    {
        float time = Random.Range(0, _maxAwakeTime);
        yield return new WaitForSeconds(time);
        StartMove();
    }
    private void StartMove()
    {
        if (_enemy is not null)
        {
            _enemy.StartMove();
        }
    }
    private void DeadAction()
    {
        _enemy.gameObject.SetActive(false);
    }
}
