using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera _camera;

    private Vector3 lastPosition;

    [SerializeField] LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _camera.nearClipPlane;
        Ray ray = _camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
