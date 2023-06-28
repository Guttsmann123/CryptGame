using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 0.125f;

    private float currentRotation = 0f;
    private float verticalRotation = 0f;
    public float rotationLimit = 80f;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (this.enabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            currentRotation -= mouseX;
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -rotationLimit, rotationLimit);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    void LateUpdate()
    {
        if (this.enabled)
        {
            float desiredYAngle = playerBody.eulerAngles.y;
            float desiredXAngle = verticalRotation;
            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0f);

            Vector3 desiredPosition = playerBody.position - (rotation * Vector3.forward * distance);
            desiredPosition.y = playerBody.position.y + height;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothSpeed);
        }
    }
}
