using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntante : MonoBehaviour
{
    public PoolingObj EnemyPool;

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject enemy = EnemyPool.GetobjEnemy();
            Vector3 randomPos = new Vector3(
                Random.Range(-23f, 25f),
                0.3f, 
                Random.Range(-25f, 23f)
            );
            enemy.transform.position = randomPos;

            CharacterController characterController = enemy.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
                characterController.transform.position = randomPos;
                characterController.enabled = true;
            }
            Renderer childRenderer = enemy.GetComponentInChildren<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length < 15)
        {
            GameObject enemy = EnemyPool.GetobjEnemy();
            Vector3 randomPos = new Vector3(
                Random.Range(-23f, 25f),
                0.3f,
                Random.Range(-25f, 23f)
            );
            enemy.transform.position = randomPos;

            CharacterController characterController = enemy.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
                characterController.transform.position = randomPos;
                characterController.enabled = true;
            }
            Renderer childRenderer = enemy.GetComponentInChildren<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}
