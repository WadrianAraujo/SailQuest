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

        private void Start()
        {
            InitializeComponents();
        }

        private void FixedUpdate()
        {
            HandleInput();
            ApplyForces();
        }

        private void InitializeComponents()
        {
            _rigidbody2Drb = GetComponent<Rigidbody2D>();
            _rigidbody2Drb.drag = drag;
        }

        private void HandleInput()
        {
            float rotation = -Input.GetAxis("Horizontal");
            float movement = Input.GetAxis("Vertical");

            UpdateRotation(rotation);
            UpdateSpeed(movement);
        }

        private void UpdateRotation(float rotationInput)
        {
            currentAngle += rotationInput * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        private void UpdateSpeed(float movementInput)
        {
            currentSpeed += movementInput * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            
            if (movementInput == 0f)
            {
                currentSpeed *= 1f - drag * Time.deltaTime;
            }
        }

        private void ApplyForces()
        {
            Vector2 force = transform.up * (currentSpeed * acceleration);
            _rigidbody2Drb.AddForce(force);
        }
    }
}