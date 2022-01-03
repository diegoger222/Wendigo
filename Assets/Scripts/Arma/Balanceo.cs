using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balanceo : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion startRotation;

    //Inicio balanceo escopeta al andar
    private float valor = 0f;
    private float limite = -5f;
    private float incremento = 0.2f;
    //Fin balanceo escopeta al andar

    public float swayAmount = 8;
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Sway();
    }

    private void Sway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //Inicio balanceo escopeta al andar
        float ejex = Input.GetAxis("Horizontal");
        float ejey = Input.GetAxis("Vertical");

        if (ejex == 0 && ejey == 0) { valor = limite; }
        else if (valor < limite || valor > 0) { incremento = -incremento; valor += incremento; }
        else { valor += incremento; }

        Quaternion xmAngle = Quaternion.AngleAxis(ejex * valor, Vector3.up);        

        Quaternion ymAngle = Quaternion.AngleAxis(ejey * valor, Vector3.back);
        //Fin balanceo escopeta al andar

        Quaternion xAngle = Quaternion.AngleAxis(mouseX * -3f, Vector3.up);

        Quaternion yAngle = Quaternion.AngleAxis(mouseY * 5f, Vector3.back);

        Quaternion targetRotation = startRotation * xAngle * yAngle * xmAngle * ymAngle;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swayAmount);
    }
}
