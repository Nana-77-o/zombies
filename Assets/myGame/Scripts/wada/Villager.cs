using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villager : MonoBehaviour
{
    [Tooltip("村人救出時に呼び出すGameData.Instance.AddCountのID  基本12"), SerializeField]
    int _villagerNumber;

    [Tooltip("救出にかかる時間"), SerializeField]
    int _rescueSecond;

    [Tooltip("救出可能な距離"), SerializeField]
    int _rescueDistance;

    [Tooltip("村人の逃げる速度"), SerializeField]
    float _moveSpeed;

    [Tooltip("ゾンビに村人を感知させる範囲向かせる範囲"), SerializeField]
    float _radius = 0;

    [Tooltip("屋外の村人か屋内の村人か"), SerializeField]
    bool _outside = false;

    [Tooltip("救出時間の標示用のImage"), SerializeField]
    Image _rescueTimeImage;

    [SerializeField]
    private GameObject _viewUI = default;
    private GameObject _connectObj;

    private Rigidbody _rb = default;

    private Vector3 _currentDir = Vector3.zero;

    private Vector3 _escapeDir = Vector3.zero;

    private float _rescueCount;
    private float _level = 0.5f;

    public float RescueCount
    {
        get
        {
            return _rescueCount;
        }
        set
        {
            _rescueCount = value;
            _rescueTimeImage.fillAmount = _rescueCount / _rescueSecond;
            if (_rescueCount >= _rescueSecond)//救出時間を超えたら
            {
                _connectObj = null; //_connectObjをNullにする
                //Debug.Log("ababababababababababababababababababababababababababababa");
                RescueComplete();//救出関数を呼び出す
            }
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Vector3.Distance(NavigationManager.Instance.DefaultTarget.position, this.transform.position) <= _rescueDistance)//救出中のプレイヤーが一定距離内の時
        {
            RescueCount += Time.deltaTime;//救出カウントすすむくん
            if (_viewUI != null)
            {
                _viewUI.SetActive(true);
            }
        }
        else if (RescueCount != 0)//救出中のプレイヤーが一定距離外に出た時
        {
            RescueCount = 0;
            if (_viewUI != null)
            {
                _viewUI.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_outside)
        {
            _escapeDir = Vector3.zero;
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, _radius);
            foreach (Collider search in colliders)
            {
                float level = search.gameObject.transform.position.y;
                if (level > transform.position.y + _level || level < transform.position.y - level)
                {
                    continue;
                }
                if (search.TryGetComponent(out NaviEnemy naviEnemy))
                {
                    naviEnemy.TargetChange(this.transform.position);

                    _escapeDir += (naviEnemy.transform.position - this.transform.position).normalized;

                }
            }

            if (colliders.Length >= 1)
            {
                var dir = -_escapeDir.normalized * _moveSpeed;
                dir.y = _rb.velocity.y;
                _rb.velocity = dir;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }
    void RescueVillager()
    {
        
    }
    void RescueComplete()
    {
        this.gameObject.SetActive(false);
        GameData.Instance.AddCount(_villagerNumber);
    }
}
