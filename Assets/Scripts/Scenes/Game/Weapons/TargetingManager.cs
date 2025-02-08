using UnityEngine;

namespace Scenes.Game.Weapons
{
    public class TargetingManager
    {
        private readonly Transform _muzzleTransform;
        private readonly float _maxRange;
        private readonly LayerMask _targetMask;

        public TargetingManager(Transform muzzleTransform, float maxRange, LayerMask targetMask)
        {
            _muzzleTransform = muzzleTransform;
            _maxRange = maxRange;
            _targetMask = targetMask;
        }

        public bool TryGetTargetObject(out GameObject target)
        {
            target = null;
            var aimDir = GetAimDir();
            var targetFound =
                Physics.Raycast(_muzzleTransform.position, aimDir, out var hitInfo, _maxRange, _targetMask);
            if (targetFound)
                target = hitInfo.collider.gameObject;

            return targetFound;
        }

        public Vector3 GetAimDir()
        {
            var muzzlePosition = _muzzleTransform.forward;
            var aimDir = new Vector3(muzzlePosition.x, 0, muzzlePosition.z).normalized;
            return aimDir;
        }
    }
}