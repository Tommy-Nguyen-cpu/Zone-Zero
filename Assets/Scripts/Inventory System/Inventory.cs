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
    public List<GameObject> Items = new List<GameObject>();

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
        if (Items[previousButton.Item3] == null)
            return;

        itemViewController.InitiateViewItem(Items[previousButton.Item3]);
    }

    private void ActivateItem()
    {
        if (Items[previousButton.Item3] == null)
            return;

        Activation activateScript = Items[previousButton.Item3].GetComponent<Activation>();
        if(activateScript != null)
        {
            Debug.Log($"{Items[previousButton.Item3].name} has activation script!");
            activateScript.Activate();
        }
    }

    /// <summary>
    /// Based on player input, display the corresponding item.
    /// </summary>
    public GameObject SelectItem()
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
                    // In situations where the player selects and then unselects.
                    return Items[previousButton.Item3];
                }
                else
                {
                    ItemNameDisplay.SetActive(true);
                    itemInstructions.SetActive(true);
                    previousButton.Item1 = iconMaker.ItemSlotKeys[i];
                    previousButton.Item2 = true;
                    previousButton.Item3 = i;
                    return Items[i];
                }
            }
        }
        return null;
    }

    public GameObject GetCurrentItem()
    {
        if (previousButton.Item3 == -1)
            return null;
        return Items[previousButton.Item3];
    }

    public void ItemAction(Transform player)
    {
        if (!ItemNameDisplay.activeInHierarchy)
            return;

        // Activate Item
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateItem();
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
