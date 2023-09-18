using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action<Card> OnCardClicked = delegate { }; 
    [SerializeField] new Camera camera;
    Vector3 cameraOrigin;
    private void Awake()
    {
        cameraOrigin = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        CheckClickPosition();
    }

    void CameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            camera.transform.position = cameraOrigin;
        }
        else
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var zoom = Input.GetAxis("Mouse ScrollWheel");
            var deltaTime = Time.deltaTime;
            var cameraMovement = new Vector3(horizontal * deltaTime * 8, -zoom * deltaTime * 80, vertical * deltaTime * 8);
            if (cameraMovement != Vector3.zero)
            {
                var position = camera.transform.position + cameraMovement;
                position = new Vector3(Math.Clamp(position.x, -4, 4), Math.Clamp(position.y, 15, 25), Math.Clamp(position.z, -10, 10));
                camera.transform.position = position;
            }
        }
    }

    void CheckClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var parent = hitInfo.collider.transform.parent;
                if (parent != null && parent.TryGetComponent<Card>(out var card) && card.CanBeSelected)
                {
                    OnCardClicked(card);
                }
            }
        }
    }
}
