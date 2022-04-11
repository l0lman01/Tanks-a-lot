using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<Transform> turretBarrels;
    public GameObject bulletPrefab;
    public float reload = 1f;

    private bool _canShoot = true;
    private Collider2D[] _tankColliders;
    private float currentDelay = 0f;

    private void Awake()
    {
        _tankColliders = GetComponentsInParent<Collider2D>();
    }

    private void Update()
    {
        if (_canShoot == false)
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                _canShoot = true;
            }
        }
    }


    public void Shoot()
    {
        Debug.Log("Shot");
        if (_canShoot)
        {
            _canShoot = false;
            currentDelay = reload;

            foreach (var barrel in turretBarrels)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = barrel.position;
                bullet.transform.localRotation = barrel.rotation;
                bullet.GetComponent<Bullet>().Init();
                foreach (var collider in _tankColliders)
                {
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), collider);
                }
            }
        }
    }
}
