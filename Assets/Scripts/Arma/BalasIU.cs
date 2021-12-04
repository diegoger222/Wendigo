using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalasIU : MonoBehaviour
{


    private GameObject escopeta;
    // Start is called before the first frame update

    public Text ammo;
    public Text balas;
    void Start()
    {

        escopeta = GameObject.Find("Double-barrel_gun");
        ammo.text = escopeta.GetComponent<DisparoArma>().GetAmo().ToString();
        balas.text = escopeta.GetComponent<DisparoArma>().GetBalas().ToString();
    
    }

    // Update is called once per frame
    void Update()
    {
        
        ammo.text = escopeta.GetComponent<DisparoArma>().GetAmo().ToString();
        balas.text = escopeta.GetComponent<DisparoArma>().GetBalas().ToString();
    }
}
