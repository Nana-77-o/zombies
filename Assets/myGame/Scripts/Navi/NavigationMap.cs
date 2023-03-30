using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// �U���}�b�v
/// </summary>
public class NavigationMap
{
    private Thread _therad = default;
    private List<NaviPoint> _naviMap = default;
    private SearchMap<NaviPoint> _searchMap = new SearchMap<NaviPoint>();
    /// <summary> �U���_���X�g </summary>
    public List<NaviPoint> NaviMap { get { return _naviMap; } }
    public NavigationMap(List<NaviPoint> naviMap)
    {
        _naviMap = naviMap;
    }
    /// <summary>
    /// �T������
    /// </summary>
    /// <param name="targetKey"></param>
    private void MakeFootprints(in string targetKey, NaviPoint point,in int power)
    {
        _searchMap.ResetFootprints(targetKey);
        _searchMap.StartMakeFootprints(point, targetKey, power);
    }
    /// <summary>
    /// �w��Ώۂ̒T��������ʃX���b�h�ōs��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetKey"></param>
    /// <param name="power"></param>
    public void MakeFootprints(in Transform target, string targetKey, int power)
    {
        NaviPoint tPoint = null;
        float dis = float.MaxValue;
        foreach (var point in _naviMap)
        {
            float range = Vector3.Distance(point.Pos, target.position);
            if (range < dis)
            {
                dis = range;
                tPoint = point;
            }
        }
        if (tPoint == null) { return; }
        _therad = new Thread(new ThreadStart(() => MakeFootprints(targetKey, tPoint, power)));
        _therad.Start();
    }
    public void ResetFootprints(in string targetKey)
    {
        _searchMap.ResetFootprints(targetKey);
    }
    /// <summary>
    /// ���[�U�[�̈ړ�������Ԃ�
    /// </summary>
    /// <param name="user"></param>
    /// <param name="targetKey"></param>
    /// <returns></returns>
    public Vector3 GetMoveDir(in Transform user, in string targetKey)
    {
        NaviPoint uPoint = null;
        float dis = float.MaxValue;
        float range;
        foreach (var point in _naviMap)
        {
            if (point.IsNoEntry == true)
            {
                continue;
            }
            range = Vector3.Distance(point.Pos, user.position);
            if (range < dis)
            {
                dis = range;
                uPoint = point;
            }
        }
        if (uPoint == null || !uPoint.FootprintDic.ContainsKey(targetKey)) { return Vector3.zero; }
        Vector3 pos = Vector3.zero;
        dis = float.MaxValue;
        foreach (var target in uPoint.ConnectPoint)
        {
            if (!target.FootprintDic.ContainsKey(targetKey))
            {
                continue;
            }
            range = Vector3.Distance(target.Pos, user.position);
            if (uPoint.FootprintDic[targetKey] + 1 == target.FootprintDic[targetKey] && dis > range)
            {
                dis = range;
                pos = target.Pos;
            }
        }
        if (dis == float.MaxValue) { return Vector3.zero; }
        var dir = pos - user.position;
        dir.y = 0;
        return dir.normalized;
    }
    /// <summary>
    /// ���[�U�[�̌��n�_�̑��Ղ�Ԃ�
    /// </summary>
    /// <param name="user"></param>
    /// <param name="targetKey"></param>
    /// <returns></returns>
    public int GetFootprints(in Transform user,in string targetKey)
    {
        NaviPoint uPoint = null;
        float dis = float.MaxValue;
        float range;
        foreach (var point in _naviMap)
        {
            if (point.IsNoEntry == true)
            {
                continue;
            }
            range = Vector3.Distance(point.Pos, user.position);
            if (range < dis)
            {
                dis = range;
                uPoint = point;
            }
        }
        if (uPoint == null || !uPoint.FootprintDic.ContainsKey(targetKey))
        {
            return 0;
        }
        return uPoint.FootprintDic[targetKey];
    }
}
