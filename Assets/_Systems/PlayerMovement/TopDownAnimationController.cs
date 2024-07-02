using UnityEngine;

namespace PlayerMovement
{
    public class TopDownAnimationController : MonoBehaviour
    {
        // IK
        public float bodyWeightIK = 0.5f;
        public float headWeightIK = 1.0f;
        public float dampSmoothTimeIK = 0.4f;
        private Animator _anim;

        // OTHER
        private float _deltaAngle;
        private float _deltaAngleVelocity;
        private float _targetAngle;
        private float _rotationAngle;
        private float _lookAngle;

        public void SetAnimator(Animator animator)
        {
            _anim = animator;
        }

        private void Awake()
        {
            InitializeAnimator();
        }

        private void InitializeAnimator()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            _anim.SetLookAtWeight(1, bodyWeightIK, headWeightIK, 1.0f);

            float targetDeltaAngle = Mathf.Clamp(Mathf.DeltaAngle(_rotationAngle, _lookAngle), -60, 60);

            _deltaAngle = Mathf.SmoothDampAngle(_deltaAngle, targetDeltaAngle, ref _deltaAngleVelocity, dampSmoothTimeIK);

            _targetAngle = (_rotationAngle + _deltaAngle - 90) * Mathf.Deg2Rad;

            Vector3 targetLookAt = transform.position + new Vector3(Mathf.Cos(_targetAngle) * 10, 0, Mathf.Sin(_targetAngle) * -10);

            Transform headTransform = _anim.GetBoneTransform(HumanBodyBones.Head);
            targetLookAt.y = headTransform.position.y;

            _anim.SetLookAtPosition(targetLookAt);
        }
    }
}