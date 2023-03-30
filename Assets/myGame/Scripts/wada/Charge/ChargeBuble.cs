using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBuble : MonoBehaviour
{
    /// <summary>�`���[�W���x</summary>
    [SerializeField] int _chargeSpeed;
    /// <summary>���̃`���[�W</summary>
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
