using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public LayerMask collisionLayer;

    [SerializeField] GameObject mouseIndicator, cellIndicator;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField]
    private GameObject[] previewObject;

    [SerializeField] private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField] GameObject gridVisulization;

    private bool canPlace = false;
    private GameManager gameManager;

    [SerializeField] private PreviewSystem previewSystem;

    private void Start()
    {
        StopPlacement();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == id);
        if(selectedObjectIndex == -1)
        {
            Debug.LogError($"No ID found {id}");
            return;
        }
        if(selectedObjectIndex == 0)
        {
            gridVisulization.SetActive(true);
            //cellIndicator.SetActive(true);
            previewSystem.StartShowingPlacementPreview(previewObject[selectedObjectIndex]);
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        if( selectedObjectIndex == 1)
        {
            gridVisulization.SetActive(true);
            //cellIndicator.SetActive(true);
            previewSystem.StartShowingPlacementPreview(previewObject[selectedObjectIndex]);
            inputManager.OnClicked += RepairTile;
            inputManager.OnExit += StopPlacement;
        }
    }

    private void RepairTile()
    {
        if(canPlace && gameManager.coin >= 10)
        {
            FindObjectOfType<AudioManager>().Play("Hammer");
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            mouseIndicator.transform.position = mousePosition;
            StartRepair(mouseIndicator.transform);
            gameManager.SetCoin(10);
        }
    }

    private void PlaceStructure()
    {
        if(canPlace && gameManager.coin >= 25)
        {
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            GameObject gameObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
            gameObject.transform.position = grid.CellToWorld(gridPosition) + new Vector3(0.1f, 0, 0.2f);
            PlaceOnTile(gameObject.transform);
            gameManager.SetCoin(25);
        }
        
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisulization.SetActive(false);
        //cellIndicator.SetActive(false);
        previewSystem.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= RepairTile;
        inputManager.OnExit -= StopPlacement;
    }

    private void StartRepair(Transform transform)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer) && hit.collider.CompareTag("Tile"))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject);

            if (hitObject != null)
            {
                TileHealth health = hitObject.GetComponent<TileHealth>();
                Debug.Log(health.amount);
                health.amount = health.maxHealth;
                Debug.Log(health.amount);
            }

        }
    }

    private void PlaceOnTile(Transform transform)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer) && hit.collider.CompareTag("Tile"))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject);
            
            if (hitObject != null)
            {
                Tile tile = hitObject.GetComponent<Tile>();
                //Debug.Log(tile);
                if (tile.tileObject != null)
                {
                    Destroy(tile.tileObject);
                }
                tile.tileObject = transform.gameObject;
            }

        }

    }

    private bool CheckTile(Transform transform)
    {
        GameObject canPlaceIcon = cellIndicator.transform.Find("CanPlaceIcon").gameObject;
        GameObject cannotPlaceIcon = cellIndicator.transform.Find("CannotPlaceIcon").gameObject;

        Ray ray = new Ray(transform.position, Vector3.down);
        //Vector3 mousePositionWithOffset = Input.mousePosition + new Vector3(0, 0, 0.5f);
        //Ray ray = Camera.main.ScreenPointToRay(mousePositionWithOffset);

        RaycastHit hit;

        LayerMask layerMask = LayerMask.GetMask("Tile", "Water");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int hitLayer = hit.collider.gameObject.layer;
            string layerName = LayerMask.LayerToName(hitLayer);

            if (hitLayer == LayerMask.NameToLayer("Tile"))
            {
                cannotPlaceIcon.SetActive(false);
                canPlaceIcon.SetActive(true);
                return true;
            }
            else
            {
                cannotPlaceIcon.SetActive(true);
                canPlaceIcon.SetActive(false);
            }
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if(selectedObjectIndex == -1) { return; }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        canPlace = CheckTile(mouseIndicator.transform);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), canPlace);
    }
}
