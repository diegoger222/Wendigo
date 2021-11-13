using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcularMostrarArma : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject escopeta;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Ocultar/MostrarArma"))
        {
            if (escopeta.activeSelf)
            {
                escopeta.SetActive(false);
            }
            else
            {
                escopeta.SetActive(true);
            }

        }
    }
}
