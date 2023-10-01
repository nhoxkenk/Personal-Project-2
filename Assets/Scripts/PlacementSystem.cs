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
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject gameObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        gameObject.transform.position = grid.CellToWorld(gridPosition) + new Vector3(0.2f, 0, 0.2f);
        checkTile(gameObject.transform);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisulization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void checkTile(Transform transform)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer))
        {
            Debug.Log("Đối tượng đang đặt chồng lên đối tượng khác.");
            GameObject hitObject = hit.collider.gameObject;

            Transform parentObject = hitObject.transform.parent;

            if (parentObject != null)
            {
                Debug.Log("Đối tượng cha: " + parentObject.name);
                Tile tile = parentObject.GetComponent<Tile>();
                if(tile.tileObject != null)
                {
                    Destroy(tile.tileObject);
                }
                tile.tileObject = transform.gameObject;
            }
            else
            {
                Debug.Log("Không có đối tượng cha.");
            }
        }
        else
        {
            Debug.Log("Đối tượng không đặt chồng lên đối tượng khác.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedObjectIndex == -1) { return; }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
