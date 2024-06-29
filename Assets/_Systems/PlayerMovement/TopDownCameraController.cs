using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
    public class TopDownCameraController : MonoBehaviour
    {
        // INPUT
        public InputActionAsset inputActionAsset;
        private InputAction _cameraZoomAction;
        private InputAction _dragAction;

        // CAMERA
        public Camera playerCamera;
        public Vector3 cameraPositionOffset = new Vector3(0, 10, 0);
        public Vector3 cameraRotationOffset = new Vector3(45, 0, 0);
        public float cameraDampTime = 0.1f;
        public float offsetDampTime = 0.25f;
        public float maxOffset = 0.3f;
        public bool isDraggable = true;
        public float minCameraHeight = 3,
            maxCameraHeight = 15,
            minCameraVertical = 0.5f,
            maxCameraVertical = 0.5f,
            cameraZoomSpeed = 15,
            cameraZoomPower = 5;
        
        private float _currentCameraHeight, _cameraHeightTarget, _currentCameraVertical, _cameraVerticalTarget;
        private Vector3 _forward;
        private Vector3 _cameraVelocity;
        private Vector3 _dampedCameraPosition;
        private Vector2 _dampedOffsetVector;
        private Vector2 _currentDistanceVectorVelocity;
        private float _dragAngle;
        private bool _isDragging;

        private void Awake()
        {
            InitializeInputSystem();
        }

        private void InitializeInputSystem()
        {
            _cameraZoomAction = inputActionAsset.FindAction("CameraZoom");
            _dragAction = inputActionAsset.FindAction("CameraDrag");
            _cameraZoomAction.Enable();
            _dragAction.Enable();
        }

        private void LateUpdate()
        {
            if (isDraggable) HandleDrag();
            if (!_isDragging) HandleCameraPosition();
            HandleCameraZoom();
        }

        private void HandleDrag()
        {
            if (_dragAction.triggered)
            {
                StartDragging();
            }
            else if (_dragAction.ReadValue<float>() > 0)
            {
                DragCamera();
            }

            if (_dragAction.WasReleasedThisFrame())
            {
                _isDragging = false;
            }
        }

        private void StartDragging()
        {
            Vector3 point = GetPoint();
            if (Vector3.Distance(point, transform.position) > 3f)
            {
                Vector3 targetCameraPosition = transform.position - _forward * _currentCameraHeight;

                _isDragging = true;

                _dragAngle = Mathf.Atan2(transform.position.z - point.z, point.x - transform.position.x) *
                             Mathf.Rad2Deg + 90;

                _dampedOffsetVector = new Vector2(
                    _dampedCameraPosition.x - targetCameraPosition.x + _dampedOffsetVector.x,
                    _dampedCameraPosition.z - targetCameraPosition.z + _dampedOffsetVector.y);
            }
        }

        private void DragCamera()
        {
            Vector3 point = GetPoint();
            if (_isDragging && Vector3.Distance(point, transform.position) > 0.25f)
            {
                float dragAngle = Mathf.Atan2(transform.position.z - point.z, point.x - transform.position.x) *
                                  Mathf.Rad2Deg + 90;

                float deltaAngle = _dragAngle - dragAngle;
                Vector3 eulerAngles = playerCamera.transform.rotation.eulerAngles;
                float targetAngle = eulerAngles.y + deltaAngle;
                targetAngle = Mathf.MoveTowardsAngle(eulerAngles.y, targetAngle, Time.deltaTime * 360f);

                playerCamera.transform.rotation =
                    Quaternion.Euler(eulerAngles.x, targetAngle, eulerAngles.z);

                _forward = playerCamera.transform.forward;
            }
        }

        private void HandleCameraPosition()
        {
            Vector3 point = GetPoint();
            Vector2 distanceToPlayer = new Vector2(point.x, point.z) -
                                       new Vector2(transform.position.x, transform.position.z);

            distanceToPlayer = Vector2.ClampMagnitude(distanceToPlayer, maxOffset);
            _dampedOffsetVector = Vector2.SmoothDamp(_dampedOffsetVector, distanceToPlayer,
                ref _currentDistanceVectorVelocity, offsetDampTime);

            _dampedCameraPosition = Vector3.SmoothDamp(_dampedCameraPosition,
                new Vector3(transform.position.x + cameraPositionOffset.x, transform.position.y, transform.position.z + _currentCameraVertical) - _forward * _currentCameraHeight, ref _cameraVelocity, cameraDampTime);

            playerCamera.transform.position = _dampedCameraPosition + new Vector3(_dampedOffsetVector.x, 0, _dampedOffsetVector.y);
        }

        private void HandleCameraZoom()
        {
            float scrollDelta = _cameraZoomAction.ReadValue<Vector2>().y;
            if (scrollDelta == 0) return;

            float heightDifference = scrollDelta < 0f ? cameraZoomPower : -cameraZoomPower;
            _cameraHeightTarget = _currentCameraHeight + heightDifference;
            _cameraVerticalTarget = _currentCameraVertical + heightDifference;
            _cameraHeightTarget = Mathf.Clamp(_cameraHeightTarget, minCameraHeight, maxCameraHeight);
            _cameraVerticalTarget = Mathf.Clamp(_cameraVerticalTarget, minCameraVertical, maxCameraVertical);

            LerpCameraHeight();
        }

        private void LerpCameraHeight()
        {
            _currentCameraHeight = Mathf.Lerp(_currentCameraHeight, _cameraHeightTarget, Time.deltaTime * cameraZoomSpeed);
            _currentCameraVertical = Mathf.Lerp(_currentCameraVertical, _cameraVerticalTarget, Time.deltaTime * cameraZoomSpeed);
        }

        private Vector3 GetPoint()
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            var mousePosition = Mouse.current.position.ReadValue();
            var ray = playerCamera.ScreenPointToRay(mousePosition);
            return playerPlane.Raycast(ray, out var hitDist) ? ray.GetPoint(hitDist) : Vector3.zero;
        }
    }
}