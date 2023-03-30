using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NaviUser : MonoBehaviour
{
    [SerializeField]
    private Transform[] _targets = default;
    [SerializeField]
    private float _naviInterval = 1f;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private Transform _body = default;
    [SerializeField]
    private LayerMask _groundLayer = default;
    [SerializeField]
    private float _rayLevel = 3f;
    [SerializeField]
    private float _lerpSpeed = 1f;
    [SerializeField]
    private float _rayRange = 100f;
    [SerializeField]
    private float _noneMoveLevel = 2f;
    [SerializeField]
    private float _fallSpeed = 1f;
    private Vector3 _currentDir = Vector3.zero;
    private float _timer = 0f;
    private bool _isMove = false;
    private IEnumerator Start()
    {
        _timer = _naviInterval;
        if (_body is null)
        {
            _body = transform;
        }
        foreach (Transform target in _targets)
        {
            NavigationManager.Instance.RequestTargetNavigation(target);
        }
        yield return null;
        StartMove();
    }

    private void Update()
    {
        if (!_isMove) { return; }
        _timer += Time.deltaTime;
        if (_timer > _naviInterval)
        {
            _timer = 0;
            int point = 0;
            string topTarget = null;
            foreach (Transform target in _targets)
            {
                if (target.gameObject.activeSelf == false)
                {
                    continue;
                }
                var p = NavigationManager.Instance.GetPoint(_body, target.gameObject.name);
                if (p > point)
                {
                    point = p;
                    topTarget = target.gameObject.name;
                }
            }
            if (topTarget is not null)
            {
                _currentDir = NavigationManager.Instance.GetMoveDir(_body, topTarget);
            }
            else
            {
                _currentDir = Vector3.zero;
            }
        }
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + Vector3.up * _rayLevel, Vector3.down, out RaycastHit ground, _rayRange, _groundLayer))
        {
            transform.position = Vector3.Lerp(transform.position, ground.point, _fallSpeed * Time.fixedDeltaTime);
        }
        if (!_isMove) { return; }
        Vector3 velo = _currentDir * _moveSpeed + transform.position + Vector3.up * _rayLevel;
        if (Physics.Raycast(velo, Vector3.down, out RaycastHit hit, _rayRange, _groundLayer))
        {
            if (Mathf.Abs(transform.position.y - hit.point.y) < _noneMoveLevel)
            {
                transform.position = Vector3.Lerp(transform.position, hit.point, _lerpSpeed * Time.fixedDeltaTime);
            }
        }
    }
    public void StartMove()
    {
        _isMove = true;
    }
    public void StopMove()
    {
        _isMove = false;
        _currentDir = Vector3.zero;
    }
    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }
}
