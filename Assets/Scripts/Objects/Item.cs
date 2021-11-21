using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string nombre;
    public enum tipo { 
        usable,
        consumible,
        arma,
        municion
    }

    public string descripcion;
    public Sprite icon;
    public bool stackeable;
    public bool objetoUnico;
    public int cantidad;
    public int maxCantidad;

    //Daño si es arma, vida si es un objeto que cura salud
    public int propiedad; 

}
