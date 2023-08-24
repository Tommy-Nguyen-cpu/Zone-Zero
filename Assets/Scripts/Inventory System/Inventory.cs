using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public IconMaker iconMaker;
    List<GameObject> Items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        Items.Add(item);
        iconMaker.AddIcon(item);
    }

    public void Dropitem(GameObject item)
    {
        Items.Remove(item);
        iconMaker.RemoveIcon(item);
    }
}
