using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charge : MonoBehaviour
{
    //[SerializeField, Tooltip("チャージの最大値")] 
    //float _chargeMax = 20;
    //[SerializeField, Tooltip("チャージスピード")]
    //float _chargeSpeed;
    //[Tooltip("現在の使える量")]
    //float _chargeTimer;
    //[Tooltip("発射できる")]
    //public bool _chargebool = true;

    private void Start()
    {
        //_chargeTimer = _chargeMax;
    }
    private void OnEnable()
    {
        //PlayerInput.SetStayInput(InputType.Fire2, this.Chargeing);
        //PlayerInput.SetExitInput(InputType.Fire2, this.UpCharge);
    }

    private void OnDisable()
    {
        //PlayerInput.LiftEnterInput(InputType.Fire1, this.Chargeing);
    }

    /// <summary>
    /// チャージボタンを離したとき
    /// </summary>
    //private void UpCharge()
    //{
    //    _chargebool = true;
    //}

    /// <summary>チャージする</summary>
    //public void Chargeing()
    //{
    //    _chargebool = false;
    //    _chargeTimer += _chargeSpeed * Time.deltaTime;
    //    if (_chargeTimer > _chargeMax)
    //    {
    //        _chargeTimer = _chargeMax;
    //        _chargebool = true;
    //    }
    //}

    /// <summary>ゲージを減らす</summary>
    /// <param name="decrease"></param>
    //public void ChargeMax(float decrease)
    //{
    //    if (_chargeTimer > 0 && _chargebool)
    //    {
    //        _chargeTimer -= decrease;
    //        if (_chargeTimer <= 0)
    //        {
    //            _chargebool = false;
    //        }
    //    }
    //}
}
