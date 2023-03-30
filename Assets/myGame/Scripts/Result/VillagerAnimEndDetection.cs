using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAnimEndDetection : MonoBehaviour
{
    public bool IsAnimEnd { get; private set; } = false;


    private void OnAnimEnd()
    {
        IsAnimEnd = true;
    }
}
