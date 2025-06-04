using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObj : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab, BulletPrefabE;
    [SerializeField] private GameObject[] EnemyPrefab;
    private Queue<GameObject> poolBullet = new Queue<GameObject>();
    private Queue<GameObject> poolBulletE = new Queue<GameObject>();
    private Queue<GameObject> poolEnemy = new Queue<GameObject>();
    public static PoolingObj Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    /// poolbullet
    public GameObject GetObjBullet()
    {
        if (poolBullet.Count > 0)
        {
            GameObject obj = poolBullet.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(BulletPrefab);
    }
    public void ReturnObjBullet(GameObject obj)
    {
        obj.SetActive(false);
        poolBullet.Enqueue(obj);
    }

    public GameObject GetObjBulletE()
    {
        if (poolBulletE.Count > 0)
        {
            GameObject obj = poolBulletE.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(BulletPrefabE);
    }
    public void ReturnObjBulletE(GameObject obj)
    {
        obj.SetActive(false);
        poolBulletE.Enqueue(obj);
    }
    ///poolenemy
    public GameObject GetobjEnemy()
    {
        if(poolEnemy.Count > 0)
        {
            GameObject obj = poolEnemy.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        int index = Random.Range(0, EnemyPrefab.Length);
        return Instantiate(EnemyPrefab[index]);
    }
    public void ReturnObjEnemy(GameObject obj)
    {
        obj.SetActive(false);
        poolEnemy.Enqueue(obj);
    }
}
