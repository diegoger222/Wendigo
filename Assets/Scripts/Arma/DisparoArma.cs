using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoArma : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform spawnPoint;
    public GameObject pistola;

    public float shotForce = 1500;
    public float shotRate = 0.5f;
    public float recoilForce = 4f;

    private float shotRateTime = 0;

    // Update is called once per frame
   
    private void Update()
    {
        Disparo();

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }
    private void Disparo()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > shotRateTime && pistola.activeSelf)
            {
                //Vector3 a = transform.localPosition;
                AddRecoil();
                GameObject newBullet;
                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);
                shotRateTime = Time.time + shotRate;

                
                Destroy(newBullet, 2);

                
            }
        }

        
     
    }
    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.right * (recoilForce / 30f);
    }
   
}
