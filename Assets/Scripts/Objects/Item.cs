using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum tipos
    {
        usable,
        consumible,
        arma,
        municion
    }

    [Header("Datos generales")]
    public int ID;
    public string nombre;
    public tipos tipo;
    [TextArea(5, 10)]
    public string descripcion;
    public Sprite icon;

    [Header("Cantidad")]
    public bool stackeable;
    public bool objetoUnico;
    public int cantidad;
    public int maxCantidad;

    //Daño si es arma, vida si es un objeto que cura salud
    [Header("Funcion del objeto")]
    public int propiedad; 

}
