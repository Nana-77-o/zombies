using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUPManager : MonoBehaviour
{
    [SerializeField, Tooltip("最初に生成するまでの時間")] 
    float spawnDelay = 2;
    [SerializeField, Tooltip("次に生成するまでの時間")] 
    float _interval = 10;
    [SerializeField, Tooltip("Player")]
    GameObject _player = default;
    [SerializeField, Tooltip("生成オブジェクト")] 
    GameObject _powerUpObj = default;
    [SerializeField, Tooltip("配置位置")] 
    GameObject[] _randmPoint = default;

    //private PoewrUPType type;

    //public PoewrUPType Type { get => type; set => type = value; }

    //public enum PoewrUPType
    //{
    //    Always,
    //    Temporary
    //}
    //private void Start()
    //{
    //    for (int i = 0; i < _randmPoint.Length; i++)
    //    {
    //        var obj = Instantiate(_powerUpObj, _randmPoint[i].transform);
    //        _player.GetComponent<PlayerParameter>().Manager = this;
    //        obj.GetComponent<PowerUP>().Player = _player;

    //        var type = UnityEngine.Random.Range(0, 2);
    //        _randmPoint[i].transform.GetChild(0);
    //        obj.gameObject.SetActive(true);
    //        _player.GetComponent<PlayerParameter>().Manager.Type = (PoewrUPType)Enum.ToObject(typeof(PoewrUPType), type);
    //    }
    //    InvokeRepeating("Creat", spawnDelay, _interval);
    //}
    //void Creat()
    //{
    //    var index = UnityEngine.Random.Range(0, _randmPoint.Length);
    //    var type = UnityEngine.Random.Range(0, 2);
    //    var obj = _randmPoint[index].transform.GetChild(0);
    //    obj.gameObject.SetActive(true);
    //    _player.GetComponent<PlayerParameter>().Manager.Type = (PoewrUPType)Enum.ToObject(typeof(PoewrUPType), type);
    //}
}
