using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    [SerializeField]
    private ElapsedTimeObject[] _houses = default;
    [SerializeField]
    private int _spwanCount = 10;
    private void Start()
    {
        ShuffleHouse();
        for (int i = 0; i < _spwanCount; i++)
        {
            _houses[i].StartChack();
        }
    }
    private void ShuffleHouse()
    {
        for (int i = 0; i < _houses.Length; i++)
        {
            var r = Random.Range(0, _houses.Length);
            var randamHouse = _houses[r];
            _houses[r] = _houses[i];
            _houses[i] = randamHouse;
        }
    }
}
