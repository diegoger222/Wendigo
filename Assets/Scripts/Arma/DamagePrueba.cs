using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePrueba : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 30;
 
    
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {/*
        if(other.tag == "Player")
        {
            other.GetComponent<BarraDeVida>().RestarVida(damage);
        }
        */
        if(other.tag == "Animal")
        {
            other.GetComponent<VidaAnimal>().RestarVida(damage);
        }
        /*
        if(other.tag == "Enemigo")
        {
            Debug.Log("Esto es un popu");
        }
        */
    }
}
