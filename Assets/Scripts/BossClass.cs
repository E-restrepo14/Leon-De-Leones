using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClass : MonoBehaviour
{


    [SerializeField]
    private float bossLife = 10f;
    [SerializeField]
    private float playerDamage = 2;
    Animator bossAnimator;
    [SerializeField]
    private float slowForce;
    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject horse1;
    [SerializeField]
    private GameObject horse2;
    [SerializeField]
    private GameObject conductor;
    [SerializeField]
    private bool estaVivo;
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float currentDistance;

    [SerializeField]
    private float finalPositionDistance;
    [SerializeField]
    private GameObject crashedDoor;
    public Transform finalPosition;


    private void Awake()
    {
        conductor.GetComponent<Rigidbody>().isKinematic = true;
        conductor.GetComponent<Rigidbody>().detectCollisions = false;

        bossAnimator = GetComponent<Animator>();
        GetComponent<AudioSource>().volume = 0;
        StartCoroutine(DesfasarCaballos());
        player = GameObject.Find("player").transform;
        estaVivo = true;
        detectionRange = 25f;
        currentDistance = Vector3.Distance(transform.position, player.position);

    }

    // este evento se llama mediante eventos de animacion, debe ser publico y no se debe renombrar
    public void VerificarSalud()
    {
        if (estaVivo == false)
        {
            conductor.GetComponent<Animator>().enabled = false;
            conductor.transform.SetParent(null);
            conductor.GetComponent<Rigidbody>().isKinematic = true;
            conductor.GetComponent<Rigidbody>().detectCollisions = true;

            //   StartCoroutine(Frenar());
            ChocarPuerta();

            
        }
    }

    public IEnumerator Frenar()
    {
        horse1.GetComponent<Animator>().SetBool("isidle", true);
        horse2.GetComponent<Animator>().SetBool("isidle", true);
        transform.tag = "suelo";
        while (bossAnimator.speed > 0)
        {
            

            bossAnimator.speed -= slowForce;
            if (bossAnimator.speed <= 0)
            {
                bossAnimator.speed = 0;
                GetComponent<AudioSource>().volume = 0;
            }

            transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, Mathf.Lerp(0, transform.position.z, bossAnimator.speed));
            GetComponent<AudioSource>().volume = bossAnimator.speed;
            yield return new WaitForSeconds(0f);
        }

        transform.tag = "suelo";
        GetComponent<AudioSource>().enabled = false;

    }


    public void ChocarPuerta()
    {
        // este script consistirá en, frenar de golpe la velocidad de la animacion del carruaje, pero darle tambien la aceleracion instantanea al carro
        bossAnimator.Play("Base Layer.crash Door pattern", 0, 0f);    
    }

    private void Update()
    {
        
            currentDistance = Vector3.Distance(transform.position, player.position);

            

            if (currentDistance <= detectionRange)
            {
                GetComponent<AudioSource>().volume = (1-(currentDistance / detectionRange)) ;         
            }
        
    }

    public void RecibirDaño()
    {
        bossLife -= playerDamage;
        print(bossLife);

        if (bossLife <= 0)
        {
            StartCoroutine(Morir());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("doorTag"))
        {
            crashedDoor.SetActive(true);

            Destroy(other.gameObject, 0);
        }
    }

    private IEnumerator DesfasarCaballos()
    {
        horse1.GetComponent<Animator>().Play("Base Layer.run", 0, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator Morir()
    {
        if(estaVivo == false)
        {
            conductor.GetComponent<Animator>().enabled = false;
            conductor.transform.SetParent(null);
            conductor.GetComponent<Rigidbody>().isKinematic = true;
            conductor.GetComponent<Rigidbody>().detectCollisions = true;
        }

        estaVivo = false;   


        yield return new WaitForSeconds(0);      
    }


}
