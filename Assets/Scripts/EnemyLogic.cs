using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    //tengo pensado en instanciar enemigos e irlos colocando en escena, estas variables deben asignarse solitas... deben tener una funcion de revivir, para no utilizar instanciacion 

    
    public float detectionRange;

    [SerializeField] 
    GameObject arrow;
    [SerializeField] 
    GameObject arrowplace;
    [SerializeField]
    float enemyMoveSpeed; 
    [SerializeField]
    private GameObject brokenShield;
    [SerializeField]
    private GameObject powerUpSpawnPosition;
    [SerializeField]
    private GameObject item;

    private Animator enemyAnimator;
    private GameObject m_spawnManager;
    private Transform player;
    private bool isAlive;
    private Rigidbody enemyRb;


    private void Awake()
    {
        m_spawnManager = GameObject.Find("Manager");
        enemyRb = GetComponent<Rigidbody>();
        isAlive = true;
        player = GameObject.Find("player").transform;
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyAnimator.SetBool("descansando", true);
    }

    private void Update()
    {
        if (isAlive == true)
        {
            float currentDistance = Vector3.Distance(transform.position, player.position);

            if (currentDistance <= detectionRange)
            {
                enemyAnimator.SetBool("descansando", false);
                enemyAnimator.SetBool("persiguiendo", true);
                RunTowardsPlayer();

            }

            if (currentDistance > detectionRange)
            {
                enemyAnimator.SetBool("descansando", true);
                enemyAnimator.SetBool("persiguiendo", false);
            }
        }
    }

    private void RunTowardsPlayer()   // ABSTRACTION
    {
        float rotSpeed = 360f;
        Vector3 dir = player.transform.position - transform.position;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.Translate(Vector3.forward * Time.deltaTime * enemyMoveSpeed);
    }

    // this is called by animation events, it must be public and the name can't be changed.
    public void DispararFlecha()   // ABSTRACTION
    {
        GameObject newArrow = Instantiate(arrow);
        newArrow.transform.position = arrowplace.transform.position;
        newArrow.transform.rotation = arrowplace.transform.rotation;
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        Destroy(newArrow, 1);
    }

    private void InstantiateBrokenShield (Transform spawnPosition)  // ABSTRACTION
    {
        GameObject newBrokenShield = Instantiate(brokenShield);
        newBrokenShield.transform.position = spawnPosition.position;
        newBrokenShield.transform.rotation = spawnPosition.rotation;
        newBrokenShield.GetComponent<Rigidbody>().velocity = transform.up * 1;
        newBrokenShield.GetComponent<Rigidbody>().AddTorque(transform.forward * 5 * 5);
        Destroy(newBrokenShield, 1f);
    }

    private IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(0.5F);
        GameObject powerUpDropped = Instantiate(item);
        powerUpDropped.transform.position = powerUpSpawnPosition.transform.position;
        powerUpDropped.transform.rotation = powerUpSpawnPosition.transform.rotation;
    }

    void Morir()
    {
        StartCoroutine(SpawnPowerUp());
        enemyAnimator.SetBool("muerto", true);
        enemyRb.isKinematic = true;
        enemyRb.detectCollisions = false;
        GetComponent<AudioSource>().Play(0);
        if (detectionRange < 8 && isAlive == true)
        {
            m_spawnManager.GetComponent<SpawnManager>().CollectDeadWarrior(this.gameObject);
        }
        isAlive = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("espadaTag"))
        {
            if(collision.transform.name == ("escudo arrojadizo(Clone)"))
            {
                Destroy(collision.gameObject,0);
                InstantiateBrokenShield(collision.transform);
            }
            if (collision.transform.name == ("dagaArrojadiza(Clone)"))
            {
                Destroy(collision.gameObject, 0);
            }
            Morir();
        }

        else
        {
            if (collision.transform.CompareTag("player"))
            {
                if(player.GetComponent<ControladorPersonaje>().hasShield == true)
                {
                    player.GetComponent<ControladorPersonaje>().hasShield = false;
                    player.GetComponent<ControladorPersonaje>().animatePlayerAtack();
                    InstantiateBrokenShield(collision.transform);
                }
                else
                player.GetComponent<ControladorPersonaje>().PlayerTakeDamage(); 
                Morir();
            }
        }
    }

    public void Revivir(Vector3 spawnPosition)  // ABSTRACTION
    {
        enemyAnimator.SetBool("muerto", false);
        enemyRb.isKinematic = false;
        enemyRb.detectCollisions = true;
        transform.position = spawnPosition;
        StartCoroutine(LineUpenemies());
    }

  private IEnumerator LineUpenemies()  // ABSTRACTION
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
        isAlive = true;
    }
}
