using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 100f;
    public float jumpHeight = 10f;

    public CharacterController controller;

    public Canvas pauseMenu;
    public static bool isPaused = false;


    #region Rotation Variables

    /// <summary>
    /// How much the player rotates based on mouse movement.
    /// </summary>
    public float mouseSensitivity = 100f;

    private float xRotation = 0.0f;
    #endregion

    #region Physics Fields

    Vector3 velocity;
    public float gravity = -9.81f;

    private bool isGrounded = true;
    public LayerMask groundMask;
    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPaused)
        {
            // If the player is not pausing the game, lock the mouse to the center.
            Cursor.lockState = CursorLockMode.Locked;


            GravitationalForce();
            PlayerMovement();
            AdjustPlayerRotation();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            pauseMenu.gameObject.SetActive(true);
            isPaused = true;
        }
    }

    /// <summary>
    /// Method responsible for allowing player movement.
    /// </summary>
    private void PlayerMovement()
    {
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        // Get a vector telling us the direction the user is moving in.
        Vector3 move = transform.forward * z + transform.right * x;

        // If the player is grounded and they click a "jump" button, using the physics equation to increase players y.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            move.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Move the player in the direction, with a specific speed, and independent of frame rates.
        controller.Move(move * playerSpeed * Time.deltaTime);
    }


    /// <summary>
    /// Method responsible for player rotation.
    /// </summary>
    private void AdjustPlayerRotation()
    {
        // Gets the x and y mouse coordinates, and ensures that rotation is frame rate independent.
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;

        // Clamps the up and down rotation to 90 degrees.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotation in the left and right direction.
        float rotationY = transform.localEulerAngles.y + MouseX;

        transform.localRotation = Quaternion.Euler(xRotation, rotationY, 0f);
    }

    private void GravitationalForce()
    {
        // Checks to see if the player hits the ground.
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Velocity increases as an object falls (free fall).
        velocity.y += gravity * Time.deltaTime;

        // Change in y is found via: 1/2*g * t^2. Where "g" is the gravitational constant and "t" is time.
        controller.Move(velocity * Time.deltaTime);
    }
}
