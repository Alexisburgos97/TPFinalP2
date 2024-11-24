using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    public InventoryObject inventory;

    public void UseItem()
    {
        //IMPLEMENTAR USO DE ITEMS - LO DE LAS POCIONES QUE ESTÁ ABAJO ES CUALQUIER COSA¿
    }

    public bool UsePotion()
    {
        if (inventory.HasItemOfType(ItemType.Potion))
        {
            inventory.UsePotion();
            return true;
        }
        else
        {
            return false;
        }
    }

    //USO Y MANEJO DE LAS LLAVES
    public bool HasKey(string key) { return inventory.HasKey(key); }
    public void UseKey(string key) { inventory.UseKey(key); }

    public void AddItem(ItemObject item, int quantity)
    {
        inventory.AddItem(item, quantity);  
    }

    public void ClearItems()
    {
        inventory.Items.Clear();
    }
    
}
