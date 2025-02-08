using Scenes.Game.Framework;
using Scenes.Game.Framework.Creature;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Player;
using Scenes.Game.GameCamera;
using Scenes.Game.Inventory;
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
        [SerializeField] private PlayerComponent _playerComponentPrefab;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private InGameUi _inGameUi;
        [SerializeField] private GameView _gameView;

        [Header("Containers")] [SerializeField]
        private WeaponsContainer _weaponsContainer;

        [SerializeField] private CreaturesDataContainer _creaturesDataContainer;

        public override void InstallBindings()
        {
            Container.Bind<GameViewPresenter>().AsSingle().NonLazy();
            Container.Bind<PlayerComponent>().FromInstance(_playerComponentPrefab).WhenInjectedInto<PlayerComponent.Factory>();
            Container.Bind<PlayerComponent.Factory>().WhenInjectedInto<Creature.ICreatureFactory>();
            Container.Bind<CameraController>().FromInstance(_cameraController).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<Creature.ICreatureFactory>().To<Creature.Factory>().WhenInjectedInto<GameViewPresenter>();
            Container.BindInstance(_inGameUi).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<ITransformGetter>().To<PlayerComponent>().FromInstance(_playerComponentPrefab)
                .WhenInjectedInto<CameraController>();
            Container.Bind<WeaponsContainer>().FromInstance(_weaponsContainer).WhenInjectedInto<Weapon.WeaponFactory>();
            Container.Bind<Weapon.WeaponFactory>().WhenInjectedInto<InventoryManager>();
            Container.Bind<InventoryManager>().WhenInjectedInto<Creature.ICreatureFactory>();
            Container.BindInterfacesTo<CreaturesDataContainer>().AsSingle();
            Container.Bind<ICreaturesHealthManagerCreator>().To<CreaturesDataContainer>()
                .FromInstance(_creaturesDataContainer).WhenInjectedInto<HealthSystem>();
            Container.BindInstance(_gameView).WhenInjectedInto<GameViewPresenter>();
            Container.Bind<HealthSystem>().WhenInjectedInto<Creature.ICreatureFactory>();
        }
    }
}