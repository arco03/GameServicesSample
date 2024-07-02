using UnityEngine;

namespace PlayerMovement
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(TopDownMovementController))]
    [RequireComponent(typeof(TopDownCameraController))]
    [RequireComponent(typeof(TopDownAnimationController))]
    public class TopDownController : MonoBehaviour
    {
        // General References
        public Camera playerCamera;
        public bool cameraEnabled = true;
        public bool initCameraOnSpawn = true;
        public string cameraName = "Main Camera";

        // Dependencies
        private TopDownMovementController _movementController;
        private TopDownCameraController _cameraController;
        private TopDownAnimationController _animationController;
        private Animator _anim;

        private void Awake()
        {
            // Initialize components
            _movementController = GetComponent<TopDownMovementController>();
            _cameraController = GetComponent<TopDownCameraController>();
            _animationController = GetComponent<TopDownAnimationController>();

            _anim = GetComponent<Animator>();

            _movementController.SetAnimator(_anim);
            _animationController.SetAnimator(_anim);

            if (cameraEnabled)
            {
                InitCamera();
            }
        }

        private void InitCamera()
        {
            if (!initCameraOnSpawn)
            {
                if (playerCamera) return;
            }

            Camera cam = GameObject.Find(cameraName).GetComponent<Camera>();
            if (cam == null)
            {
                Debug.LogError(
                    "TOPDOWN_CONTROLLER: NO CAMERA FOUND! MAKE SURE TO EITHER DRAG AND DROP ONE, OR ENABLE INIT CAMERA AND TYPE A VALID CAMERA NAME");
            }
            else
            {
                playerCamera = cam;
            }
            
            if (playerCamera == null) return;
            playerCamera.transform.eulerAngles = _cameraController.cameraRotationOffset;
        }
    }
}