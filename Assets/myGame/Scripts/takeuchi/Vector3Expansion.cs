using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector3�̂݊g������
/// </summary>
public static class Vector3Expansion
{
    /// <summary>
    /// �w��ʔ͈͂Ŋg�U���������_����Vector3��Ԃ�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="diffusivity"></param>
    /// <returns></returns>
    public static Vector3 Diffusivity(this Vector3 target, float diffusivity)
    {
        if (diffusivity > 0)
        {
            target.x += Random.Range(-diffusivity, diffusivity);
            target.y += Random.Range(-diffusivity, diffusivity);
            target.z += Random.Range(-diffusivity, diffusivity);
        }
        return target;
    }
    /// <summary>
    /// �w��ʋ��`�͈͂Ŋg�U���������_����Vector3��Ԃ�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="diffusivity"></param>
    /// <returns></returns>
    public static Vector3 DiffusivitySufia(this Vector3 target, float diffusivity)
    {
        if (diffusivity > 0)
        {
            Vector3 pos = target.Diffusivity(diffusivity).normalized;
            pos *= Random.Range(-diffusivity, diffusivity);
            return pos;
        }
        return target;
    }
    /// <summary>
    /// �w��ʔ͈͂Ŋg�U���������_����Vector3��Ԃ�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="baseVector"></param>
    /// <param name="diffusivity"></param>
    /// <returns></returns>
    public static Vector3 Diffusivity(this Vector3 target, Vector3 baseVector, float diffusivity)
    {
        if (diffusivity > 0)
        {
            var next = target.DiffusivitySufia(diffusivity);
            next.x *= baseVector.x;
            next.y *= baseVector.y;
            next.z *= baseVector.z;
            target += next;
        }
        return target;
    }
}
