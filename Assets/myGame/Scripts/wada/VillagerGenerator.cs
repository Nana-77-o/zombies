using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerGenerator : MonoBehaviour
{
    [Tooltip("マップ上の家's"), SerializeField]
    private List<House> _homes;

    [Tooltip("屋内で助けを求める村人の数"), SerializeField]
    private int _helpVillagersNum;

    [Tooltip("屋外の村人生成位置's"), SerializeField]
    private Transform[] _villagersSpawnPoint;

    [Tooltip("ゲーム開始時の屋外の村人の数"), SerializeField]
    private int _villagersSpawnNum = 5;

    [Tooltip("屋外の村人生成間隔"), SerializeField]
    private float _villagersSpawnTime = 10f;

    [Tooltip("屋外にいる村人")]
    public GameObject insideVillager;

    [Tooltip("屋内にいる村人")]
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
    /// 家に村人がいるか否か
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
