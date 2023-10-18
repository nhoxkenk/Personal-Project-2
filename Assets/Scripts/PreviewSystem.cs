using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    private void Start()
    {
        cellIndicator.SetActive(false);
    }

    public void StartShowingPlacementPreview(GameObject prefab)
    {
        previewObject = Instantiate(prefab);
        cellIndicator.SetActive(true);
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false );
        Destroy( previewObject );
    }

    public void UpdatePosition(Vector3 pos,bool check)
    {
        if (!check)
            previewObject.SetActive(false);
        else
            previewObject.SetActive(true);
        MovePreview(pos);
        MoveCursor(pos);
    }

    private void MoveCursor(Vector3 pos)
    {
        cellIndicator.transform.position = pos;
    }

    private void MovePreview(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(
            pos.x,
            pos.y + previewYOffset,
            pos.z);
    }

}
