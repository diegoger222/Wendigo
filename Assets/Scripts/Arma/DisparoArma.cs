using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoArma : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform spawnPoint;
    public GameObject pistola;

    public float shotForce = 1500;     //Relacionado con el disparo
    public float shotRate = 0.6f;
    public float recoilForce = 4f;

    private float shotRateTime = 0;

    public float rechargeRate = 1.3f;       //Relacionado con recargar
    public float rechargeRateTime = 0;
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
            if (shotCounter < numberOfShots && Time.time > shotRateTime && pistola.activeSelf && !recharging)
            {
                //Vector3 a = transform.localPosition;
                AddRecoil();
                GameObject newBullet;

                for (int i = -3; i < 4; i++)    //Se disparan 7 balas, con diferentes desviaciones
                {
                    Vector3 desviacion = new Vector3(i * 0.1f, i%2 * 0.1f, 0);
                    newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                    newBullet.GetComponent<Rigidbody>().AddForce((spawnPoint.forward + desviacion) * shotForce);
                    Destroy(newBullet, 2);
                } 
                shotRateTime = Time.time + shotRate;
                shotCounter++;
            }

            if (shotCounter == numberOfShots)       //Si he gastado los disparos, recargo automaticamente
            {
                Recharge();
            }
        }

        
     
    }

    public void Recharge() {
        if (!recharging) { rechargeRateTime = Time.time + rechargeRate; recharging = true; }
        if (Time.time > rechargeRateTime) { shotCounter = 0; recharging = false; }
    }
    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.right * (recoilForce / 30f);
    }
   
}
