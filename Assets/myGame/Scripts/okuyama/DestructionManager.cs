using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �j��\�I�u�W�F�N�g�̐���
/// </summary>
public class DestructionManager : MonoBehaviour
{
    [SerializeField, Tooltip("������")] int _generationCount;
    [SerializeField, Tooltip("�����I�u�W�F�N�g")] GameObject _generationObj;
    [SerializeField, Tooltip("�z�u�ʒu")] GameObject[] _randmPoint;

    private void Start()
    {
        GenerationObj();
    }

    /// <summary>
    /// �����_������
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
        if (ObjectPoolManager.Instance.GetCount(_generationObj) == 0)//�j��\�I�u�W�F�N�g��0�ɂȂ�����
        {
            Debug.Log("0�ɂȂ���");
        }
    }
}
