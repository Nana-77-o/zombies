using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : MonoBehaviour
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
    [SerializeField ]
    private float _time = 3;

    private GameObject _player = default;

    public GameObject Player { get => _player; set => _player = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hitCenter.position, _range / 2);
    }
    private void OnEnable()
    {
        if (_isActiveStart == false) { return; }
        StartChack();
    }
    private IEnumerator ChackUpdate()
    {
        _effect.Play();
        while (!HitCheck())
        {
            yield return null;
        }
        _effect.Stop();
        GameData.Instance.AddCount(_objID);
        _hitEffect.Play();
        _icon.SetActive(false);
        BossMissionManager.Instance.TypeSelect();
        var playerParam = _player.GetComponent<PlayerParameter>();
        if (playerParam != null && !playerParam.Item)
        {
            playerParam.PowerUPItem();
        }
        yield return new WaitForSeconds(_time);
        StartCoroutine(ChackUpdate());
    }
    private bool HitCheck()
    {
        return Vector3.Distance(_hitCenter.position, NavigationManager.Instance.DefaultTarget.position) <= _range;
    }
    public void StartChack()
    {
        StartCoroutine(ChackUpdate());
    }
}
