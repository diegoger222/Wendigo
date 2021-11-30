using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class VidaAnimal : MonoBehaviour
{
    // Start is called before the first frame update

    public float vidaActual = 100;

    public float vidaMaxima = 100;
    public Image barraVida;

    private  Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    // Update is called once per frame
    public void RestarVida(int cantidad)
    {

        vidaActual -= cantidad;
        barraVida.fillAmount = vidaActual / vidaMaxima;
        if (vidaActual < 0)
        {
            anim.SetBool("Muerte", true);
         }
        if (vidaActual > 100)
        {
            vidaActual = 100;
        }
    }
  
}
