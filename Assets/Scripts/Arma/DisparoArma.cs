using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoArma : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform spawnPoint;
    public GameObject pistola;

    [Header("Disparo")]
    public float shotForce = 1500;     //Relacionado con el disparo
    public float recoilForce = 4f;
    public float shotRate = 0.6f;
    private float shotRateTime = 0;

    [Header("Recarga")]
    public float rechargeRate = 1.3f;       //Relacionado con recargar
    private float rechargeRateTime = 0;
    public int shotCounter = 0;
    public int numberOfShots = 2;
    public bool recharging = false;


    // Update is called once per frame
   
    private void Update()
    {
        Disparo();

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }
    private void Disparo()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!recharging && Time.time > shotRateTime && pistola.activeSelf)
            {
                //Vector3 a = transform.localPosition;
                AddRecoil();
                GameObject newBullet;

                int dx, dy;
                for (int i = 0; i < 8; i++)    //Se disparan 8 balas, con diferentes desviaciones predefinidas
                {
                    Desviaciones(i, out dx, out dy);
                    Vector3 desviacion = new Vector3( dx * 0.035f, dy * 0.025f, 0);
                    newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                    newBullet.GetComponent<Rigidbody>().AddForce((spawnPoint.forward + desviacion) * shotForce);
                    Destroy(newBullet, 1);
                } 
                shotRateTime = Time.time + shotRate;
                shotCounter++;

                if (shotCounter == numberOfShots) { Recharge(); } //Si he gastado los disparos, recargo automaticamente
            }
            
        }

        //Tenemos que ir actualizando el estado de la recarga
        if (recharging) { UpdateRecharge(); }

    }



    //! Metodos Auxiliares
    public void Recharge() //Si ya estas recargando o tienes a tope las balas, no empiezas a recargar.
    { 
        if (!recharging && shotCounter != 0) { rechargeRateTime = Time.time + rechargeRate; recharging = true; }  
    }

    private void UpdateRecharge() //Cuando pase el tiempo establecido, se completara la recarga.
    { 
        if (Time.time > rechargeRateTime) { shotCounter = 0; recharging = false; } 
    }

    private void Desviaciones(int i, out int dx, out int dy)
    {
        switch (i)
        {
            case 0: dx = -2; dy =  1; break;
            case 1: dx = -1; dy =  2; break;
            case 2: dx = -2; dy = -1; break;
            case 3: dx = -1; dy = -2; break;
            case 4: dx =  2; dy =  1; break;
            case 5: dx =  1; dy =  2; break;
            case 6: dx =  2; dy = -1; break;
            case 7: dx =  1; dy = -2; break;
            default: dx = 0; dy =  0; break;
        }
    }


    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.right * (recoilForce / 30f);
    }
   
}
