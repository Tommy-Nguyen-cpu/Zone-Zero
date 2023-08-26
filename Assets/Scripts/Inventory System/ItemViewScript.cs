using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemViewScript : MonoBehaviour
{
    #region Scripts To Enable/Disable
    public Player playerScript;
    public Inventory inventoryScript;

    #endregion

    GameObject ViewedItem;
    Vector3 InitialPosition;
    public void InitiateViewItem(GameObject item)
    {
        ViewedItem = item;
        InitialPosition = ViewedItem.transform.position;
        ViewedItem.transform.parent = gameObject.transform;
        ViewedItem.transform.position = transform.position + .2f * transform.forward;
        ViewedItem.transform.LookAt(transform.forward);

        playerScript.enabled = false;
        inventoryScript.enabled = false;
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // TODO: Sometimes when we disable it, the "Press E to pick up" flickers on screen.
            ViewedItem.transform.parent = null;
            ViewedItem.transform.position = InitialPosition;
            playerScript.enabled = true;
            inventoryScript.enabled = true;
            this.enabled = false;
        }

        Debug.Log("Rotating...");
        // TODO: Create mouse sensitivity static field to be used in all locations where mouse speed is used.
        ViewedItem.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 100f);

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            ViewedItem.transform.position -= .005f * transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ViewedItem.transform.position += .005f * transform.forward;
        }
    }
}
