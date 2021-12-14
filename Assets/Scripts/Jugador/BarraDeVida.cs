using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BarraDeVida : MonoBehaviour
{
    // Start is called before the first frame update
    public Image barraVida;

    public float vidaActual = 80;
    public GameObject pantallaMuerte;
    public float vidaMaxima = 100;
    public bool invencible = false;
    private float damage;
    // Update is called once per frame
    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;

        if(Input.GetKeyDown("return")&& pantallaMuerte.activeSelf)
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync("MenuV2");
            
        }
    }

    private void Start()
    {
        pantallaMuerte.SetActive(false);
    }

    public void RestarVida(int cantidad)
    {
        //damage = cantidad;
        if (!invencible && vidaActual > 0)
        {

            vidaActual -= cantidad;
           // StartCoroutine(FrenarNasus());

            if (vidaActual  <= 0)
            {
                Time.timeScale = 0;
                pantallaMuerte.SetActive(true);
                GameObject.Find("HUD").SetActive(false);
                GameObject.Find("ArmaPosicion").SetActive(false);
                Debug.Log("Has muerto");
           

            }
            if (vidaActual > 100)
            {
                vidaActual = 100;
            }
        }
    }


    IEnumerator FrenarNasus() {
        invencible = true;
        yield return new WaitForSeconds(0.75f);   
        invencible = false;
    }
}
