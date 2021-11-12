using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPpal : MonoBehaviour
{
    public GameObject PpalFirstButton, OpcionesFirstButton, CreditosFirstButton;

    public RawImage ImagenCargando;
    public float progreso;

    AsyncOperation asyncLoad = null;

    Vector3 rotationEuler = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Esto es para que gire la imagen de cargando
        if (asyncLoad != null) {
            
            rotationEuler += Vector3.forward * 15 * Time.deltaTime;     //Incrementa 15 grados cada vez
            transform.rotation = Quaternion.Euler(rotationEuler);
            ImagenCargando.rectTransform.Rotate(transform.rotation.eulerAngles);
        }
    }

    public void EmpezarJuego()
    {
        //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        asyncLoad = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);

    }

    public void CerrarJuego()
    {
        Application.Quit();
        Debug.Log("Salir");
    }

    //Esto es para poder usar el teclado en el menu
    public void fPpalAOpciones() { EventSystem.current.SetSelectedGameObject(OpcionesFirstButton); }
    public void fOpcionesAPpal() { EventSystem.current.SetSelectedGameObject(PpalFirstButton); }
    public void fPpalACreditos() { EventSystem.current.SetSelectedGameObject(CreditosFirstButton); }
    public void fCreditosAPpal() { EventSystem.current.SetSelectedGameObject(PpalFirstButton); }

}
