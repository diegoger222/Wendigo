using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public GameObject inventario;
    public GameObject PanelSlots;

    private int numeroSlots;
    private List<GameObject> slots;
    private List<Item> objetos;
    private List<Item> objetosUnicos;


    void Start()
    {
        numeroSlots = PanelSlots.transform.childCount;
        slots = new List<GameObject>();
        objetos = new List<Item>();
        objetosUnicos = new List<Item>();
        objetosUnicos.Add(GameObject.FindGameObjectWithTag("Escopeta").GetComponent<Item>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool addToInventory(Item item) {
        // Caso el objeto es stakeable y ya tienes más de esos objetos en el imventario
        if (item.objetoUnico) { 
            objetosUnicos.Add(item); return true;
        }
        else if (item.stackeable && objetos.Contains(item))
        {
            if (objetos.Find(x => x.nombre == item.nombre).cantidad + item.cantidad <
                item.maxCantidad)
            {
                objetos.Find(x => x.nombre == item.nombre).cantidad += item.cantidad;
                return true;
            }
            else if (objetos.Find(x => x.nombre == item.nombre).cantidad + item.cantidad >=
                item.maxCantidad && objetos.Count < numeroSlots)
            {
                item.cantidad -= (item.maxCantidad - objetos.Find(x => x.nombre == item.nombre).cantidad);
                objetos.Find(x => x.nombre == item.nombre).cantidad = item.maxCantidad;
                if (item.cantidad != 0) { objetos.Add(item); }
                return true;
            }
            else { return false; }
        }
        else if (objetos.Count < numeroSlots)
        {
            objetos.Add(item);
            return true;
        }

        else { return false; }
    }

    private void removeItem(Item item) {
        objetos.Remove(item);
    }

}
