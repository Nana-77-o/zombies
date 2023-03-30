using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector3‚Ì‚ÝŠg’£‚·‚é
/// </summary>
public static class Vector3Expansion
{
    /// <summary>
    /// Žw’è—Ê”ÍˆÍ‚ÅŠgŽU‚µ‚½ƒ‰ƒ“ƒ_ƒ€‚ÈVector3‚ð•Ô‚·
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
    /// Žw’è—Ê‹…Œ`”ÍˆÍ‚ÅŠgŽU‚µ‚½ƒ‰ƒ“ƒ_ƒ€‚ÈVector3‚ð•Ô‚·
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
    /// Žw’è—Ê”ÍˆÍ‚ÅŠgŽU‚µ‚½ƒ‰ƒ“ƒ_ƒ€‚ÈVector3‚ð•Ô‚·
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
