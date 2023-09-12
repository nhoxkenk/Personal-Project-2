using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Tốc độ di chuyển của kẻ địch
    public Vector3 targetPos; // Vị trí mục tiêu (gần điểm spawn)

    private void Update()
    {
        // Kiểm tra khoảng cách giữa kẻ địch và vị trí mục tiêu
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        // Nếu khoảng cách còn xa, di chuyển tới vị trí mục tiêu
        if (distanceToTarget > 0.2f)
        {
            // Tính toán hướng di chuyển
            Vector3 moveDirection = (targetPos - transform.position).normalized;

            // Di chuyển kẻ địch theo hướng
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}