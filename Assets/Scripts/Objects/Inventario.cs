using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    [Header("Referencias a inventario")]
    public GameObject inventario;
    public GameObject PanelSlots;

    [Header("Referencia a la alerta")]
    public Text Alerta;

    [Header("Cantidad the objetos dropeados")]
    public int cantidadDrop;

    private int numeroSlots;
    private List<GameObject> slots;
    private List<Item> objetos;
    private List<Item> objetosUnicos;

    //variable para sacar y ocultar el inventario
    private bool inventarioVisible;


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
        if (Input.GetKeyDown(KeyCode.E)) { inventarioVisible = !inventarioVisible; }
        if (inventarioVisible)
        {
            inventario.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else {
            inventario.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
        //Caso el objeto no es stakeable o lo es pero no hay stacks disponibles
        else if (objetos.Count < numeroSlots){
            objetos.Add(item);
            item.cantidad = 0;
            return item;
        }
        //Caso inventario lleno
        else { return item; }
    }

    public void removeItem(Item item) {
        if (item.stackeable) {
            int index = objetos.FindIndex(x => x.nombre == item.nombre && x.cantidad == item.cantidad);
            if (objetos[index].cantidad - cantidadDrop <= item.maxCantidad) { objetos.Remove(objetos[index]); }
            else { objetos[index].cantidad -= cantidadDrop; }
        }
        else { objetos.Remove(item); }
        refrescarUi();
    }

    public void consumirItem(Item item, int cantidad) {
        if (item.stackeable) {
            int index = objetos.FindIndex(x => x.nombre == item.nombre && x.cantidad == item.cantidad);
            if (objetos[index].cantidad - cantidad <= item.maxCantidad) { objetos.Remove(objetos[index]); }
            else { objetos[index].cantidad -= cantidadDrop; }
        }
        else { objetos.Remove(item); }
        refrescarUi();
    }

    public int getMunicion(string tipo) {
        if (objetos.Find(x => x.nombre == tipo && x.tipo == Item.tipos.municion))
        {
            return objetos.Find(x => x.nombre == tipo && x.tipo == Item.tipos.municion).cantidad;
        }
        else { return -1; }
    }

    // Metodo en proceso. Faltan un script pa spawnear objetos y averiguar un poco como manejar el tema de la alerta
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            //Alerta.text = "Pulsa F para recoger " + item.nombre + "x " + item.cantidad;
            if (Input.GetKeyDown(KeyCode.F)) {
                Item itemAux = addToInventory(item);
                if (itemAux.cantidad == item.cantidad) { Alerta.text = "Inventario lleno"; }
                else { Destroy(other.gameObject); refrescarUi(); }
            }
        }
    }


    private void refrescarUi() { }

}
