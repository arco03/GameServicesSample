using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
    [RequireComponent(typeof(CharacterController))]
    public class TopDownMovementController : MonoBehaviour
    {
        // INPUT
        public InputActionAsset inputActionAsset;
        private InputAction _moveAction;
        private InputAction _jumpAction;

        // NAVIGATION
        public bool movementEnabled = true;
        public float jumpHeight = 4;
        public float gravity = 10;
        public float moveSpeed = 5;

        private CharacterController _characterController;
        private Animator _anim;
        private Vector3 _displacement;
        private float _verticalSpeed;
        private bool _isJumping;
        private float _actualMoveSpeed;

        // ANIMATOR
        static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
        static readonly int VerticalHash = Animator.StringToHash("Vertical");
        static readonly int FallingHash = Animator.StringToHash("Falling");
        static readonly int JumpHash = Animator.StringToHash("Jump");
        public float animatorSmoothTime = 0.15f;

        // OTHER
        private Vector2? _targetPosition;
        private Camera _mainCamera;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _actualMoveSpeed = moveSpeed;
            InitializeInputSystem();
            
            _mainCamera = Camera.main;
        }

        private void InitializeInputSystem()
        {
            _moveAction = inputActionAsset.FindAction("Move");
            _jumpAction = inputActionAsset.FindAction("Jump");
            _moveAction.Enable();
            _jumpAction.Enable();
        }

        private void Update()
        {
            if (movementEnabled) HandleMovement();
        }

        public void SetAnimator(Animator animator)
        {
            _anim = animator;
        }

        private void HandleMovement()
        {
            _displacement = Vector3.zero;

            if (_characterController.isGrounded)
            {
                if (_jumpAction.triggered) HandleJump();
                else HandleGroundedMovement();
            }
            else
            {
                HandleAirborneMovement();
            }

            RotateCharacterToCursor();
            ApplyGravity();
            _characterController.Move(_displacement);
        }

        private void HandleJump()
        {
            _targetPosition = null;
            _verticalSpeed = jumpHeight;
            _isJumping = true;
            _anim?.SetTrigger(JumpHash);
        }

        private void HandleGroundedMovement()
        {
            _verticalSpeed = 0f;
            _isJumping = false;

            _anim?.SetBool(FallingHash, false);
            
            Vector2 input = _moveAction.ReadValue<Vector2>();
            input.Normalize();

            // Transform input to be relative to the camera's direction
            Vector3 forward = _mainCamera.transform.forward;
            Vector3 right = _mainCamera.transform.right;

            forward.y = 0; // Ensure we're only moving in XZ plane
            right.y = 0; // Ensure we're only moving in XZ plane

            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
            moveDirection *= (_actualMoveSpeed * Time.deltaTime);

            if (_targetPosition is { } value)
            {
                Vector2 displacementToTarget = value - new Vector2(transform.position.x, transform.position.z);
                Vector3 clampedMoveDirection = Vector2.ClampMagnitude(displacementToTarget, _actualMoveSpeed * Time.deltaTime);
                moveDirection = new Vector3(clampedMoveDirection.x, 0, clampedMoveDirection.y);
            }

            if (moveDirection != Vector3.zero)
            {
                _displacement.x = moveDirection.x;
                _displacement.z = moveDirection.z;
                SetAnimatorParameters(new Vector2(moveDirection.x, moveDirection.z), true);
            }
            else
            {
                SetAnimatorParameters(Vector2.zero, false);
            }
        }

        private void HandleAirborneMovement()
        {
            _targetPosition = null;
            _isJumping = true;
            float groundDistance = GetGroundDistance();

            _anim?.SetBool(FallingHash, groundDistance > 0.2f);
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && !_isJumping)
            {
                _displacement.y = -gravity * Time.deltaTime;
            }
            else
            {
                _displacement.y = _verticalSpeed * Time.deltaTime;
            }

            _verticalSpeed -= gravity * Time.deltaTime;
        }

        private void RotateCharacterToCursor()
        {
            Vector3 point = GetPoint();
            Vector3 direction = point - transform.position;
            direction.y = 0; // Ensure we're only rotating on the Y axis

            if (direction.sqrMagnitude > 0.1f) // Check for a valid direction to avoid jittering
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        private void SetAnimatorParameters(Vector2 input, bool isMoving)
        {
            if (isMoving)
            {
                float horizontal = input.x;
                float vertical = input.y;
                _anim?.SetFloat(HorizontalHash, horizontal, animatorSmoothTime, Time.deltaTime);
                _anim?.SetFloat(VerticalHash, vertical, animatorSmoothTime, Time.deltaTime);
            }
            else
            {
                _anim?.SetFloat(HorizontalHash, 0, animatorSmoothTime, Time.deltaTime);
                _anim?.SetFloat(VerticalHash, 0, animatorSmoothTime, Time.deltaTime);
            }
        }

        private float GetGroundDistance()
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out var hit))
            {
                return hit.distance;
            }
            return 0;
        }

        private Vector3 GetPoint()
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            var mousePosition = Mouse.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(mousePosition);
            return playerPlane.Raycast(ray, out var hitDist) ? ray.GetPoint(hitDist) : Vector3.zero;
        }
    }
}