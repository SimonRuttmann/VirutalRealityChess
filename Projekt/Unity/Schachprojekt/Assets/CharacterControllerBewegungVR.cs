using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class CharacterControllerBewegungVR : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector2 trackpad;
    public SteamVR_Input_Sources Hand;
    public GameObject AxisHand;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;
    public float Deadzone=0.5f;


    private void Start()
    {
        
        //controller = gameObject.AddComponent<CharacterController>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        updateInput();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;
        if (trackpad.magnitude > Deadzone) {
        //Debug.Log("TrackpadMag: "+trackpad.magnitude);
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
        // Changes the height position of the player..
        /*
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        */
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
    private void updateInput()
    {
        trackpad = SteamVR_Actions._default.MoveVR.GetAxis(Hand);

    }
}
