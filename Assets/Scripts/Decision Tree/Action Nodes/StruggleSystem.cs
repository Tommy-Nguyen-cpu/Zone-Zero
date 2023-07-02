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

    private bool Stop = false;

    public delegate void HealthDecrement();
    public HealthDecrement DecreaseHealth;

    List<KeyCode> PotentialKeys = new List<KeyCode>{KeyCode.A, KeyCode.B, KeyCode.C };

    private void OnEnable()
    {
        Debug.Log("Got to struggle!");
        Stop = false;
        // Disables enemy tree and player movement.
        EnemyTree.enabled = false;
        Player.GetComponent<Player>().enabled = false;
        Player.GetComponent<CharacterController>().enabled = false;

        // Picks random key for this quick time event run.
        QTButton = PotentialKeys[Random.Range(0, PotentialKeys.Count)];
        tmpText.text = QTButton.ToString();
        QTFillAmount = .5f;

        Player.transform.LookAt(new Vector3(transform.position.x, -1*transform.position.y, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (!Stop)
        {
            if (Input.GetKeyDown(QTButton))
            {
                // Debug.Log($"pressed {QTButton}!");
                QTFillAmount += .2f;
            }
            TimeThreshold += Time.deltaTime;

            if (TimeThreshold > .02f)
            {
                TimeThreshold = 0f;
                QTFillAmount -= .02f;
            }
            if (QTFillAmount < 0)
            {
                QTFillAmount = 0;
                EventFailed();
            }

            if (QTFillAmount >= 1)
            {
                EventSuccess();
            }

            QT.fillAmount = QTFillAmount;
        }
    }


    void EventSuccess()
    {
        DecreaseHealth?.Invoke();
        Player.GetComponent<Player>().enabled = true;
        Player.GetComponent<CharacterController>().enabled = true;
        Player.transform.rotation = Quaternion.identity;
        StopAllCoroutines();

        // Hides QT UI.
        tmpText.text = "";
        QTFillAmount = 0f;
        // TODO: Not entirely sure if we need the "StunnedNode" now since this will pause the enemy.
        // Stuns the enemy.
        Stop = true;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);

        // After the enemy has been stunned for a couple of seconds, we enable the decision tree and disable struggle system script.
        EnemyTree.enabled = true;
        this.enabled = false;
    }


    void EventFailed()
    {
        // TODO: Implement once win/lost state is implemented.
        Debug.Log("Lost game!");
    }
}
