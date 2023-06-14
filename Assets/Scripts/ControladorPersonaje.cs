using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{

    [SerializeField]
    private GameObject escudoAtacante;
    [SerializeField]
    private GameObject escudoDefensor;
    [SerializeField]
    private GameObject shieldThrowed;
    [SerializeField]
    private GameObject brokenShield;
    [SerializeField]
    private HudManager myHudManager;

    [SerializeField]
    private GameObject dagaArrojadiza;

    // lo va a cambiar la clase enemigo por lo que debe ser publico
    public bool hasShield;
    [SerializeField]
    private bool hasDagger;
    [SerializeField]
    private bool lanzoEscudo;
    [SerializeField]
    private bool lanzoLaDaga;

    public GameObject hudManagerParent;
    [SerializeField] GameObject dangerSignal;
    [SerializeField] GameObject dodgeSignal;
    public Animator animator;
    public float playerLife = 10f;
    public float jumpForce = 250f;
    public bool hasLanded;
    public bool isGameOver;

    Rigidbody m_Rigidbody;
    Vector3 initialPosition;

    private void Start()
    {
        myHudManager = hudManagerParent.GetComponent<HudManager>();
        hasShield = false;
        hasDagger = false;
        lanzoEscudo = false;
        lanzoLaDaga = false;
        initialPosition = transform.position;
        playerLife = 10f;
        animator = gameObject.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    public void Reintentar()
    {
        playerLife = 10f;
        isGameOver = false;
        transform.position = initialPosition;
    }

    //debe ser publica porque a esto accede el evento de animacion, no cambiarle el nombre
    public void VerificarManoDeEscudo()
    {
        if (hasShield == false & escudoDefensor.GetComponent<Renderer>().isVisible)
        {
            CambiarEscudoDeMano();
        }

    }


    IEnumerator TakeShield()
    {
        hasShield = true;
        animator.SetBool("hadshield", true);
        escudoDefensor.GetComponent<Renderer>().enabled = true;
        escudoAtacante.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0);
        animator.SetBool("hadshield", false);
    }

    IEnumerator TakeDagger()
    {
        hasDagger = true;
        animator.SetBool("hadshield", true);
        yield return new WaitForSeconds(0);
        animator.SetBool("hadshield", false);
    }

    void CambiarEscudoDeMano()
    {
        escudoDefensor.GetComponent<Renderer>().enabled = false;
        escudoAtacante.GetComponent<Renderer>().enabled = true;
    }

    void DestroyShields()
    {
        escudoDefensor.GetComponent<Renderer>().enabled = false;
        escudoAtacante.GetComponent<Renderer>().enabled = false;
    }

    // es publico porque se llama sola desde un evento de animacion, no cambiarle el nombre
    public void LanzarODestruirEscudo()
    {
        if (hasDagger == true & lanzoLaDaga == true)
        {
            GameObject newDaga = Instantiate(dagaArrojadiza);
            newDaga.transform.position = escudoAtacante.transform.position;
            newDaga.transform.rotation = transform.rotation;
            newDaga.GetComponent<Rigidbody>().velocity = transform.forward * 10;
            Destroy(newDaga, 2);

            lanzoLaDaga = false;
            hasDagger = false;
        }
        else
        {

            if (lanzoEscudo == true)
            {
                GameObject newShield = Instantiate(shieldThrowed);
                newShield.transform.position = escudoAtacante.transform.position;
                newShield.transform.rotation = escudoAtacante.transform.rotation;
                newShield.GetComponent<Rigidbody>().velocity = transform.forward * 10;
                Destroy(newShield, 2);
                escudoAtacante.GetComponent<Renderer>().enabled = false;

                DestroyShields();
                lanzoEscudo = false;
                hasShield = false;
            }
        }

        if (hasShield == false)
        {
            DestroyShields();
        }
        

    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded == false)
        {
            animator.Play("Base Layer.idle", 0, 0f);
        }

        if (collision.gameObject.CompareTag("suelo"))
        {
            animator.SetBool("isjumping", false);
            hasLanded = true;
        }

        if (collision.gameObject.CompareTag("finishTag"))
        {
            playerLife = 20f;
            VerifyHealth();
        }

    }

    private void InstanciarEscudoRoto(Transform spawnPosition)
    {
        GameObject newBrokenShield = Instantiate(brokenShield);
        newBrokenShield.transform.position = spawnPosition.position;
        newBrokenShield.transform.rotation = spawnPosition.rotation;
        newBrokenShield.GetComponent<Rigidbody>().velocity = transform.up * 1;
        newBrokenShield.GetComponent<Rigidbody>().AddTorque(transform.forward * 5 * 5);

        Destroy(newBrokenShield, 1f);
    }

    private void DirectDeath()  // ABSTRACTION
    {
        playerLife = 0;
        VerifyHealth();
    }

    private void ChangeCheckpoint(Transform newCheckpoint)  // ABSTRACTION
    {
        initialPosition = newCheckpoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("checkpoint"))
        {
            ChangeCheckpoint(other.transform);
        }

        if (other.gameObject.CompareTag("arrowTag") & dodgeSignal.activeSelf == false )
        {
            if(hasShield == true)
            {
                hasShield = false;
                InstanciarEscudoRoto(transform);
                animator.Play("Base Layer.charging", 0, 0f);
                DestroyShields();
            }
            else
            PlayerTakeDamage();
        }

       

        if (other.gameObject.CompareTag("endTag"))
        {
            DirectDeath();
        }

        if (other.gameObject.CompareTag("bossTag"))
        {
            if (hasLanded == true)
            {
                DirectDeath();
            }
            else
            {
                this.gameObject.transform.GetChild(0).GetComponent<AudioSource>().Play(0);
                other.GetComponent<BossClass>().RecibirDaño();
            }
        }

        if (other.gameObject.CompareTag("shieldTag"))
        {
            if (hasShield == false)
            {
                StartCoroutine(TakeShield());
                if (other.GetComponent<AudioSource>() != null)
                {
                    other.GetComponent<AudioSource>().Play(0);
                }
                Destroy (other.gameObject,0.5F);
            }
        }

        if (other.gameObject.CompareTag("dagaTag"))
        {
            if (hasDagger == false)
            {
                StartCoroutine(TakeDagger());
                if (other.GetComponent<AudioSource>() != null)
                {
                    other.GetComponent<AudioSource>().Play(0);
                }
                Destroy(other.gameObject, 0.5F);
            }
        }
    }

    public IEnumerator Rodar() // ABSTRACTION
    {
        dodgeSignal.SetActive(true);
        animator.SetBool("isrolling", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isrolling", false);
        dodgeSignal.SetActive(false);
    }

    public IEnumerator ThrowWeapon()  // ABSTRACTION
    {
        if (hasDagger == true)
        {
            lanzoLaDaga = true;
            animatePlayerAtack();
        }
        else if (hasShield == true)
        {
            lanzoEscudo = true;
            animatePlayerAtack();
        }
        yield return new WaitForSeconds(0);
    }


    public void Run(float direction,float speed) // ABSTRACTION
    {
        transform.Translate(speed, 0, 0,Space.World);
        animator.SetBool("isrunning", true);
        animator.SetBool("isidle", false);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, direction, 0), Time.deltaTime* 5f);
    }

    public void PlayerTakeDamage()
    {
        playerLife -= 2f;
        animator.Play("Base Layer.atack", 0, 0f);
        GetComponent<AudioSource>().Play(0);
        VerifyHealth();
    }

    private void VerifyHealth()
    {
        switch (playerLife)
        {
            case 0:
                myHudManager.StartCoroutine(myHudManager.GameOver());
                break;
            case 20:
                myHudManager.WinLevel();
                break;
            default:
                myHudManager.ModifyHud();
                break;
        }
    }


    public void StopWalk(float direction)  // ABSTRACTION
    {
       animator.SetBool("isrunning", false);
       animator.SetBool("isidle", true);
       transform.rotation = Quaternion.Euler(0, direction, 0);
    }


    public void Jump()  // ABSTRACTION
    {
        if (hasLanded == true)
        {
            hasLanded = false;
            animator.SetBool("isjumping", true);
            m_Rigidbody.AddForce(transform.up * jumpForce);
        }
    }

    public void animatePlayerAtack()
    {
        StartCoroutine(Atacar());
    }
    private IEnumerator Atacar()  // ABSTRACTION
    {
        gameObject.transform.tag = "espadaTag";
        dangerSignal.SetActive(true);
        animator.SetBool("isatacking", true);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("isatacking", false);
        gameObject.transform.tag = "player";
        dangerSignal.SetActive(false);
    }
}
