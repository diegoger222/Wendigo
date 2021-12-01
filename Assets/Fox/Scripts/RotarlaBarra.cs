using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarlaBarra : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cam;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
    }
}
