using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    new public Rigidbody2D rigidbody2D;
    public float maxSpeed = 10f;
    public float rotationSpeed = 100f;
    public Turret[] turrets;

    private Vector2 _moveVector;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if(turrets == null)
        {
            turrets = GetComponentsInChildren<Turret>();
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = (Vector2)transform.up * _moveVector.y * maxSpeed * Time.fixedDeltaTime;
        rigidbody2D.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -_moveVector.x * rotationSpeed * Time.fixedDeltaTime));
    }

    public void HandleShoot()
    {
        Debug.Log("trying to shoot");
        foreach (var turret in turrets)
        {
            turret.Shoot();
            Debug.Log("shots fired");
        }
    }

    public void HandleMoveBody(Vector2 _moveVector)
    {
        this._moveVector = _moveVector;
    }

    public void HandleTurretMovement(Vector2 pointerPos)
    {

    }


}
