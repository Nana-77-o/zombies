using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSet : MonoBehaviour
{
    NaviUser _myNaviUser;
    GameObject[] _otherBosses;

    private void Awake()
    {
        _otherBosses = GameObject.FindGameObjectsWithTag("BossEnemy");
        foreach(GameObject bosses in _otherBosses)
        {
            //_myNaviUser.
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
