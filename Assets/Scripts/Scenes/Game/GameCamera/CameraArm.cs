using System;
using UnityEngine;

namespace Scenes.Game.GameCamera
{
    [ExecuteAlways]
    public class CameraArm : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _armLength;

        private void Update()
        {
            _cameraTransform.position = transform.position - _cameraTransform.forward * _armLength;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_cameraTransform.position, transform.position);
        }
    }
}
