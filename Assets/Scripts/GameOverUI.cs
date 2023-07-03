using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image GameOverText;
    [SerializeField]
    private Image YouWinText;
    [SerializeField]
    private Image RestartButton;
    [SerializeField]
    private Image QuitButton;

    public void Start()
    {
        clear();
    }

    public void ShowGameOver()
    {
        canvas.enabled = true;
        GameOverText.enabled = true;
        RestartButton.enabled = true;
        QuitButton.enabled = true;
    }

    public void ShowWinScreen()
    {
        canvas.enabled = true;
        YouWinText.enabled = true;
        RestartButton.enabled = true;
        QuitButton.enabled = true;
    }

    public void clear()
    {
        canvas.enabled = false;
        GameOverText.enabled = false;
        YouWinText.enabled = false;
        RestartButton.enabled = false;
        QuitButton.enabled = false;
    }
}
