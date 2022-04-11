using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private CameraSO _mainCamera;

    public UnityEvent onShoot = new UnityEvent();
    public UnityEvent<Vector2> onMoveBody = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> onMoveTurret = new UnityEvent<Vector2>();

    public void Update()
    {
        GetBodyMovement();
        GetTurretMovement();
        GetShootingInput();
    }

    private void GetBodyMovement()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        onMoveBody?.Invoke(moveVector.normalized);
    }

    private void GetTurretMovement()
    {
        onMoveTurret?.Invoke(GetMousePos());
    }

    private void GetShootingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onShoot?.Invoke();
        }
    }

    private Vector2 GetMousePos()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _mainCamera.camera.nearClipPlane;
        Vector2 mouseWorldPos = _mainCamera.camera.ScreenToWorldPoint(mousePosition);
        return mouseWorldPos;
    }


}
