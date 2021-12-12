using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Item item;
    private bool empty;
    private Transform SlotIcon;
    private Transform quantityText;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Sprite spriteVacio;
    
    // Start is called before the first frame update
    void Start()
    {
        SlotIcon = transform.GetChild(0);
        quantityText = transform.GetChild(1);
        empty = true;
        UpdateSlot();
    }

    public void UpdateSlot() {
        if (!empty)
        {
            SlotIcon.GetComponent<Image>().sprite = item.icon;
            quantityText.GetComponent<Text>().text = item.cantidad.ToString();
        }
        else {
            SlotIcon.GetComponent<Image>().sprite = spriteVacio;
            quantityText.GetComponent<Text>().text = "";
        }
    }

    public void setEmpty(bool empty) {
        this.empty = empty;
    }

    public bool getEmpty() {
        return empty;
    }

    public void setItem(Item item) {
        this.item = item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!empty)
        {
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (item.tipo == Item.tipos.consumible)
                {
                    player.GetComponent<BarraDeVida>().RestarVida(-item.propiedad);
                    player.GetComponent<Inventario>().consumirItem(item, 1);
                }
                else if (item.tipo == Item.tipos.municion) {
                    //Funcion para equipar balas
                    player.GetComponent<Inventario>().consumirItem(item, item.cantidad);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right) {
                player.GetComponent<Inventario>().removeItem(item);
            }
        }
    }

    
}
