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
    private int score;
    public Text scoreText;


    public void Start()
    {
        score = 0;
        controlador = controladorParent.GetComponent<ControladorPersonaje>();
        Pausar();
    }

    public void ModificarScore(int value)
    {
        score = score + value;
        scoreText.text = score.ToString();
    }

    public void ModifyHud() // esta funcion se está llamando cada frame por medio del update de gamemanager
    {
        lifeSlider.value = controlador.playerLife;
    } 

    public void Pausar ()
    {
        Time.timeScale = 0f;
    }

    // debe ser publico porque otras clases acceden aca
    // tal vez en un futuro haga una animacion de que se muera y a los segundos que salga la pantalla gameover
    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0);
        Pausar();
        gameOverScreen.SetActive(true);

    }

    public void WinLevel()
    {
        Pausar();
        winScreen.SetActive(true);

    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Despausar ()
    {
        Time.timeScale = 1.0f;
    }


 // aver aver aver vamos a ver que mierda es este script y que hace?
 // R/ este script se encargará de revisar la variable vida en el gamemanager, y la reflejará en el valor que tenga el slider

 // tambien se encargará de pausar o despausar el juego, lo hará por medio de un booleano que diga esstá pausado o no está pausado
 // luego pensaré que mierda hace mientras está pausado y que hace cuando no lo está.
}
