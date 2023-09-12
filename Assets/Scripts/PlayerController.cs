using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 5.0f;  // Tốc độ di chuyển của nhân vật
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Lấy thông tin về phím từ bàn phím
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Tạo vector di chuyển dựa trên thông tin phím
        Vector3 direction = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

}