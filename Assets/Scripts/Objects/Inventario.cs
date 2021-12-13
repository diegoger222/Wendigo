using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    [Header("Referencias a inventario")]
    public GameObject inventario;
    public GameObject SlotsHolder;

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
    private bool teclaPulsada;


    void Start()
    {
        numeroSlots = SlotsHolder.transform.childCount;
        slots = new List<GameObject>();
        objetos = new List<Item>();
        objetosUnicos = new List<Item>();
        objetosUnicos.Add(GameObject.FindGameObjectWithTag("Escopeta").GetComponent<Item>());
        for (int i = 0; i < numeroSlots; i++) {
            slots.Add(SlotsHolder.transform.GetChild(i).gameObject);
        }
        inventarioVisible = false;
        teclaPulsada = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { inventarioVisible = !inventarioVisible; FijarVista(); }
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

    private int addToInventory( Item item ) { 
        if (item.objetoUnico) { 
            objetosUnicos.Add(item); return 0;
        }
        //Caso el objeto se puede stakear y ya tienes uno en el inventario que no tiene el maximo de staks
        else if (item.stackeable && objetos.Find(x => x.nombre == item.nombre && x.cantidad < x.maxCantidad)){
            Debug.Log("Stakeable y disponible en inventario");
            int index = objetos.FindIndex(x => x.nombre == item.nombre && x.cantidad < x.maxCantidad); 
            //Si la suma no se pasa del maximo solo se aumenta el contador
            if (objetos[index].cantidad + item.cantidad <= item.maxCantidad){
                Debug.Log("la suma no se pasa del maximo solo se aumenta el contador");
                objetos[index].cantidad += item.cantidad;
                return 0;
            }
            //Si la suma se pasa se pone a maximo ese stack, y si hay espacio se añade otro stack con el numero restante
            else
            {
                Debug.Log("a suma se pasa se pone a maximo ese stack, y si hay espacio se añade otro stack con el numero restante");
                item.cantidad -= (item.maxCantidad - objetos[index].cantidad);
                objetos[index].cantidad = objetos[index].maxCantidad;
                if (objetos.Count < numeroSlots) {
                    objetos.Add(item);
                    return 0;
                }
                return item.cantidad;
            }
        }
        //Caso el objeto no es stakeable o lo es pero no hay stacks disponibles
        else if (objetos.Count < numeroSlots){
            Debug.Log("objeto no es stakeable o lo es pero no hay stacks disponibles");
            objetos.Add(item);
            return 0;
        }
        //Caso inventario lleno
        else { return item.cantidad; }
    }

    public void removeItem(Item item) {
        if (item.stackeable) {
            int index = objetos.FindIndex(x => x.nombre == item.nombre && x.cantidad == item.cantidad);
            if (objetos[index].cantidad - cantidadDrop <= item.maxCantidad) {
                gameObject.GetComponent<Spawneador_Items>().spawnObject(item, item.cantidad, gameObject.GetComponent<Transform>());
                objetos.Remove(objetos[index]);
            }
            else {
                objetos[index].cantidad -= cantidadDrop;
                gameObject.GetComponent<Spawneador_Items>().spawnObject(item, cantidadDrop, gameObject.GetComponent<Transform>());
            }
        }
        else {
            gameObject.GetComponent<Spawneador_Items>().spawnObject(item, item.cantidad, gameObject.GetComponent<Transform>());
            objetos.Remove(item);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            Debug.Log(item.nombre);
            Alerta.text = "Pulsa F para recoger " + item.nombre + " x " + item.cantidad;
        }
    }

    // Metodo en proceso. Faltan un script pa spawnear objetos
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if ( Input.GetKeyUp(KeyCode.F) && !teclaPulsada) {
                teclaPulsada = true;
                Item item = other.gameObject.GetComponent<Item>();
                int cantidadRestante = addToInventory(item);
                Debug.Log(cantidadRestante);
                if (cantidadRestante == item.cantidad) { Alerta.text = "Inventario lleno"; }
                else {
                    Destroy(other.gameObject);
                    refrescarUi();
                    Alerta.text = "";
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Alerta.text = "";
            teclaPulsada = false;
        }
    }

    //metodo para refrescar la interfaz del inventario
    private void refrescarUi() {
        Debug.Log("refrescarUi");
        foreach (GameObject slot in slots) {
            slot.GetComponent<Slot>().setEmpty(true);
        }
        for (int i = 0; i < objetos.Count; i++) {
            slots[i].GetComponent<Slot>().setItem(objetos[i]);
            slots[i].GetComponent<Slot>().setEmpty(false);
        }
        foreach (GameObject slot in slots)
        {
            slot.GetComponent<Slot>().UpdateSlot();
        }
    }

    public void FijarVista() { GameObject.Find("Player").GetComponent<PlayerController>().AdaptLook(); }


}
