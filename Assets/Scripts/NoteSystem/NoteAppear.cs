using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;



public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private Image _note_bg;
    [SerializeField]
    private TMP_Text _note_txt;
    [SerializeField]
    private TMP_Text instruction;

    // Start is called before the first frame update

    public void Start()
    {
        _note_bg.enabled = false;
        _note_txt.enabled = false;
        instruction.enabled = false;
    }

    public Image note_bg
    {
        get
        {
            return _note_bg;
        }
        set
        {
            _note_bg = value;
        }
    }

    public TMP_Text note_txt_1
    {
        get
        {
            return _note_txt;
        }
        set
        {
            _note_txt = value;
        }
    }

    public void ShowNote()
    {
        _note_bg.enabled = true;
        _note_txt.enabled = true;
        instruction.enabled = true;
    }

    public void HideNote()
    {
        _note_bg.enabled = false;
        _note_txt.enabled = false;
        instruction.enabled = false;
    }
    /*

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _note_bg.enabled = true;
            _note_txt_1.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _note_bg.enabled = false;
            _note_txt_1.enabled = false;
        }
    }
    */
}
