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
    private TeamAssignment _firerTeam;

    private void Awake()
    {
        _tankColliders = GetComponentsInParent<Collider2D>();

        // Get team assignment from parent tank
        _firerTeam = GetComponentInParent<TeamAssignment>();
        if (_firerTeam == null)
        {
            Debug.LogWarning($"[Turret] Tank parent of turret '{gameObject.name}' has no TeamAssignment!");
        }
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
                
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.Init();
                
                // Set which team fired this bullet
                if (_firerTeam != null)
                {
                    bulletComponent.SetFirer(_firerTeam);
                }
                
                foreach (var collider in _tankColliders)
                {
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), collider);
                }
            }
        }
    }
}
