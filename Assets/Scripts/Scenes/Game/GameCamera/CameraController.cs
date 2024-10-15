using UnityEngine;

namespace Scenes.Game.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed;
        private ITransformGetter _transformGetter;
        private Transform _transformToFollow;

        public void SetTransformToFollow(ITransformGetter transformGetter)
        {
            _transformToFollow = transformGetter.GetTransform();
        }

        private void LateUpdate()
        {
            transform.position = _transformToFollow.position;
        }

        public void RotateFrameRateIndependently(float amount)
        {
            transform.Rotate(Vector3.up, amount * Time.deltaTime * _rotateSpeed);
        }
    }
}
