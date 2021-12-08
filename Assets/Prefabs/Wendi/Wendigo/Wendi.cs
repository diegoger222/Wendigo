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
    public float DistanciaTeReviento = 40;
    bool nerfVoli = false;
    bool atacando = false;
    bool grito = false;
    // Start is called before the first frame update
    void Start()
    {
      //  player = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        DistanciaTeReviento = 40;
         DistanciaConJugador = Vector3.Distance(transform.position, player.position);
    }

    // Update is called once per frame
    void Update()
    {
        DistanciaConJugador = Vector3.Distance(transform.position, player.position);
        if ((DistanciaConJugador < DistanciaTeReviento) && !grito)
        {

            Reventar();
        }
        else
        {
            anim.SetBool("Andar", false);
            anim.SetBool("Correr", false);
            GetComponent<NavMeshAgent>().speed = 0;

        }
        if ((DistanciaConJugador < DistanciaTeReviento)){
            this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        }
        if (Input.GetKeyDown("h"))
        {
            if (!grito)
            {
                this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                GetComponent<NavMeshAgent>().speed = 0;
                StartCoroutine(GritoVoli());
            }
        }
    }


    void Reventar()
    {

        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        if (DistanciaConJugador >= 9 && !atacando)
        {
            anim.SetBool("Andar", true);
            GetComponent<NavMeshAgent>().speed = 12;

       

            GetComponent<NavMeshAgent>().destination = player.position;

            anim.SetBool("Andar", true);
            anim.SetBool("Correr", true);
        }
        if(DistanciaConJugador< 9)
        {
        
            GetComponent<NavMeshAgent>().speed = 0;
            anim.SetBool("Atacar", true);
            

                if (!nerfVoli)
                {
                    StartCoroutine(DañoVoli());

                }
            

        }
        else
        {

            anim.SetBool("Atacar", false);
        }
    }




  
   
       
    
    IEnumerator DañoVoli()
    {
        nerfVoli= true;
        atacando = true;
        yield return new WaitForSeconds(1.25f);
        if (DistanciaConJugador < 10)
        {
            GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(70);
        }
        yield return new WaitForSeconds(1.75f);
        atacando = false;
        nerfVoli = false;
    }

    IEnumerator GritoVoli()
    {
        grito = true;
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        anim.SetBool("Grito", true);
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        yield return new WaitForSeconds(5);
        anim.SetBool("Grito", false);
        grito = false;

    }

}

