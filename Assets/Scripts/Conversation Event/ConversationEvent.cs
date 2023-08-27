using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationEvent : MonoBehaviour
{
    public GameObject panel, textPrefab;

    Dictionary<List<(string, Color)>, string> Conversation = new Dictionary<List<(string, Color)>, string>();

    /// <summary>
    /// Makes sure we only run the program once.
    /// </summary>
    private bool RanOnce = false;
    private float Delay = 2f;

    // Update is called once per frame
    void Update()
    {
        if(Conversation.Count > 0 && !RanOnce)
        {
            RanOnce = true;
            StartCoroutine(DisplayConversation());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(gameObject);
        }
    }

    IEnumerator DisplayConversation()
    {
        foreach(var pair in Conversation)
        {
            GameObject newText = Instantiate(textPrefab, panel.transform);
            newText.GetComponent<TMPro.TextMeshProUGUI>().text = pair.Key[0].Item1 + ":" + pair.Value;
            newText.GetComponent<TMPro.TextMeshProUGUI>().color = pair.Key[0].Item2;
            yield return new WaitForSeconds(Delay);
        }

        Conversation = new Dictionary<List<(string, Color)>, string>();
        yield break;
    }

    public void SetConversation(Dictionary<List<(string, Color)>, string> newConversation, float delayTime = 2f)
    {
        RanOnce = false;
        Conversation = newConversation;
        Delay = delayTime;
    }
}
