using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<GameObject> Items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        Items.Add(item);
    }

    public void Dropitem(GameObject item)
    {
        Items.Remove(item);
    }
}
