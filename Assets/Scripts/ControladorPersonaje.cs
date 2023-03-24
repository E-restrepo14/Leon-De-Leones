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
    public GameObject hudManagerParent;

    [SerializeField]
    private GameObject dagaArrojadiza;

    // lo va a cambiar la clase enemigo por lo que debe ser publico
    public bool aunTieneEscudo;
    [SerializeField]
    private bool aunTieneDaga;
    [SerializeField]
    private bool lanzoEscudo;
    [SerializeField]
    private bool lanzoLaDaga;

    public bool isGameOver;
    public GameObject dangerSignal;
    public GameObject dodgeSignal;
    public float playerLife = 10f;
    public Animator animator;
    float smooth = 5.0f;
    Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    public bool canJump;
    Vector3 initialPosition;

    private void Start()
    {
        myHudManager = hudManagerParent.GetComponent<HudManager>();
        aunTieneEscudo = false;
        aunTieneDaga = false;
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

    void CheckPoint (Transform checkpoint)
    {
        initialPosition = checkpoint.position;
    }

    //debe ser publica porque a esto accede el evento de animacion, no cambiarle el nombre
    public void VerificarManoDeEscudo()
    {
        if (aunTieneEscudo == false & escudoDefensor.GetComponent<Renderer>().isVisible)
        {
            CambiarEscudoDeMano();
        }

    }


    IEnumerator AgarrarEscudo()
    {
        aunTieneEscudo = true;
        animator.SetBool("hadshield", true);
        escudoDefensor.GetComponent<Renderer>().enabled = true;
        escudoAtacante.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0);
        animator.SetBool("hadshield", false);
    }

    IEnumerator AgarrarDaga()
    {
        aunTieneDaga = true;
        animator.SetBool("hadshield", true);
        yield return new WaitForSeconds(0);
        animator.SetBool("hadshield", false);
    }

    void CambiarEscudoDeMano()
    {
        escudoDefensor.GetComponent<Renderer>().enabled = false;
        escudoAtacante.GetComponent<Renderer>().enabled = true;
    }

    // ojo con cambiarle el nombre porque ya despues no se llama por eventos de animaciones
    void DestruirEscudos()
    {
        escudoDefensor.GetComponent<Renderer>().enabled = false;

        escudoAtacante.GetComponent<Renderer>().enabled = false;
    }

    // es publico porque se llama sola desde un evento de animacion, no cambiarle el nombre
    public void LanzarODestruirEscudo()
    {
        if (aunTieneDaga == true & lanzoLaDaga == true)
        {
            GameObject newDaga = Instantiate(dagaArrojadiza);
            newDaga.transform.position = escudoAtacante.transform.position;
            newDaga.transform.rotation = transform.rotation;
            newDaga.GetComponent<Rigidbody>().velocity = transform.forward * 10;
            Destroy(newDaga, 2);

            lanzoLaDaga = false;
            aunTieneDaga = false;
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

                DestruirEscudos();
                lanzoEscudo = false;
                aunTieneEscudo = false;
            }
        }

        if (aunTieneEscudo == false)
        {
            DestruirEscudos();
        }
        

    }

    void OnCollisionEnter(Collision collision)
    {
        if (canJump == false)
        {
            animator.Play("Base Layer.idle", 0, 0f);
        }

        if (collision.gameObject.CompareTag("suelo"))
        {
            animator.SetBool("isjumping", false);
            canJump = true;
        }

        if (collision.gameObject.CompareTag("finishTag"))
        {
            playerLife = 20f;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("checkpoint"))
        {
            CheckPoint(other.transform);
        }

        if (other.gameObject.CompareTag("arrowTag") & dodgeSignal.activeSelf == false )
        {
            if(aunTieneEscudo == true)
            {
                aunTieneEscudo = false;
                InstanciarEscudoRoto(transform);
                animator.Play("Base Layer.charging", 0, 0f);
                DestruirEscudos();
            }
            else
            RecibirDaño();
        }

        if (other.gameObject.CompareTag("endTag"))
        {
            playerLife = 0;
        }

        if (other.gameObject.CompareTag("bossTag"))
        {
            if (canJump == true)
            {
                playerLife = 0;
            }
            else
            {
                this.gameObject.transform.GetChild(0).GetComponent<AudioSource>().Play(0);
                other.GetComponent<BossClass>().RecibirDaño();
            }
        }

        if (other.gameObject.CompareTag("shieldTag"))
        {
            if (aunTieneEscudo == false)
            {
                StartCoroutine(AgarrarEscudo());
                if (other.GetComponent<AudioSource>() != null)
                {
                    other.GetComponent<AudioSource>().Play(0);
                }
                Destroy (other.gameObject,0.5F);
            }
        }

        if (other.gameObject.CompareTag("dagaTag"))
        {
            if (aunTieneDaga == false)
            {
                StartCoroutine(AgarrarDaga());
                if (other.GetComponent<AudioSource>() != null)
                {
                    other.GetComponent<AudioSource>().Play(0);
                }
                Destroy(other.gameObject, 0.5F);
            }
        }
    }

    public IEnumerator Rodar()
    {
        dodgeSignal.SetActive(true);
        animator.SetBool("isrolling", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isrolling", false);
        dodgeSignal.SetActive(false);
    }

    //porque tiene que ser corutina?
    public IEnumerator Lanzar()
    {
        if (aunTieneDaga == true)
        {
            lanzoLaDaga = true;
            StartCoroutine(Atacar());
        }
        else
        {
            if (aunTieneEscudo == true)
            {
                StartCoroutine(Atacar());
                lanzoEscudo = true;
            }
        }
        yield return new WaitForSeconds(0);
    }


    public void Correr(float direction,float speed) 
    {
        transform.Translate(speed, 0, 0,Space.World);
        animator.SetBool("isrunning", true);
        animator.SetBool("isidle", false);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, direction, 0), Time.deltaTime* smooth);
    }

    public void RecibirDaño()
    {
        playerLife -= 2f;
        animator.Play("Base Layer.atack", 0, 0f);
        GetComponent<AudioSource>().Play(0);
        myHudManager.ModificarScore(-1);

    }


    public void Detenerse(float direction)
    {
       animator.SetBool("isrunning", false);
       animator.SetBool("isidle", true);
       transform.rotation = Quaternion.Euler(0, direction, 0);
    }


    public void Jump()
    {
        if (canJump == true)
        {
            canJump = false;
            animator.SetBool("isjumping", true);
            m_Rigidbody.AddForce(transform.up * m_Thrust);
        }

    }

    public IEnumerator Atacar()
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
