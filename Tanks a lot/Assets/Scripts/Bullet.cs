using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 10f;
    public float damage = 5f;

    private Vector2 _startPosition;
    private float _travelDistance = 0f;
    private Rigidbody2D _rb2d;
    private TeamAssignment _firerTeam;
    private GameParameters _gameParameters;
    private bool _hasHit = false;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gameParameters = Resources.Load<GameParameters>("GameParameters");
        if (_gameParameters == null)
        {
            Debug.LogError("[Bullet] GameParameters not found in Resources!");
        }
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
        _hasHit = false;
    }

    public void SetFirer(TeamAssignment firer)
    {
        _firerTeam = firer;
    }

    private void DisableObject()
    {
        _rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't hit if we already hit something
        if (_hasHit)
            return;

        // Check if hit a tank
        TankHealth tankHealth = collision.GetComponent<TankHealth>();
        if (tankHealth != null && !tankHealth.IsDead)
        {
            _hasHit = true;

            // Calculate damage with falloff
            float finalDamage = damage;
            if (_gameParameters != null)
            {
                finalDamage = _gameParameters.TankShellDamage;

                // Apply falloff if we're beyond the falloff distance from start
                float distanceFromStart = Vector2.Distance(transform.position, _startPosition);
                if (distanceFromStart > _gameParameters.TankShellDamageFalloff)
                {
                    float excessDistance = distanceFromStart - _gameParameters.TankShellDamageFalloff;
                    float falloffFactor = Mathf.Max(0, 1 - (excessDistance / _gameParameters.TankShellDamageFalloff));
                    finalDamage *= falloffFactor;
                }
            }

            Debug.Log($"[Bullet] Hit tank '{collision.name}' for {finalDamage} damage!");
            tankHealth.TakeDamage(finalDamage, _firerTeam);
        }

        // Always disable on collision with anything
        DisableObject();
    }
}
