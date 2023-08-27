using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkmanActivation : Activation
{
    public GameObject ConversationUIPrefab;
    public GameObject InstantiatedConversationUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Deactivate();
        }

        GameObject currentItem = InventoryScript.GetCurrentItem();
        if(currentItem != null)
        {
            if (!GameObject.Find("Conversation"))
            {
                Conversations conversation = currentItem.GetComponent<Conversations>();
                if(conversation != null)
                {
                    InstantiatedConversationUI = Instantiate(ConversationUIPrefab);
                    InstantiatedConversationUI.name = InstantiatedConversationUI.name.Replace("(Clone)", "");
                    ConversationEvent ConversationTrigger = InstantiatedConversationUI.GetComponent<ConversationEvent>();
                    ConversationTrigger.SetConversation(conversation.GetConversations());
                    InstantiatedConversationUI.SetActive(true);
                    Deactivate();
                }
            }
        }
    }

    public override bool Condition()
    {
        return base.Condition();
    }

    public override bool Activate()
    {
        Cursor.lockState = CursorLockMode.None;
        ActivateInstruction.gameObject.SetActive(true);
        ActivateInstruction.text = "Select an item slot that contains a cassette tape to play the cassette tape.";
        enabled = true;
        return base.Activate();
    }

    public override bool Deactivate()
    {
        ActivateInstruction.gameObject.SetActive(false);
        enabled = false;
        return base.Deactivate();
    }
}
