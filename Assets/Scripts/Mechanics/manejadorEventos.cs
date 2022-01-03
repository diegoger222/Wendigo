using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manejadorEventos : MonoBehaviour
{

    [SerializeField]
    private GameObject nota1;
    [SerializeField]
    private GameObject nota2;
    [SerializeField]
    private Text mision;
    [SerializeField]
    private GameObject evento1;
    [SerializeField]
    private GameObject evento2;
    [SerializeField]
    private GameObject evento3;
    [SerializeField]
    private Text alerta;
    private bool keyPressed;
    private GameObject escopeta;

    void Start()
    {
        nota1.SetActive(false);
        nota2.SetActive(false);
        evento2.SetActive(false);
        evento3.SetActive(false);
        keyPressed = false;
        escopeta = GameObject.FindGameObjectWithTag("Escopeta");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && nota1.activeSelf) {
            activarEvento1();
        }
        if (Input.GetKeyDown("0"))
        {
            SceneManager.LoadScene("Nightt");
        }
    }

    public void activarEvento1() {
        Debug.Log("Xd");
        evento1.SetActive(false);
        evento2.SetActive(true);
        nota1.SetActive(false);
        escopeta.SetActive(true);
        gameObject.GetComponent<PlayerController>().AdaptLook();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mision.text = "Viaja al pueblo y reunete con el viejo Clark";
        alerta.text = "";
        keyPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "evento1" | other.gameObject.name == "evento2") {
            alerta.text = "pulsa F para inspeccionar nota";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "evento1" | other.gameObject.name == "evento2")
        {
            alerta.text = "";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "evento1" && Input.GetKeyDown(KeyCode.F) && !keyPressed)
        {
            keyPressed = true;
            escopeta.SetActive(false);
            gameObject.GetComponent<PlayerController>().AdaptLook();
            Debug.Log("chorizo");
            //Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            alerta.text = "";
            nota1.SetActive(true);
        }
        else if (other.gameObject.name == "evento2" && Input.GetKeyDown(KeyCode.F) && !keyPressed)
        {
            keyPressed = true;
            nota2.SetActive(true);
            escopeta.SetActive(false);
            gameObject.GetComponent<PlayerController>().AdaptLook();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (other.gameObject.name == "evento3") {
            activarEvento3();
        }
    }

    public void activarEvento2()
    {
        keyPressed = false;
        evento2.SetActive(false);
        evento3.SetActive(true);
        nota2.SetActive(false);
        escopeta.SetActive(true);
        gameObject.GetComponent<PlayerController>().AdaptLook();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mision.text = "Visita la cabaña del cazador Smith";
        alerta.text = "";
    }

    public void activarEvento3() {
        evento3.SetActive(false);
        mision.text = "Sobrevive y regresa a tu cabaña!";
        //SleepTimeout(3);
        StartCoroutine(EsperarSegundos());
    }

    //metodo para activar todo lo relacionado con la noche y desactivar cosas del día
    private void canvioHorario() { }



    IEnumerator EsperarSegundos()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Nightt");
    }
}
