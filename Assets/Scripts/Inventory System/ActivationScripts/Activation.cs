using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    public static Player PlayerScript;
    public static TMPro.TextMeshProUGUI ActivateInstruction;
    public static Inventory InventoryScript;

    public virtual bool Condition()
    {
        return false;
    }

    public virtual bool Activate()
    {
        return false;
    }

    public virtual bool Deactivate()
    {
        return false;
    }
}
