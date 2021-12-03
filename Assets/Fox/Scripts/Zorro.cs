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
    public float DistanciaTeReviento = 20;
    private NavMeshAgent agent;
    public bool Bakugou = true;
    int ataque = 1;
    void Start()
    {
        player = GameObject.Find("Player").transform;
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
        DistanciaConJugador = Vector3.Distance(transform.position, player.position);
        if (DistanciaConJugador < DistanciaTeReviento)
        {
            Reventar();
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
            GetComponent<NavMeshAgent>().destination = player.position;
            if (DistanciaConJugador < 3)
            {

                if( ataque == 0)
                {
                    anim.SetBool("Atacar", true);
                    anim.SetInteger("Ataque", 0);
                   

                }
                else
                {
                    anim.SetBool("Atacar", true);
                    anim.SetInteger("Ataque", 1);
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

}
