using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarraDeVida : MonoBehaviour
{
    // Start is called before the first frame update
    public Image barraVida;

    public float vidaActual = 80;

    public float vidaMaxima = 100;
    public bool invencible = false;
    private float damage;
    // Update is called once per frame
    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public void RestarVida(int cantidad)
    {
        //damage = cantidad;
        if (!invencible && vidaActual > 0)
        {

            vidaActual -= cantidad;
            StartCoroutine(FrenarNasus());

            if (vidaActual < 0)
            {

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
