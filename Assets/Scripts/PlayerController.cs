using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform graphics;

    [SerializeField]
    private float rotationSpeed = 5f;

    public float moveSpeed = 5.0f;  // Tốc độ di chuyển của nhân vật

    private List<Tile> lastTilesTouched = new List<Tile>();

    private CharacterController characterController;
    private PlacementSystem placementSystem;
    [SerializeField] private GameManager gameManager;
    Vector3 direction;
    Rigidbody rb;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        placementSystem = GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Lấy thông tin về phím từ bàn phím
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Tạo vector di chuyển dựa trên thông tin phím
        direction = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        Vector3 vector = GetMousePosition(Input.mousePosition) - base.transform.position;
        vector.y = 0f;
        float y = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
        Quaternion b = Quaternion.Euler(0f, y, 0f);
        graphics.transform.rotation = Quaternion.Slerp(graphics.transform.rotation, b, rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Q))
            placementSystem.StartPlacement(0);
        if (Input.GetKeyDown(KeyCode.E))
            placementSystem.StartPlacement(1);
    }

    private void FixedUpdate()
    {
        // Di chuyển nhân vật
        //characterController.Move(direction * moveSpeed * Time.deltaTime);
        Vector3 vector = direction * moveSpeed;
        rb.velocity = new Vector3(vector.x, rb.velocity.y, vector.z);
        //TileDetec();
    }

    private Vector3 GetMousePosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Vector3 result = Vector3.zero;
        if (plane.Raycast(ray, out var enter))
        {
            result = ray.GetPoint(enter);
        }
        result.y = transform.position.y; // Giữ độ cao của nhân vật không thay đổi
        return result;
    }

    void TileDetec()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            if (hit.transform.CompareTag("Tile"))
            {
                if (lastTilesTouched.Count >= 3)
                {
                    lastTilesTouched.RemoveAt(0);
                }
                lastTilesTouched.Add(hit.transform.gameObject.GetComponent<Tile>());
            }
            foreach (var tile in lastTilesTouched)
            {
                Debug.Log(tile.name);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.CompareTag("Tile"))
        {
            //Debug.Log("Tile");
            if (lastTilesTouched.Count >= 3)
            {
                lastTilesTouched.RemoveAt(0);
            }
            lastTilesTouched.Add(collision.gameObject.GetComponent<Tile>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("Stuck");
            gameManager.SpawnPlayer(lastTilesTouched);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("Stuck");
            gameManager.SpawnPlayer(lastTilesTouched);
        }
    }
}