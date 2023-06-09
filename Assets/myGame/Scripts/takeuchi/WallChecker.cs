using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁判定を行う
/// </summary>
public class WallChecker : MonoBehaviour
{
    [SerializeField]
    private Transform[] _checkStartPoint = default;
    [SerializeField]
    private Vector3 _checkDir = Vector3.down;
    [SerializeField]
    private float _checkRange = 0.2f;
    [SerializeField]
    private int _minConunt = 1;
    [SerializeField]
    private LayerMask _physicsLayer = default;
    /// <summary>
    /// 壁判定結果を返す
    /// </summary>
    /// <returns></returns>
    public bool IsWalled()
    {
        int hitCount = default;
        foreach (var pos in _checkStartPoint)
        {
            Vector3 start = pos.position;
            Vector3 end = start + _checkDir * _checkRange;
            if (Physics.Linecast(start, end, _physicsLayer))
            {
                hitCount++;
                if (hitCount > _minConunt)//最小判定数以上なら壁
                {
                    return true;
                }
            }
        }
        return false;
    }
}
