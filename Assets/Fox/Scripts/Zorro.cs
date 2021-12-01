using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zorro : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator anim;
    void Start()
    {
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

}
