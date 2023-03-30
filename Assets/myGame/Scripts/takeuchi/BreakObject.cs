using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour , IDamageApplicable
{
    [SerializeField]
    private int _durableValue = 0;
    [SerializeField]
    private GameObject _breakEffect = null;
    [SerializeField]
    private GameObject _hitEffect = null;
    private int _count = 0;
    public void AddDamage(Damage damage)
    {
        _count++;
        if (_hitEffect != null)
        {
            ObjectPoolManager.Instance.Use(_hitEffect).transform.position = transform.position;
        }
        if (_durableValue <= _count)
        {
            if (_breakEffect != null)
            {
                ObjectPoolManager.Instance.Use(_breakEffect).transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }
    }

}
