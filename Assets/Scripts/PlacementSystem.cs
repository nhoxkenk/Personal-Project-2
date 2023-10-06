using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public LayerMask collisionLayer;

    [SerializeField] GameObject mouseIndicator, cellIndicator;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField] GameObject gridVisulization;

    private bool canPlace = false;

    private void Start()
    {
        StopPlacement();
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
        gridVisulization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(canPlace)
        {
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            GameObject gameObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
            gameObject.transform.position = grid.CellToWorld(gridPosition);
            PlaceOnTile(gameObject.transform);
        }
        
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisulization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void PlaceOnTile(Transform transform)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            Transform parentObject = hitObject.transform.parent;

            if (parentObject != null)
            {
                Tile tile = parentObject.GetComponent<Tile>();
                if(tile.tileObject != null)
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

            Debug.Log("Tên layer của đối tượng được hit: " + layerName);

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
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        canPlace = CheckTile(mouseIndicator.transform);
    }
}
