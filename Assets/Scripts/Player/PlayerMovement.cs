using UnityEngine;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Boat Specs")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float drag = 0.1f;
        [SerializeField] private float rotationSpeed = 180f;
        
        private Rigidbody2D _rigidbody2Drb;
        private float currentSpeed;
        private float currentAngle;

        void Start()
        {
            _rigidbody2Drb = GetComponent<Rigidbody2D>();
            _rigidbody2Drb.drag = drag;
        }

        void FixedUpdate()
        {
            float rotation = -Input.GetAxis("Horizontal");
            float movement = Input.GetAxis("Vertical");
            
            UpdateRotation(rotation);
            UpdateSpeed(movement);
            
            Vector2 force = transform.up * (currentSpeed * acceleration);
            _rigidbody2Drb.AddForce(force);
        }

        void UpdateRotation(float rotation)
        {
            currentAngle += rotation * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        void UpdateSpeed(float movement)
        {
            currentSpeed += movement * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
    }
}

