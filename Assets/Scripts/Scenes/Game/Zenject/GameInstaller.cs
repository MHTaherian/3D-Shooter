using Scenes.Game.GameCamera;
using Scenes.Game.Player;
using Scenes.Game.UI.InGameUI;
using Scenes.Game.UI.InGameUI.InputManager;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Scenes.Game.Zenject
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private InGameUi _inGameUi;

        public override void InstallBindings()
        {
            Container.Bind<GameViewPresenter>().FromInstance(new GameViewPresenter(_playerController, _cameraController, _inGameUi));
            Container.Bind<ITransformGetter>().To<PlayerController>().FromInstance(_playerController)
                .WhenInjectedInto<CameraController>();
        }
    }
}