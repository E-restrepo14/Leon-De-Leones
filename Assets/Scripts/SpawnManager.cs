using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemySpawns;
    [SerializeField] List<GameObject> deadEnemies;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("player"))
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    public void CollectDeadWarrior(GameObject warrior) // ABSTRACTION
    {
        deadEnemies.Add(warrior);
    }

    private IEnumerator SpawnEnemies() // ABSTRACTION
    {
        for(int i = 0; i < 3; i++)
        {
            deadEnemies[0].GetComponent<EnemyLogic>().Revivir(enemySpawns[i].transform.position);
            deadEnemies.Remove(deadEnemies[0]);
        }
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 3; i++)
        {
            deadEnemies[0].GetComponent<EnemyLogic>().Revivir(enemySpawns[i].transform.position);
            deadEnemies.Remove(deadEnemies[0]);
        }
    }
}
