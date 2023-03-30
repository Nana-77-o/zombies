using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Tooltip("�����ɂ��鑺�l���o��������ꏊ"), SerializeField]
    Transform _appearancePos;

    bool _emptyHouse = false;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_emptyHouse && collision.gameObject.name == "Player")
        {
            if (VillagerGenerator.villagerGenerator.VillagerCall(this))
            {
                GameObject outsideMurabito = ObjectPoolManager.Instance.Use(VillagerGenerator.villagerGenerator.outsideVillager);
                outsideMurabito.transform.position = _appearancePos.position;
                outsideMurabito.SetActive(true);
            }
            _emptyHouse = true;
        }
    }
}
