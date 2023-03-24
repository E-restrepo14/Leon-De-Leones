using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    //tengo pensado en instanciar enemigos e irlos colocando en escena, estas variables deben asignarse solitas... deben tener una funcion de revivir, para no utilizar instanciacion 

    public float enemyMoveSpeed;
    public Transform player;
    public float detectionRange;
    Animator enemyAnimator;
    public bool estaVivo;
    public Rigidbody enemyRb;
    public GameObject arrow;
    public GameObject arrowplace;
    [SerializeField]
    private GameObject brokenShield;
    [SerializeField]
    private GameObject powerUp;
    [SerializeField]
    private GameObject item;
    [SerializeField]
    private HudManager myHudManager;
    [SerializeField]
    private GameObject hudManagerParent;

    private void Awake()
    {
        hudManagerParent = GameObject.Find("Manager");
        myHudManager = hudManagerParent.GetComponent<HudManager>();
        enemyRb = GetComponent<Rigidbody>();
        estaVivo = true;
        player = GameObject.Find("player").transform;
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyAnimator.SetBool("descansando", true);
    }

    private void Update()
    {
        if (estaVivo == true)
        {
            float currentDistance = Vector3.Distance(transform.position, player.position);

            if (currentDistance <= detectionRange)
            {
                enemyAnimator.SetBool("descansando", false);
                enemyAnimator.SetBool("persiguiendo", true);

                float rotSpeed = 360f;
                Vector3 D = player.transform.position - transform.position;
                Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime );
                transform.rotation = rot;
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
;
                transform.Translate(Vector3.forward * Time.deltaTime * enemyMoveSpeed);
            }

            if (currentDistance > detectionRange)
            {
                enemyAnimator.SetBool("descansando", true);
                enemyAnimator.SetBool("persiguiendo", false);
            }
        }
    }

    // por medio de un evento de animacion se llama, por lo que deben ser publicos y no cambiarles el nombre
    public void DispararFlecha()
    {
        GameObject newArrow = Instantiate(arrow);
        newArrow.transform.position = arrowplace.transform.position;
        newArrow.transform.rotation = arrowplace.transform.rotation;
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        Destroy(newArrow, 1);
        arrowplace.GetComponent<Renderer>().enabled = false;
    }

    private void InstanciarEscudoRoto (Transform spawnPosition)
    {
        GameObject newBrokenShield = Instantiate(brokenShield);
        newBrokenShield.transform.position = spawnPosition.position;
        newBrokenShield.transform.rotation = spawnPosition.rotation;
        newBrokenShield.GetComponent<Rigidbody>().velocity = transform.up * 1;
        newBrokenShield.GetComponent<Rigidbody>().AddTorque(transform.forward * 5 * 5);

        Destroy(newBrokenShield, 1f);
    }

    // por medio de un evento de animacion se llama, por lo que deben ser publicos y no cambiarles el nombre
    public void AgarrarFlecha()
    {
        arrowplace.GetComponent<Renderer>().enabled = true;   
    }

    private IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(0.5F);
        GameObject powerUpDropped = Instantiate(item);
        powerUpDropped.transform.position = powerUp.transform.position;
        powerUpDropped.transform.rotation = powerUp.transform.rotation;

    }

    void Morir()
    {
        StartCoroutine(SpawnPowerUp());
        enemyAnimator.SetBool("muerto", true);
        enemyRb.isKinematic = true;
        enemyRb.detectCollisions = false;
        GetComponent<AudioSource>().Play(0);
        myHudManager.ModificarScore(+5);

        if (detectionRange < 8 && estaVivo == true)
        {
            hudManagerParent.GetComponent<SpawnManager>().AlmacenarSoldadoMuerto(this.gameObject);
        }

        estaVivo = false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("espadaTag"))
        {
            Morir();
            if(collision.transform.name == ("escudo arrojadizo(Clone)"))
            {
                Destroy(collision.gameObject,0);
                InstanciarEscudoRoto(collision.transform);
            }

            if (collision.transform.name == ("dagaArrojadiza(Clone)"))
            {
                Destroy(collision.gameObject, 0);
            }
            

        }

        else
        {
            if (collision.transform.CompareTag("player"))
            {
                if(collision.gameObject.GetComponent<ControladorPersonaje>().aunTieneEscudo == true)
                {
                    collision.gameObject.GetComponent<ControladorPersonaje>().aunTieneEscudo = false;
                    collision.gameObject.GetComponent<ControladorPersonaje>().animator.Play("Base Layer.atack", 0, 0f);
                    InstanciarEscudoRoto(collision.transform);
                }
                else
                collision.gameObject.GetComponent<ControladorPersonaje>().RecibirDaño();
                
                Morir();
            }
        }
    }

    public void Revivir(Vector3 spawnPosition)
    {
        enemyAnimator.SetBool("descansando", true);
        enemyAnimator.SetBool("persiguiendo", false);
        enemyAnimator.SetBool("muerto", false);
        enemyRb.isKinematic = false;
        enemyRb.detectCollisions = true;
        transform.position = spawnPosition;
        StartCoroutine(Alinearse());


    }

  private IEnumerator Alinearse()
    {
        enemyAnimator.SetBool("descansando", false);
        enemyAnimator.SetBool("persiguiendo", true);

        while (transform.position.z < 0  )
        {
            
            transform.rotation = Quaternion.Euler(0, 0, 0);
            
            transform.Translate(Vector3.forward * Time.deltaTime * enemyMoveSpeed);
            yield return new WaitForSeconds(0);
        }

        detectionRange = 50f;
        estaVivo = true;
        print("me revivi");


    }

}
