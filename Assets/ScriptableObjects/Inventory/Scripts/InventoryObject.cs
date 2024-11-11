using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Items = new List<InventorySlot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].item == _item)
            {
                Items[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Items.Add(new InventorySlot(_item, _amount));
        }
    }
    
    public bool HasItemOfType(ItemType type)
    {
        return Items.Exists(slot => slot.item.type == type && slot.amount > 0);
    }

    public ItemObject GetItemOfType(ItemType type)
    {
        return Items.Find(slot => slot.item.type == type && slot.amount > 0)?.item;
    }

    public void RemoveItemOfType(ItemType type, int amount)
    {
        var slot = Items.Find(s => s.item.type == type && s.amount > 0);
        if (slot != null)
        {
            slot.amount -= amount;
            if (slot.amount <= 0)
                Items.Remove(slot);
        }
    }
    
    public bool HasKey(string keyName)
    {
        foreach (InventorySlot slot in Items)
        {
            KeyObject keyItem = slot.item as KeyObject;  // Asegurarse de que es una llave
            if (keyItem != null && keyItem.name == keyName && slot.amount > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void UseKey(string keyName)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            KeyObject keyItem = Items[i].item as KeyObject;
            if (keyItem != null && keyItem.name == keyName && Items[i].amount > 0)
            {
                Items[i].amount--;  // Usar la llave
                if (Items[i].amount <= 0)
                {
                    Items.RemoveAt(i);  // Eliminar la llave si la cantidad es 0
                }
                break;
            }
        }
    }
    
    public void UsePotion()
    {
        // Iteramos por la lista de ítems para encontrar la poción
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].item.type == ItemType.Potion)
            {
                // Reducimos la cantidad de pociones
                Items[i].amount--;

                // Si la cantidad es 0 o menos, eliminamos el ítem
                if (Items[i].amount <= 0)
                {
                    Items.RemoveAt(i);
                }
                return;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}