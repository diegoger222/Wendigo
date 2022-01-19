using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneador_Items : MonoBehaviour
{
    [Header("Objetos a spawnear")]
    public GameObject cartuchos;
    public GameObject monton_cartuchos;
    public GameObject palos;
    public GameObject piedra;
    public GameObject carne_cruda;
    public GameObject carne_cocinada;
    public GameObject pelaje;
    public GameObject cerilla;

    [Header("Fuerza del spawn")]
    public float spawnforce;

    //Spawnear items especificos al dropearlos del inventario o cuando un animal muere. Le pasas el objeto Item y la posicion en la que spawnear
    public void spawnObject(Item item, int cantidad, Transform spawnPoint) {
        GameObject newItem;

        if (item.nombre == "Cartucho")
        {
            if (cantidad < 5)
            {
                newItem = Instantiate(cartuchos, spawnPoint.position, spawnPoint.rotation);
                newItem.GetComponent<Item>().cantidad = cantidad;
                newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
                Destroy(newItem, 200);
            }
            else {
                newItem = Instantiate(monton_cartuchos, spawnPoint.position, spawnPoint.rotation);
                newItem.GetComponent<Item>().cantidad = cantidad;
                newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
                Destroy(newItem, 200);
            }
        }
        else if (item.nombre == "Palo")
        {
            newItem = Instantiate(palos, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.nombre == "Piedra")
        {
            newItem = Instantiate(piedra, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.nombre == "Carne")
        {
            newItem = Instantiate(carne_cruda, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
           // newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
           // Destroy(newItem, 200);
        }
        else if (item.nombre == "Carne cocinada")
        {
            newItem = Instantiate(carne_cocinada, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.nombre == "Pelaje")
        {
            newItem = Instantiate(pelaje, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.nombre == "Cerilla")
        {
            newItem = Instantiate(cerilla, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
    }
}
