using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float movementSpeed = 4f, rotationSpeed = 180f, mouseSensitivity =5f;

    [SerializeField]
    float startingVerticalEyeAngle = 10f;

    CharacterController characterController;
    Transform eye;
    Vector2 eyeAngles;

    CapsuleCollider player_collider;

    int note_level = 0;
    bool awake = false;
    public delegate void NoteDelegate(); //NoteFound
    public NoteDelegate NoteFound;
    public NoteDelegate NoteDropped;
    private void Awake()
    {
        if (!awake)
        {
            player_collider = GetComponent<CapsuleCollider>();
            characterController = GetComponent<CharacterController>();
            eye = transform.GetChild(0);
            awake = true;
        }
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Note")
        {
            note_level += 1;
            NoteFound?.Invoke();
            print("Found note!");
            //Destroy(collider.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Note")
        {
            NoteDropped?.Invoke();
            Destroy(other.gameObject);
        }
    }

    public void StartNewGame (Vector3 position)
    {
        Awake();
        eyeAngles.x = Random.Range(0f, 360f);
        eyeAngles.y = startingVerticalEyeAngle;
        characterController.enabled = false;
        transform.localPosition = position;
        characterController.enabled = true;
    }

    public Vector3 Move()
    {
        UpdateEyeAngles();
        UpdatePosition();
        return transform.localPosition;
    }

    void UpdatePosition()
    {
        var movement = new Vector2(
            Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")
        );
        float sqrMagnitude = movement.sqrMagnitude;
        if (sqrMagnitude > 1f)
        {
            movement /= Mathf.Sqrt(sqrMagnitude);
        }
        movement *= movementSpeed;

        var forward = new Vector2(
            Mathf.Sin(eyeAngles.x * Mathf.Deg2Rad),
            Mathf.Cos(eyeAngles.x * Mathf.Deg2Rad)
        );
        var right = new Vector2(forward.y, -forward.x);

        movement = right * movement.x + forward * movement.y;
        characterController.SimpleMove(new Vector3(movement.x, 0f, movement.y));
    }

    void UpdateEyeAngles()
    {
        float rotationDelta = rotationSpeed * Time.deltaTime;
        eyeAngles.x += rotationDelta * Input.GetAxis("Horizontal View");
        eyeAngles.y -= rotationDelta * Input.GetAxis("Vertical View");
        if (mouseSensitivity > 0f)
        {
            float mouseDelta = rotationDelta * mouseSensitivity;
            eyeAngles.x += mouseDelta * Input.GetAxis("Mouse X");
            eyeAngles.y -= mouseDelta * Input.GetAxis("Mouse Y");
        }

        if (eyeAngles.x > 360f)
        {
            eyeAngles.x -= 360f;
        }
        else if (eyeAngles.x < 0f)
        {
            eyeAngles.x += 360f;
        }
        eyeAngles.y = Mathf.Clamp(eyeAngles.y, -45f, 45f);
        eye.localRotation = Quaternion.Euler(eyeAngles.y, eyeAngles.x, 0f);
    }
    //This method is used when the player reaches the end without acquiring all notes. 
    public void PlayerReset()
    {
        print("PLAYER RESET");
        this.note_level = 0;
    }

    public int GetNoteLevel()
    {
        return this.note_level;
    }
}
