using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    GameObject button;
    SoundController soundController;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        soundController = GameObject.Find("Canvas").GetComponent<SoundController>();
        button = gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == button)
        {
            
            if (animator.GetBool("Selected") == false)
            {
                if(soundController.GetDisableOnce() == false) { soundController.PlaySound1();}
                else { soundController.InvertDisableOnce(); }

                animator.SetBool("Selected", true);
            }
            
            if (Input.GetAxis("Submit") == 1 || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (animator.GetBool("Pressed") == false)
                {
                    //soundController.PlaySound2();
                    animator.SetBool("Pressed", true);
                }
            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
            }            
        }
        else { animator.SetBool("Selected", false); }
    }

    void CallPlaySound1()
    {
        soundController.PlaySound1();
    }

    void CallPlaySound2()
    {
        soundController.PlaySound2();
    }
}
