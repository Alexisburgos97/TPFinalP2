using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEN_ITEMS;

    private Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        ClearDisplay();
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            var obj = Instantiate(inventory.Items[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Items[i], obj);
        }
    }

    public void UpdateDisplay()
    {
        
        // Lista temporal para almacenar los ítems visuales que deben ser eliminados
        List<InventorySlot> itemsToRemove = new List<InventorySlot>();

        foreach (var displayItem in itemsDisplayed)
        {
            // Si el ítem ya no existe en el inventario o su cantidad es 0
            if (!inventory.Items.Contains(displayItem.Key) || displayItem.Key.amount <= 0)
            {
                // Agregar a la lista de eliminación
                itemsToRemove.Add(displayItem.Key);
            }
            else
            {
                // Actualizar texto de cantidad
                displayItem.Value.GetComponentInChildren<TextMeshProUGUI>().text = displayItem.Key.amount.ToString("n0");
            }
        }

        // Eliminar visualmente los ítems con cantidad 0
        foreach (var itemSlot in itemsToRemove)
        {
            Destroy(itemsDisplayed[itemSlot]);
            itemsDisplayed.Remove(itemSlot);
        }

        // Verificar si hay ítems nuevos que no están visualizados
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (!itemsDisplayed.ContainsKey(inventory.Items[i]))
            {
                var obj = Instantiate(inventory.Items[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Items[i], obj);
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + ((-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS))), 0f);
    }
    
    public void ClearDisplay()
    {
        foreach (var item in itemsDisplayed)
        {
            Destroy(item.Value);
        }
        itemsDisplayed.Clear();
    }
}