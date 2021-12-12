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
    public bool BufeoNasus = false;
    public int damage = 20;
    private bool atacando = false;

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
        if(tipoZorro == 1)
        {
            GetComponent<NavMeshAgent>().destination = target.position;
            GetComponent<NavMeshAgent>().updateRotation = true;
        }
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
        if(tipoZorro == 2)
        {
            anim.SetBool("Sentarse", true);
            anim.SetInteger("Index_wolf", 0);
            anim.SetInteger("MoverCabezaSentado", 3);

        }
        if (tipoZorro == 3)
        {
            anim.SetBool("Tactico", true);
        }
        /*
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
        */
    }

    void Reventar()
    {
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        anim.SetInteger("Index_wolf", 1);
        anim.SetBool("Correr", true);
       
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        if (Bakugou)
        {
           // GetComponent<NavMeshAgent>().destination = player.position;
            if (DistanciaConJugador < 2)
            {
                //StartCoroutine(TipoAtaque());

                if( ataque == 0)
                {
                    anim.SetBool("Atacar", true);
                    anim.SetInteger("Ataque", 0);
                    if (!nerfthis)
                    {
                        StartCoroutine(NerfeaEsto());

                    }


                }
                else
                {
                    if (ataque == 1)
                    {
                        anim.SetBool("Atacar", true);
                        anim.SetInteger("Ataque", 1);

                        if (!nerfthis)
                        {
                            StartCoroutine(NerfeaEsto());

                        }
                    }
                    else
                    {
                        anim.SetBool("Atacar", true);
                        anim.SetInteger("Ataque", 2);

                        if (!nerfthis)
                        {
                            StartCoroutine(NerfeaEsto());

                        }
                    }
                }
                atacando = true;
                if (BufeoNasus)
                {
                    GetComponent<NavMeshAgent>().speed = 0;
                    GetComponent<NavMeshAgent>().destination = player.position;
                }

            }

            if (DistanciaConJugador > 2)
            {
                anim.SetBool("Atacar", false);
                atacando = false;
                if (BufeoNasus && !nerfthis)
                {
                    GetComponent<NavMeshAgent>().speed = 5;
                    GetComponent<NavMeshAgent>().destination = player.position;
                }
            }
        }
    }

    void caminar()
    {

        
        
        anim.SetInteger("Index_wolf", 1);
        anim.SetBool("CorrerS", true); // quitar  this.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
       // this.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        DistanciaPunto = Vector3.Distance(transform.position, target.position);
       transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        if (DistanciaPunto < 3)
        {

            SiguienteLugar();
            /*
             if(target == puntofinal)
             {
                 anim.SetBool("CorrerS", false); // quitar
                 anim.SetInteger("Index_wolf", 0);
                 anim.SetBool("Sentarse", true);
             }
             else
             {

             }
           */
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
        if (DistanciaConJugador < 2)
        {
            GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(damage);
        }
        yield return new WaitForSeconds(0.6f);
        ataque = UnityEngine.Random.Range(0, 3);
        nerfthis = false;
    }



    IEnumerator AtacandoB()
    {
        atacando = true;
        yield return new WaitForSeconds(1.1f);
        atacando = false;
        
    }


    void SiguienteLugar()
    {
        int i = UnityEngine.Random.Range(0, 6);
        switch (i)
        {
            case 0: target = GameObject.Find("1").transform; break;
            case 1: target = GameObject.Find("2").transform; break;
            case 2: target = GameObject.Find("3").transform; break;
            case 3: target = GameObject.Find("4").transform; break;
            case 4: target = GameObject.Find("5").transform; break;
            case 5: target = GameObject.Find("6").transform; break;
            case 6: target = GameObject.Find("1").transform; break;
            case 7: target = GameObject.Find("1").transform; break;
            default: target = GameObject.Find("3").transform; break;
        }
    }
}
