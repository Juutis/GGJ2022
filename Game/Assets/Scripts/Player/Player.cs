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
    private Swiping swiping;
    private Shooting shooting;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private float gravity = -9.81f * 3f;
    private float jumpHeight = 2f;
    private float yVel = 0f;
    private Vector3 xVel;
    private float leapCharge = 0f;
    private float maxChargeDuration = 2.0f;
    private bool charging = false;
    private float chargeStarted;
    private bool leapNoMovement = false;
    private float checkDistance = 0.4f;
    private bool isGrounded;
    private bool jumpStarted = false;
    private bool jumpOngoing = false; // werewolf leaping
    private float leapStartedTime;

    private bool leapStarted;
    private bool leapOnGoing;

    private Vector3 leapDirection;

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
        controller = GetComponent<CharacterController>();
        swiping = GetComponent<Swiping>();
        shooting = GetComponent<Shooting>();
        if (hideMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        xRotation = -10;

        werewolfParts.SetActive(host.TargetType == TargetEntityType.Werewolf);
        humanParts.SetActive(host.TargetType == TargetEntityType.Human);
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
                jumpStarted = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                shooting.Shoot();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpStarted = true;
            }
            if (Input.GetMouseButtonUp(1) && isGrounded)
            {
                charging = false;
                leapStarted = true;
                leapStartedTime = Time.time;
                leapNoMovement = true;
                leapDirection = cameraObject.forward;
                leapCharge = Mathf.Min(Time.time - chargeStarted, maxChargeDuration) / maxChargeDuration; // 0.0f - 1.0f
            }

            if (Input.GetMouseButton(1) && isGrounded)
            {
                if (!charging) {
                    chargeStarted = Time.time;
                }
                charging = true;
            }

            if (Input.GetMouseButton(0))
            {
                swiping.NormalSwipe();
            }

            if (swiping.CheckFront().Count > 0) {
                EndLeap();
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
        var minLeap = Mathf.Lerp(0.3f, 1.0f, leapCharge);

        isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

        if (isGrounded && yVel < 0 && Time.time - leapStartedTime > minLeap)
        {
            leapNoMovement = false;
            yVel = -2f;
            xVel = Vector3.zero;
        }

        if (isGrounded && leapOnGoing && (Time.time - leapStartedTime) > minLeap && host.TargetType != TargetEntityType.Human)
        {
            EndLeap();
        }

        if (jumpStarted)
        {
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpStarted = false;
        }

        if (leapStarted && host.TargetType != TargetEntityType.Human)
        {
            var leapXZ = leapDirection.normalized;
            leapXZ.y = 0;
            var leapY = leapDirection.normalized.y;
            var yStrength = Mathf.Lerp(0.25f, 1.0f, leapCharge);

            yVel = 50.0f * leapY * yStrength;
            xVel = 50.0f * leapXZ;

            leapStarted = false;
            leapOnGoing = true;
        }

        yVel += gravity * Time.deltaTime;

        float moveBuff = host.TargetType == TargetEntityType.Human ? 1 : wolfMovementBuff;
        controller.Move(
            (transform.right * moveDir.x + transform.forward * moveDir.z) * movementSpeed * moveBuff * Time.deltaTime +
            transform.up * yVel * Time.deltaTime +
            xVel * Time.deltaTime
        );

        float lookSpeedBuff = host.TargetType == TargetEntityType.Human ? 1 : wolfLookSpeedBuff;
        xRotation -= mouseY * mouseSensitivity * lookSpeedBuff;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        cameraObject.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up, mouseX * mouseSensitivity * lookSpeedBuff);
    }

    private void EndLeap() {
        if (leapOnGoing) {
            leapOnGoing = false;
            swiping.HeavySwipe();
        }
    }
}
