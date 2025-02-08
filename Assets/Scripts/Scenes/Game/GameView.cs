using UnityEngine;

namespace Scenes.Game
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnTransform;
        [SerializeField] private Transform _chomperSpawnTransform;

        public Vector3 GetPlayerSpawnPosition() => _playerSpawnTransform.position;
        public Vector3 GetChomperSpawnPosition() => _chomperSpawnTransform.position;
    }
}