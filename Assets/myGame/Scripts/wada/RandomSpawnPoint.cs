using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _start = default;
    [SerializeField]
    private Transform _end = default;
    public Vector3 GetSpawnPos()
    {
        return new Vector3(Random.Range(_start.position.x, _end.position.x), transform.position.y, Random.Range(_start.position.z, _end.position.z));
    }
}
