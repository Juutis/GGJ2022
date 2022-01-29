using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool hideMouse;
    [SerializeField]
    private float mouseSensitivity;
    [SerializeField]
    private Transform cameraObject;
    [SerializeField]
    private GameObject humanParts;
    [SerializeField]
    private GameObject werewolfParts;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float wolfMovementBuff;
    [SerializeField]
    private float wolfLookSpeedBuff;
    [SerializeField]
    private float leapChargeSpeed;
    private TargetEntity host;
    private CharacterController controller;
    private Vector3 moveDir;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private float gravity = -9.81f * 3f;
    private float jumpHeight = 2f;
    private float yVel = 0f;
    private float xVel = 0f;
    private float leapCharge = 0f;
    private float startCharge = 3f;
    private bool charging = false;
    private bool leapNoMovement = false;
    private float checkDistance = 0.4f;
    private bool isGrounded;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
        controller = GetComponent<CharacterController>();
        if (hideMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        xRotation = -10;

        werewolfParts.SetActive(host.TargetType == TargetEntityType.Werewolf);
        humanParts.SetActive(host.TargetType == TargetEntityType.Human);
        leapCharge = startCharge;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Vector3.zero;
        if (!leapNoMovement)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDir.z = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir.z = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDir.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir.x = 1;
            }

            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     host.TogglePlayerTargetType();
            //     humanParts.SetActive(host.TargetType == TargetEntityType.Human);
            //     werewolfParts.SetActive(host.TargetType == TargetEntityType.Werewolf);
            // }
        }

        if (host.TargetType == TargetEntityType.Human)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jump = true;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
            {
                charging = false;
                jump = true;
                // TODO: stop updating movement direction. => can look around but don't rotate direction
            }

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                charging = true;
                leapNoMovement = true;
                moveDir = Vector3.zero;
            }
        }

        if (DayChanger.main.IsNight())
        {
            host.SetPlayerTargetType(TargetEntityType.Werewolf);
        }
        else
        {
            host.SetPlayerTargetType(TargetEntityType.Human);
            // changing to human, disable leap charging
            leapNoMovement = false;
            charging = false;
            leapCharge = 0;
        }

        humanParts.SetActive(host.TargetType == TargetEntityType.Human);
        werewolfParts.SetActive(host.TargetType == TargetEntityType.Werewolf);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

        if (isGrounded && yVel < 0)
        {
            leapNoMovement = false;
            yVel = -2f;
            xVel = 0f;
        }

        if (jump && host.TargetType == TargetEntityType.Human)
        {
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jump = false;
        }
        else if (jump && host.TargetType != TargetEntityType.Human)
        {
            float yCharge = Mathf.Max(leapCharge * 0.08f, jumpHeight);
            yVel = Mathf.Sqrt(yCharge * -2f * gravity);
            xVel = Mathf.Sqrt(leapCharge * -2f * gravity);

            jump = false;
            leapCharge = startCharge;
        }

        yVel += gravity * Time.deltaTime;

        float moveBuff = host.TargetType == TargetEntityType.Human ? 1 : wolfMovementBuff;
        controller.Move(
            (transform.right * moveDir.x + transform.forward * moveDir.z) * movementSpeed * moveBuff * Time.deltaTime +
            transform.up * yVel * Time.deltaTime +
            transform.forward * xVel * Time.deltaTime
        );

        float lookSpeedBuff = host.TargetType == TargetEntityType.Human ? 1 : wolfLookSpeedBuff;
        xRotation -= mouseY * mouseSensitivity * lookSpeedBuff;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        cameraObject.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up, mouseX * mouseSensitivity * lookSpeedBuff);


        if (charging)
        {
            Debug.Log(leapCharge);
            leapCharge += Time.deltaTime * leapChargeSpeed;
        }
    }
}
