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

    private float shotRateTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > shotRateTime && pistola.activeSelf)
            {
               
                GameObject newBullet;
                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);
                shotRateTime = Time.time + shotRate;

                Destroy(newBullet, 2);
            }
        }

        if (Input.GetButton("1"))
        {
            pistola.SetActive(false);
        }
        if (Input.GetButton("2"))
        {
            pistola.SetActive(true);
        }
    }
}
