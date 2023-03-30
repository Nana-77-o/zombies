using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NaviEnemy : MonoBehaviour
{
    [SerializeField]
    private float _naviInterval = 1f;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private Transform _body = default;
    private Vector3 _currentDir = Vector3.zero;
    [SerializeField]
    private float _lostTime = 0.05f;
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
    private float _activeRange = 300f;
    [SerializeField]
    private float _fallSpeed = 1f;
    private float _timer = 0f;
    private bool _isMove = false;
    [SerializeField]
    private float _randamRange = 0.5f;
    private float _lostTimer = 0;
    private bool _trackingVillager = false;
    private bool _activeMode = false;

    private void Start()
    {
        _timer = _naviInterval;
        if (_body is null)
        {
            _body = transform;
        }
    }

    private void Update()
    {
        if (_isMove == false || _activeMode == false) { return; }
        _timer += Time.deltaTime;
        if (_timer > _naviInterval)
        {
            _timer = 0;
            if (_trackingVillager == false)
            {
                _currentDir = NavigationManager.Instance.GetMoveDir(_body);
            }
            else
            {
                _lostTimer += Time.deltaTime;
                if (_lostTimer >= _lostTime)
                {
                    _trackingVillager = false;
                    _lostTimer = 0; 
                }
            }
            _currentDir.x += Random.Range(-_randamRange, _randamRange);
            _currentDir.z += Random.Range(-_randamRange, _randamRange);
        }
    }
    private void FixedUpdate()
    {
        _activeMode = Vector3.Distance(transform.position, NavigationManager.Instance.DefaultTarget.position) < _activeRange;
        if (Physics.Raycast(transform.position + Vector3.up * _rayLevel, Vector3.down, out RaycastHit ground, _rayRange, _groundLayer))
        {
            transform.position = Vector3.Lerp(transform.position, ground.point, _fallSpeed * Time.fixedDeltaTime);
        }
        if (_isMove == false || _activeMode == false) { return; }
        Vector3 velo = _currentDir.normalized * _moveSpeed + transform.position + Vector3.up * _rayLevel;
        if (Physics.Raycast(velo, Vector3.down, out RaycastHit hit, _rayRange, _groundLayer))
        {
            if (Mathf.Abs(transform.position.y - hit.point.y) < _noneMoveLevel)
            {
                transform.position = Vector3.Lerp(transform.position, hit.point, _lerpSpeed * Time.fixedDeltaTime);
            }
        }
    }
    private void OnDisable()
    {
        StopMove();
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

    public void TargetChange(Vector3 villagerPos)
    {
        if (!_isMove) { return; }
        _trackingVillager = true;
        _lostTimer = 0;
        _currentDir = (villagerPos - this.transform.position).normalized;
    }
}
