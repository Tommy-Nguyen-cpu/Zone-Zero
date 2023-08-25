using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    (int, bool) previousButton = (0, true);
    public GameObject ItemNameDisplay;
    public IconMaker iconMaker;
    List<GameObject> Items = new List<GameObject>();

    public bool AddItem(GameObject item)
    {
        // Only add items if we have less items than we have cameras.
        if(Items.Count < iconMaker.cams.Count)
        {
            Items.Add(item);
            iconMaker.AddIcon(item);
            return true;
        }
        return false;
    }

    public void Dropitem(GameObject item)
    {
        Items.Remove(item);
        iconMaker.RemoveIcon(item);
    }

    /// <summary>
    /// Based on player input, display the corresponding item.
    /// </summary>
    public void SelectItem()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)iconMaker.ItemSlotKeys[i]))
            {
                ItemNameDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = Items[iconMaker.ItemSlotKeys[i] - 49].name;

                if (iconMaker.ItemSlotKeys[i] == previousButton.Item1)
                {
                    previousButton.Item2 = !previousButton.Item2;
                    ItemNameDisplay.SetActive(previousButton.Item2);
                }
                else
                {
                    ItemNameDisplay.SetActive(true);
                    previousButton.Item1 = iconMaker.ItemSlotKeys[i];
                    previousButton.Item2 = true;
                }
                return;
            }
        }
    }
}
