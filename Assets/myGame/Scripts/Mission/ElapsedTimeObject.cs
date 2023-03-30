using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElapsedTimeObject : MonoBehaviour
{
    [SerializeField]
    private int _objID = default;
    [SerializeField]
    private ParticleSystem _effect = default;
    [SerializeField]
    private ParticleSystem _hitEffect = default;
    [SerializeField]
    private float _range = 1.5f;
    [SerializeField]
    private Transform _hitCenter = default;
    [SerializeField]
    private bool _isActiveStart = true;
    [SerializeField]
    private GameObject _icon = default;
    [SerializeField]
    private GameObject _obj = default;
    [SerializeField]
    private Image _gauge = default;
    [SerializeField]
    private float _time = 3;
    private float _timer = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hitCenter.position, _range / 2);
    }
    private void Start()
    {
        if (_isActiveStart == false) { return; }
        StartChack();
    }
    private IEnumerator ChackUpdate()
    {
        _effect.Play();
        while (_timer < _time)
        {
            if (HitCheck())
            {
                _timer += Time.deltaTime;
                if (_gauge != null)
                {
                    _gauge.fillAmount = _timer / _time;
                }
                _obj.SetActive(true);
            }
            else if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _obj.SetActive(false);
            }
            yield return null;
        }
        _effect.Stop();
        GameData.Instance.AddCount(_objID);
        _hitEffect.Play();
        _icon.SetActive(false);
        _obj.SetActive(false);
    }
    private bool HitCheck()
    {
        return Vector3.Distance(_hitCenter.position, NavigationManager.Instance.DefaultTarget.position) <= _range;
    }
    public void StartChack()
    {
        _icon.SetActive(true);
        StartCoroutine(ChackUpdate());
    }
}
