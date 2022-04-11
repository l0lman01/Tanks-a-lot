using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 10f;
    public int damage = 5;

    private Vector2 _startPosition;
    private float _travelDistance = 0f;
    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _travelDistance = Vector2.Distance(transform.position, _startPosition);
        if (_travelDistance >= maxDistance)
        {
            DisableObject();
        }
    }

    public void Init()
    {
        _startPosition = transform.position;
        _rb2d.velocity = transform.up * speed;
    }

    private void DisableObject()
    {
        _rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        DisableObject();
    }
}
