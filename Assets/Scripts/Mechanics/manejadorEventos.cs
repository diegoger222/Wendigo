using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manejadorEventos : MonoBehaviour
{

    [SerializeField]
    private GameObject nota1;
    [SerializeField]
    private GameObject nota2;
    [SerializeField]
    private Text mision;
    [SerializeField]
    private GameObject indicador1;
    [SerializeField]
    private GameObject indicador2;
    [SerializeField]
    private Text alerta;

    void Start()
    {
        nota1.SetActive(false);
        nota2.SetActive(false);
        indicador1.SetActive(false);
        indicador2.SetActive(false);
    }

    public void activarEvento1() {
        indicador1.SetActive(true);
        nota1.SetActive(false);
        gameObject.GetComponent<PlayerController>().AdaptLook();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mision.text = "Viaja al pueblo y reunete con el viejo Clark";
        alerta.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "evento1" | other.gameObject.name == "evento2") {
            alerta.text = "pulsa F para inspeccionar nota";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "evento1" && Input.GetKeyDown(KeyCode.F))
        {
            nota1.SetActive(true);
            gameObject.GetComponent<PlayerController>().AdaptLook();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (other.gameObject.name == "evento2" && Input.GetKeyDown(KeyCode.F))
        {
            nota2.SetActive(true);
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
        indicador1.SetActive(false);
        indicador2.SetActive(true);
        nota2.SetActive(false);
        gameObject.GetComponent<PlayerController>().AdaptLook();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mision.text = "Visita la cabaña del cazador Smith";
        alerta.text = "";
    }

    public void activarEvento3() {
        indicador2.SetActive(false);
        mision.text = "Sobrevive y regresa a tu cabaña!";
    }

    //metodo para activar todo lo relacionado con la noche y desactivar cosas del día
    private void canvioHorario() { }


}
