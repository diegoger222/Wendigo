using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneador_Items : MonoBehaviour
{
    [Header("Objetos a spawnear")]
    public GameObject cartuchos;
    public GameObject palos;
    public GameObject piedra;
    public GameObject carne_cruda;
    public GameObject carne_cocinada;
    public GameObject pelaje;
    public GameObject cerilla;

    [Header("Fuerza del spawn")]
    public float spawnforce;

    //Spawnear items especificos al dropearlos del inventario o cuando un animal muere. Le pasas el objeto Item y la posicion en la que spawnear
    public void spawnObject(Item item, Transform spawnPoint) {
        GameObject newItem;

        if (item.name == "Cartuchos de escopeta")
        {
            newItem = Instantiate(cartuchos, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Palo")
        {
            newItem = Instantiate(palos, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Piedra")
        {
            newItem = Instantiate(piedra, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Carne cruda")
        {
            newItem = Instantiate(carne_cruda, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Carne cocinada")
        {
            newItem = Instantiate(carne_cocinada, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Pelaje")
        {
            newItem = Instantiate(pelaje, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
        else if (item.name == "Cerilla")
        {
            newItem = Instantiate(cerilla, spawnPoint.position, spawnPoint.rotation);
            newItem.GetComponent<Item>().cantidad = item.cantidad;
            newItem.GetComponent<Rigidbody>().AddForce(spawnPoint.right * spawnforce, ForceMode.Impulse);
            Destroy(newItem, 200);
        }
    }
}
