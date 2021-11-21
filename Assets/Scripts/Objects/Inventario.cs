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

    private Item addToInventory( Item item ) {
        //Caso objeto único
        if (item.objetoUnico) { 
            objetosUnicos.Add(item); return item;
        }
        //Caso el objeto se puede stakear y ya tienes uno en el inventario que no tiene el maximo de staks
        else if (item.stackeable && objetos.Find(x => x.nombre == item.nombre && x.cantidad < x.maxCantidad)){
            int index = objetos.FindIndex(x => x.nombre == item.nombre && x.cantidad < x.maxCantidad); 
            //Si la suma no se pasa del maximo solo se aumenta el contador
            if (objetos[index].cantidad + item.cantidad <= item.maxCantidad){
                objetos[index].cantidad += item.cantidad;
                item.cantidad = 0;
                return item;
            }
            //Si la suma se pasa se pone a maximo ese stack, y si hay espacio se añade otro stack con el numero restante
            else
            {
                item.cantidad -= (item.maxCantidad - objetos[index].cantidad);
                objetos[index].cantidad = objetos[index].maxCantidad;
                if (objetos.Count < numeroSlots) {
                    objetos.Add(item);
                    item.cantidad = 0;
                }
                return item;
            }
        }
        //Caso el objeto no es stakeable o lo es pero no hay ya stacks disponibles
        else if (objetos.Count < numeroSlots)
        {
            objetos.Add(item);
            item.cantidad = 0;
            return item;
        }
        //Caso inventario lleno
        else { return item; }
    }

    private void removeItem(Item item) {
        objetos.Remove(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
