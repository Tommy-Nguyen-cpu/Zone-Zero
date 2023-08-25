using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
}
