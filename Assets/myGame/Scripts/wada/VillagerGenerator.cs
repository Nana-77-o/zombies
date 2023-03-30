using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerGenerator : MonoBehaviour
{
    [Tooltip("�}�b�v��̉�'s"), SerializeField]
    private List<House> _homes;

    [Tooltip("�����ŏ��������߂鑺�l�̐�"), SerializeField]
    private int _helpVillagersNum;

    [Tooltip("���O�̑��l�����ʒu's"), SerializeField]
    private Transform[] _villagersSpawnPoint;

    [Tooltip("�Q�[���J�n���̉��O�̑��l�̐�"), SerializeField]
    private int _villagersSpawnNum = 5;

    [Tooltip("���O�̑��l�����Ԋu"), SerializeField]
    private float _villagersSpawnTime = 10f;

    [Tooltip("���O�ɂ��鑺�l")]
    public GameObject insideVillager;

    [Tooltip("�����ɂ��鑺�l")]
    public GameObject outsideVillager;

    private bool _isVillagersSpawn;

    public static VillagerGenerator villagerGenerator;



    private void Awake()
    {
        villagerGenerator = this.GetComponent<VillagerGenerator>();
    }
    void Start()
    {
        if (_helpVillagersNum > _homes.Count)
        {
            _helpVillagersNum = _homes.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �Ƃɑ��l�����邩�ۂ�
    /// </summary>
    public bool VillagerCall(House house)
    {
        if (_helpVillagersNum < _homes.Count)
        {
            if (Random.Range(0, _homes.Count) <= _helpVillagersNum - 1)
            {
                _helpVillagersNum--;
                return true;
            }
            _homes.Remove(house);
            return false;

        }
        else
        {
            _homes.Remove(house);
            _helpVillagersNum--;
            return true;
        }
    }

    //private IEnumerator VillagerGenerat()
    //{

    //}
}
