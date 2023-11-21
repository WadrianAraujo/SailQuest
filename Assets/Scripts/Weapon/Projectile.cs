using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f; 
    
    private float timer = 0f;
    
    private void Update()
    {
        Move();
        CheckLifetime();
    }
    
    private void CheckLifetime()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the bullet has exceeded its lifetime
        if (timer >= lifetime)
        {
            DeactivateProjectile();
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    
    private void DeactivateProjectile()
    {
        gameObject.SetActive(false);
        timer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            DeactivateProjectile();
        }
    }
}