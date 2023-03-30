using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 破壊可能オブジェクトの生成
/// </summary>
public class DestructionManager : MonoBehaviour
{
    [SerializeField, Tooltip("生成個数")] int _generationCount;
    [SerializeField, Tooltip("生成オブジェクト")] GameObject _generationObj;
    [SerializeField, Tooltip("配置位置")] GameObject[] _randmPoint;

    private void Start()
    {
        GenerationObj();
    }

    /// <summary>
    /// ランダム生成
    /// </summary>
    public void GenerationObj()
    {
        for (int i = 0; i < _generationCount;)
        {
            var index = Random.Range(0, _randmPoint.Length);
            if (_randmPoint[index].activeSelf == false)
            { 
                _randmPoint[index].SetActive(true);
                var gm = ObjectPoolManager.Instance.Use(_generationObj, _randmPoint[index].transform.position);
                gm.GetComponent<Destruction>().Destructionmanager = this;
                i++;
            }
        }
    }

    public void GameobjCount()
    {
        ObjectPoolManager.Instance.GetCount(_generationObj);
        if (ObjectPoolManager.Instance.GetCount(_generationObj) == 0)//破壊可能オブジェクトが0個になった時
        {
            Debug.Log("0になった");
        }
    }
}
