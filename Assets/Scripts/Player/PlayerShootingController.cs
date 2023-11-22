using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Utils;

namespace Game.Player
{
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Shooter Specs")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform frontFirePoint;
        [SerializeField] private List<Transform> sideFirePoints;
        [SerializeField] private float frontCooldownTime;
        [SerializeField] private float sideCooldownTime;
        
        private ObjectPooler objectPooler;
        
        private float frontCooldownTimer = 0f;
        private float sideCooldownTimer = 0f;
        private bool canShootFront = true;
        private bool canShootSide = true;
        
        private KeyCode frontShootKey = KeyCode.K;
        private KeyCode sideShootKey = KeyCode.L;
        
        
        private void Start()
        {
            objectPooler = new ObjectPooler(projectilePrefab, 10);
        }
        
        public void Update()
        {
            HandleInput();
            
            if (!canShootFront)
            {
                frontCooldownTimer += Time.deltaTime;

                if (frontCooldownTimer >= frontCooldownTime)
                {
                    frontCooldownTimer = 0f;
                    canShootFront = true;
                }
            }

            if (!canShootSide)
            {
                sideCooldownTimer += Time.deltaTime;

                if (sideCooldownTimer >= sideCooldownTime)
                {
                    sideCooldownTimer = 0f;
                    canShootSide = true;
                }
            }
        }
        
        private void HandleInput()
        {
            if (Input.GetKeyDown(frontShootKey))
            {
                ShootFront();
            }
            else if (Input.GetKeyDown(sideShootKey))
            {
                ShootSide();
            }
        }

        private void ShootFront()
        {
            if (canShootFront)
            {
                SpawnProjectile(frontFirePoint.position, frontFirePoint.rotation);
                canShootFront = false;
            }
        }

        private void ShootSide()
        {
            if (canShootSide)
            {
                foreach (Transform sideFirePoint in sideFirePoints)
                {
                    SpawnProjectile(sideFirePoint.position, sideFirePoint.rotation);
                }

                canShootSide = false;
            }
        }
        
        private void SpawnProjectile(Vector2 position, Quaternion rotation)
        {
            GameObject projectile = objectPooler.GetPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.SetActive(true);
            }
        }
    }
}