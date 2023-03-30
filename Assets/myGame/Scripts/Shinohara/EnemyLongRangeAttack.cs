using System.Collections;
using UnityEngine;

/// <summary> 敵の遠距離攻撃で使用する攻撃オブジェクトを管理するクラス  </summary>
public class EnemyLongRangeAttack : WeaponBullet
{
    [SerializeField, Header("滞空時間")]
    float _flightTime = 2;
    [SerializeField, Header("移動速度")]
    float _moveSpeed = 3;
    /// <summary>指定した位置に到着</summary>
    bool _isArrival = false;
    /// <summary>生成位置と着弾位置の距離 </summary>
    float _distance = 0f;
    /// <summary>発射位置 </summary>
    Vector3 _firePoint = Vector3.zero;

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer > _lifeTime) { gameObject.SetActive(false); }
        if (_isArrival)     //到着後にz軸方向に移動させる
        {
            transform.position += transform.forward * _distance * Time.deltaTime;
        }
    }

    /// <summary>
    /// 遠距離攻撃を発動する
    /// 遠距離攻撃を行う時に呼び出す
    /// </summary>
    /// <param name="targetPos">着弾位置</param>
    public override void EnemyLongAttack(Damage damage, Vector3 generatePoint, Vector3 arrivalPoint)
    {
        _lifeTimer = 0;
        base.EnemyLongAttack(damage, generatePoint, arrivalPoint);
        MoveInitialization(generatePoint, arrivalPoint);
        ActiveEffect();
    }

    /// <summary> 移動開始時の初期設定 </summary>
    /// <param name="arrivalPoint">着弾位置</param>
    public void MoveInitialization(Vector3 generatePoint,  Vector3 arrivalPoint)
    {
        _firePoint = generatePoint;
        _distance = Vector3.Distance(generatePoint, arrivalPoint);
        StartCoroutine(Move(arrivalPoint));
    }

    /// <summary>攻撃オブジェクト(自身)の移動処理 </summary>
    /// <param name="targetPos">着弾位置</param>
    IEnumerator Move(Vector3 targetPos)
    {
        var diffY = (targetPos - _firePoint).y; // y軸差分
        var firstSpeed = (diffY - Physics.gravity.y * 0.5f * _flightTime * _flightTime) / _flightTime; // 初速度

        // 放物運動
        for (var t = 0f; t < _flightTime; t += Time.deltaTime * _moveSpeed)     //滞空時間分回す
        {
            var currentPos = Vector3.Lerp(_firePoint, targetPos, t / _flightTime);
            currentPos.y = _firePoint.y + firstSpeed * t + 0.5f * Physics.gravity.y * t * t;
            transform.position = currentPos;
            transform.forward = targetPos - transform.position;

            yield return null;
        }

        _isArrival = true;
    }
}