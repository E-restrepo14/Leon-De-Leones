using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    public Slider lifeSlider;
    public GameObject controladorParent;
    public ControladorPersonaje controlador;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    [SerializeField]

    public void Start()
    {
        controlador = controladorParent.GetComponent<ControladorPersonaje>();
        Pausar();
    }

    public void ModifyHud() // esta funcion se está llamando cada frame por medio del update de gamemanager
    {
        lifeSlider.value = controlador.playerLife;
    } 

    public void Pausar () // ABSTRACTION
    {
        Time.timeScale = 0f;
    }

    public IEnumerator GameOver()  // ABSTRACTION
    {
        yield return new WaitForSeconds(0);
        Pausar();
        gameOverScreen.SetActive(true);

    }

    public void WinLevel()  // ABSTRACTION
    {
        Pausar();
        winScreen.SetActive(true);
    }

    public void Salir()  // ABSTRACTION
    {
        Application.Quit();
    }

    public void Despausar ()  // ABSTRACTION
    {
        Time.timeScale = 1.0f;
    }
}
