using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletE : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PoolingObj>().ReturnObjBulletE(gameObject);
        }
    }
}
