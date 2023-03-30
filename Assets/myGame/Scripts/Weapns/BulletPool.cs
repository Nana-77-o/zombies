using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private const int DEFAULT_BULLET_COUNT = 50;
    private static BulletPool instance;
    public static BulletPool Instance
    {
        get 
        { 
            if (instance == null)
            {
                var obj = new GameObject("BulletPool");
                instance = obj.AddComponent<BulletPool>();
                instance._bulletDic = new Dictionary<string, List<WeaponBullet>>();
            }
            return instance; 
        }
    }
    private Dictionary<string, List<WeaponBullet>> _bulletDic = default;
    private void CreateBulletPool(WeaponBullet newBullet)
    {
        var bulletList = new List<WeaponBullet>();
        for (int i = 0; i < DEFAULT_BULLET_COUNT; i++)
        {
            var bullet = Instantiate(newBullet,transform);
            bullet.gameObject.SetActive(false);
            bulletList.Add(bullet);
        }
        _bulletDic.Add(newBullet.gameObject.name, bulletList);
    }
    public WeaponBullet GetBullet(WeaponBullet bullet)
    {
        if (!_bulletDic.ContainsKey(bullet.gameObject.name))
        {
            CreateBulletPool(bullet); 
        }
        foreach (var poolBullet in _bulletDic[bullet.gameObject.name])
        {
            if (poolBullet.gameObject.activeInHierarchy)
            {
                continue;
            }
            return poolBullet;
        }
        var newBullet = Instantiate(bullet, transform);
        newBullet.gameObject.SetActive(false);
        _bulletDic[bullet.gameObject.name].Add(newBullet);
        return newBullet;
    }
}
