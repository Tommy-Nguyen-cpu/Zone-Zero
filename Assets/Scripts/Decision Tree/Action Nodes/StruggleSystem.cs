using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleSystem : MonoBehaviour
{
    public DecisionTree EnemyTree;
    public GameObject Player;
    public UnityEngine.UI.Image QT;
    public TMPro.TMP_Text tmpText;
    private KeyCode QTButton;

    private float QTFillAmount = .5f;
    private float TimeThreshold = 0f;

    List<KeyCode> PotentialKeys = new List<KeyCode>{KeyCode.A, KeyCode.B, KeyCode.C };

    private void OnEnable()
    {
        Debug.Log("Got to struggle!");
        // Disables enemy tree and player movement.
        EnemyTree.enabled = false;
        Player.GetComponent<Player>().enabled = false;
        Player.GetComponent<CharacterController>().enabled = false;

        // Picks random key for this quick time event run.
        QTButton = PotentialKeys[Random.Range(0, PotentialKeys.Count)];
        tmpText.text = QTButton.ToString();

        Player.transform.LookAt(new Vector3(transform.position.x, -1*transform.position.y, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(QTButton))
        {
            // Debug.Log($"pressed {QTButton}!");
            QTFillAmount += .2f;
        }
        TimeThreshold += Time.deltaTime;

        if(TimeThreshold > .02f)
        {
            TimeThreshold = 0f;
            QTFillAmount -= .02f;
        }
        if (QTFillAmount < 0)
            QTFillAmount = 0;
        QT.fillAmount = QTFillAmount;
    }


    void EventSuccess()
    {

    }


    void EventFailed()
    {

    }
}
