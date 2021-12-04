using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zorro : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    private Animator anim;
    public Transform player;
    float DistanciaConJugador;
    float DistanciaPunto;
    public float DistanciaTeReviento = 20;
    private NavMeshAgent agent;
    public bool Bakugou = true;
    public int tipoZorro = 0;
    private bool nerfthis = false;

    Transform puntofinal;
    bool muerto = false;
    int ataque = 1;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        target = GameObject.Find("1").transform;
        puntofinal = GameObject.Find("2").transform;
        anim = GetComponent<Animator>();
        //anim.SetBool("Sentarse", true);
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
       // anim.SetBool("Sentarse", true);
    }
    // Update is called once per frame
    void Update()
    {

        muerto = anim.GetBool("Muerte");
        DistanciaConJugador = Vector3.Distance(transform.position, player.position);
        if ((DistanciaConJugador < DistanciaTeReviento)&&!muerto&& tipoZorro==0)
        {
            Reventar();
        }
        if(tipoZorro == 1)
        {     
            caminar();
        }
        if (Input.GetKeyDown("m")) {
            if (anim.GetBool("Sentarse") != true) {
                anim.SetBool("Sentarse", true);
                anim.SetInteger("Index_wolf", 0);

            }
            else
            {
                anim.SetBool("Sentarse", false);
            }
            
        }

        if (Input.GetKeyDown("j"))
        {
            if (anim.GetInteger("Index_wolf") != 1)
            {
                anim.SetInteger("Index_wolf", 1); // estado caminar
            }
            else
            {
                anim.SetInteger("Index_wolf", 3);// estado normal
            }
        }

        if (Input.GetKeyDown("b")) {
            if (anim.GetBool("Correr") != true) {
                anim.SetBool("Correr", true);
              

            }
            else
            {
                anim.SetBool("Correr", false);
            }
            
        }
    }

    void Reventar()
    {

        anim.SetInteger("Index_wolf", 1);
        anim.SetBool("Correr", true);
        GetComponent<NavMeshAgent>().speed = 12;

        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        if (Bakugou)
        {
           // GetComponent<NavMeshAgent>().destination = player.position;
            if (DistanciaConJugador < 3)
            {

                if( ataque == 0)
                {
                    anim.SetBool("Atacar", true);
                    anim.SetInteger("Ataque", 0);
                   GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(10);
                   

                }
                else
                {
                    anim.SetBool("Atacar", true);
                    anim.SetInteger("Ataque", 1);
                   
                    if (!nerfthis)
                    {
                        StartCoroutine(NerfeaEsto());
                       
                    }
                }

                GetComponent<NavMeshAgent>().speed = 0;


            }

            if (DistanciaConJugador > 3)
            {
                anim.SetBool("Atacar", false);
                GetComponent<NavMeshAgent>().speed = 12;

            }
        }
    }

    void caminar()
    {


        anim.SetInteger("Index_wolf", 1);
        anim.SetBool("Correr", true); // quitar
        this.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        DistanciaPunto = Vector3.Distance(transform.position, target.position);
        if (DistanciaPunto < 3)
        {

           
            target = GameObject.Find("2").transform;
            if(target = puntofinal)
            {
                anim.SetBool("Correr", false); // quitar
                anim.SetInteger("Index_wolf", 0);
                anim.SetBool("Sentarse", true);
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {



        if(other.tag == "Player")
        {

            if (DistanciaConJugador < 3)
            {
                other.GetComponent<BarraDeVida>().RestarVida(10);
            }
        }
       
    }
    */
     IEnumerator NerfeaEsto()
    {
        nerfthis = true;
        yield return new WaitForSeconds(0.5f);
        if (DistanciaConJugador < 3)
        {
            GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(10);
        }
        yield return new WaitForSeconds(0.6f);
        nerfthis = false;
    }
}
