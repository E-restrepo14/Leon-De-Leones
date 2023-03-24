using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawn1;

    [SerializeField]
    private GameObject spawn2;

    [SerializeField]
    private GameObject spawn3;

    public List<GameObject> enemigosMuertos;



    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("player"))
        {
            StartCoroutine(SpawnearEnemigos());
        }
    }

    // los enemigos deben poder acceder a este script una vez mueran
    public void AlmacenarSoldadoMuerto(GameObject soldado)
    {
        enemigosMuertos.Add(soldado);
    }

    //=========================================================================================================================================================

  

        // este proceso solo se debe ejecutar cuando le ordenen, para que no espawnee nada antes de tiempo
    public IEnumerator SpawnearEnemigos()
    {
        if(enemigosMuertos.Count >= 3)
        {
            print(enemigosMuertos.Count);


            print(enemigosMuertos[0]);
            enemigosMuertos[0].GetComponent<EnemyLogic>().Revivir(spawn1.transform.position);

            print(enemigosMuertos[1]);
            enemigosMuertos[1].GetComponent<EnemyLogic>().Revivir(spawn2.transform.position);

            print(enemigosMuertos[2]);
            enemigosMuertos[2].GetComponent<EnemyLogic>().Revivir(spawn3.transform.position);

            enemigosMuertos.Remove(enemigosMuertos[2]);
            enemigosMuertos.Remove(enemigosMuertos[1]);
            enemigosMuertos.Remove(enemigosMuertos[0]);
        }

        yield return new WaitForSeconds(8);

        if (enemigosMuertos.Count >= 3)
        {
            print(enemigosMuertos.Count);

            print(enemigosMuertos[0]);
            enemigosMuertos[0].GetComponent<EnemyLogic>().Revivir(spawn1.transform.position);

            print(enemigosMuertos[1]);
            enemigosMuertos[1].GetComponent<EnemyLogic>().Revivir(spawn2.transform.position);

            print(enemigosMuertos[2]);
            enemigosMuertos[2].GetComponent<EnemyLogic>().Revivir(spawn3.transform.position);

            enemigosMuertos.Remove(enemigosMuertos[2]);
            enemigosMuertos.Remove(enemigosMuertos[1]);
            enemigosMuertos.Remove(enemigosMuertos[0]);
        }
    }



    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +  en una lista se puede acceder al numero de elementos que almacena.
    //  int j = UnityEngine.Random.Range(0, enemigosMuertos.Count);

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +  en una lista se pueden eliminar elementos que esten almacenados en ella.
    // enemigosMuertos.Remove(enemigosMuertos[j2]);

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + en una lista se puede acceder a un elemento especifico almacenado en ella.
    // (enemigosMuertos[j2]);

}
