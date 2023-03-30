using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBuble : MonoBehaviour
{
    /// <summary>チャージ速度</summary>
    [SerializeField] int _chargeSpeed;
    /// <summary>今のチャージ</summary>
    [SerializeField] float _chargeBuble;

    public bool _canCharge = false;

    public float Charge
    {
        get
        {
            return _chargeBuble;
        }
        set
        {
            _chargeBuble = value;
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_canCharge)
        {
            Charge += _chargeSpeed * Time.deltaTime;
        }
    }
}
