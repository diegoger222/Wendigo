using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wendi : MonoBehaviour
{

    private Animator anim;
    public Transform player;
    float DistanciaConJugador;
    float DistanciaPunto;
    public float DistanciaTeReviento = 20;
    bool nerfVoli = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DistanciaConJugador = Vector3.Distance(transform.position, player.position);
        if ((DistanciaConJugador < DistanciaTeReviento))
        {
            Reventar();
        }
    }


    void Reventar()
    {

        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        if (DistanciaConJugador >= 9)
        {
            GetComponent<NavMeshAgent>().speed = 12;

       

            GetComponent<NavMeshAgent>().destination = player.position;

            anim.SetBool("Correr", true);
        }
        if(DistanciaConJugador< 9)
        {
        
            GetComponent<NavMeshAgent>().speed = 0;
            anim.SetBool("Atacar", true);
        }
        else
        {

            anim.SetBool("Atacar", false);
        }
    }




    private void OnTriggerEnter(Collider other)
    {/*
        if(other.tag == "Player")
        {
            other.GetComponent<BarraDeVida>().RestarVida(damage);
        }
        */
        if (other.tag == "Animal")
        {
            other.GetComponent<VidaAnimal>().RestarVida(70);
        }
        if(other.tag == "Player")
        {

            if (!nerfVoli)
            {
                StartCoroutine(DañoVoli());

            }
        }
        /*
        if(other.tag == "Enemigo")
        {
            Debug.Log("Esto es un popu");
        }
        */

    }

    IEnumerator DañoVoli()
    {
        nerfVoli= true;
        yield return new WaitForSeconds(0.5f);
        if (DistanciaConJugador < 9)
        {
            GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(70);
        }
        yield return new WaitForSeconds(0.6f);
        nerfVoli = false;
    }

}

