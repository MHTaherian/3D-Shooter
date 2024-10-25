using Scenes.Game.GameCamera;
using Scenes.Game.Inventory;
using Scenes.Game.Player;
using Scenes.Game.UI.InGameUI;
using Scenes.Game.UI.InGameUI.InputManager;
using Scenes.Game.Weapons;
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

        [Header("Containers")] [SerializeField]
        private WeaponsContainer _weaponsContainer;

        public override void InstallBindings()
        {
            Container.Bind<GameViewPresenter>().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromInstance(_playerController).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<CameraController>().FromInstance(_cameraController).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<InGameUi>().FromInstance(_inGameUi).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<ITransformGetter>().To<PlayerController>().FromInstance(_playerController)
                .WhenInjectedInto<CameraController>();
            Container.Bind<WeaponsContainer>().FromInstance(_weaponsContainer).WhenInjectedInto<Weapon.WeaponFactory>();
            Container.Bind<Weapon.WeaponFactory>().WhenInjectedInto<InventoryManager>();
            Container.Bind<InventoryManager>().WhenInjectedInto<PlayerController>();
        }
    }
}