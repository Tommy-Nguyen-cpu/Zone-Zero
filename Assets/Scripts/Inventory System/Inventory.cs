using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    (int, bool, int) previousButton = (0, true, -1);
    public ItemViewScript itemViewController;
    public GameObject ItemNameDisplay;
    public GameObject itemInstructions;
    public IconMaker iconMaker;
    List<GameObject> Items = new List<GameObject>();

    public bool AddItem(GameObject item)
    {
        int emptySlotIndex = -1;
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i] == null)
            {
                emptySlotIndex = i;
                break;
            }
        }
        if (emptySlotIndex != -1)
        {
            Items[emptySlotIndex] = item;
            iconMaker.AddIcon(item);
            return true;
        }
        else if(Items.Count < iconMaker.cams.Count)
        {
            Items.Add(item);
            iconMaker.AddIcon(item);
            return true;
        }

        return false;
    }

    private void Dropitem(Transform player)
    {
        // If we already dropped the item.
        if (Items[previousButton.Item3] == null)
            return;

        iconMaker.RemoveIcon(Items[previousButton.Item3]);
        Items[previousButton.Item3].transform.position = player.position + player.forward;
        Items[previousButton.Item3] = null;


        previousButton.Item2 = !previousButton.Item2;
        ItemNameDisplay.SetActive(previousButton.Item2);
        itemInstructions.SetActive(previousButton.Item2);
    }

    private void ViewItem(Transform player)
    {
        // TODO: Probably should provide players with controls to zoom in and out and rotate item when viewing.
        if (Items[previousButton.Item3] == null)
            return;

        itemViewController.InitiateViewItem(Items[previousButton.Item3]);
    }

    /// <summary>
    /// Based on player input, display the corresponding item.
    /// </summary>
    public void SelectItem()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)iconMaker.ItemSlotKeys[i]) && Items[i] != null)
            {
                ItemNameDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = Items[iconMaker.ItemSlotKeys[i] - 49].name;

                if (iconMaker.ItemSlotKeys[i] == previousButton.Item1)
                {
                    previousButton.Item2 = !previousButton.Item2;
                    ItemNameDisplay.SetActive(previousButton.Item2);
                    itemInstructions.SetActive(previousButton.Item2);
                }
                else
                {
                    ItemNameDisplay.SetActive(true);
                    itemInstructions.SetActive(true);
                    previousButton.Item1 = iconMaker.ItemSlotKeys[i];
                    previousButton.Item2 = true;
                    previousButton.Item3 = i;
                }
            }
        }
    }

    public void ItemAction(Transform player)
    {
        if (!ItemNameDisplay.activeInHierarchy)
            return;

        // Activate Item
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
        // View Item
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ViewItem(player);
        }
        // Drop Item
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Dropitem(player);
        }
    }
}
