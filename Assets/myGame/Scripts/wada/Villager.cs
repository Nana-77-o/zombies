using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villager : MonoBehaviour
{
    [Tooltip("���l�~�o���ɌĂяo��GameData.Instance.AddCount��ID  ��{12"), SerializeField]
    int _villagerNumber;

    [Tooltip("�~�o�ɂ����鎞��"), SerializeField]
    int _rescueSecond;

    [Tooltip("�~�o�\�ȋ���"), SerializeField]
    int _rescueDistance;

    [Tooltip("���l�̓����鑬�x"), SerializeField]
    float _moveSpeed;

    [Tooltip("�]���r�ɑ��l�����m������͈͌�������͈�"), SerializeField]
    float _radius = 0;

    [Tooltip("���O�̑��l�������̑��l��"), SerializeField]
    bool _outside = false;

    [Tooltip("�~�o���Ԃ̕W���p��Image"), SerializeField]
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
            if (_rescueCount >= _rescueSecond)//�~�o���Ԃ𒴂�����
            {
                _connectObj = null; //_connectObj��Null�ɂ���
                //Debug.Log("ababababababababababababababababababababababababababababa");
                RescueComplete();//�~�o�֐����Ăяo��
            }
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Vector3.Distance(NavigationManager.Instance.DefaultTarget.position, this.transform.position) <= _rescueDistance)//�~�o���̃v���C���[����苗�����̎�
        {
            RescueCount += Time.deltaTime;//�~�o�J�E���g�����ނ���
            if (_viewUI != null)
            {
                _viewUI.SetActive(true);
            }
        }
        else if (RescueCount != 0)//�~�o���̃v���C���[����苗���O�ɏo����
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
