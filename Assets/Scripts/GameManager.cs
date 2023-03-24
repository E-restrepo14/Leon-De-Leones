using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public ControladorPersonaje controlador;
    public HudManager myHudManager;
    public bool isRunningRight;
    public bool isRunningLeft;
    public float speed = 5f;
    public float rotationSpeed = 100.0f;
    float H;

    private void Start()
    {
        controlador = player.GetComponent<ControladorPersonaje>();
        myHudManager = GetComponent<HudManager>();
        controlador.isGameOver = false;

    }

    void Update()
    {
        H = Input.GetAxis("Horizontal");
        float translation = H * speed;
        translation *= Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            isRunningRight = true;
            if (isRunningLeft == false)
            {
                controlador.Correr(90f, translation);
            }
            else
                controlador.Detenerse(90f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            isRunningLeft = true;
            if (isRunningRight == false)
            {
                controlador.Correr(-90f, translation);
            }
            else
                controlador.Detenerse(-90f);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            isRunningRight = false;
            controlador.Detenerse(90f);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            isRunningLeft = false;
            controlador.Detenerse(-90f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controlador.Jump();
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            controlador.StartCoroutine(controlador.Lanzar());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            controlador.StartCoroutine(controlador.Rodar());
        }
      

        if (Input.GetKeyDown(KeyCode.W))
        {
            controlador.StartCoroutine(controlador.Atacar());
        }

        myHudManager.ModifyHud();
        if(controlador.playerLife <=0)
        {
            if(controlador.isGameOver == false)
            {
                myHudManager.StartCoroutine(myHudManager.GameOver());
            }
            controlador.isGameOver = true;
        }

        if (controlador.playerLife > 15)
        {
            if (controlador.isGameOver == false)
            {
                myHudManager.WinLevel();
            }
            controlador.isGameOver = true;
        }
    }
}
