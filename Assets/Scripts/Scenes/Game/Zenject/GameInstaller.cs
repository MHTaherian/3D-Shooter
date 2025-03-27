using Scenes.Game.Framework;
using Scenes.Game.Framework.Creature;
using Scenes.Game.Framework.Creature.Containers;
using Scenes.Game.Framework.Creature.Enemy.Chomper;
using Scenes.Game.Framework.Creature.Managers;
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
        [SerializeField] private ChomperComponent _chomperComponentPrefab;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private InGameUi _inGameUi;
        [SerializeField] private GameView _gameView;
        [SerializeField] private HealthBar _healthBarPrefab;

        [Header("Containers")] [SerializeField]
        private WeaponsContainer _weaponsContainer;

        [SerializeField] private CreaturesDataContainer _creaturesDataContainer;

        public override void InstallBindings()
        {
            Container.Bind<GameViewPresenter>().AsSingle().NonLazy();
            Container.Bind<CameraController>().FromInstance(_cameraController).WhenInjectedInto<GameViewPresenter>();
            Container.BindInstance(_inGameUi).WhenInjectedInto<GameViewPresenter>();

            Container.Bind<ITransformGetter>().To<PlayerComponent>().FromInstance(_playerComponentPrefab)
                .WhenInjectedInto<CameraController>();
            Container.Bind<WeaponsContainer>().FromInstance(_weaponsContainer).WhenInjectedInto<Weapon.WeaponFactory>();
            Container.Bind<Weapon.WeaponFactory>().WhenInjectedInto<InventoryManager>();

            Container.BindInstance(_gameView).WhenInjectedInto<GameViewPresenter>();
            
            InstallCreatureBindings();

            InstallHealthBindings();
        }

        private void InstallCreatureBindings()
        {
            Container.Bind<Creature.ICreatureFactory>().To<Creature.Factory>().WhenInjectedInto<CreaturesManager>();
            Container.Bind<CreaturesManager>().WhenInjectedInto<GameViewPresenter>();
            Container.Bind<InventoryManager>().WhenInjectedInto<Creature.ICreatureFactory>();
            Container.Bind<CreaturesDataContainer>().FromInstance(_creaturesDataContainer).AsSingle();
            
            InstallPlayerBindings();

            InstallChomperBindings();
        }

        private void InstallPlayerBindings()
        {
            Container.Bind<PlayerComponent>().FromInstance(_playerComponentPrefab).WhenInjectedInto<PlayerComponent.Factory>();
            Container.Bind<PlayerComponent.Factory>().WhenInjectedInto<Creature.ICreatureFactory>();
        }

        private void InstallChomperBindings()
        {
            Container.Bind<ChomperComponent>().FromInstance(_chomperComponentPrefab).WhenInjectedInto<ChomperComponent.Factory>();
            Container.Bind<ChomperComponent.Factory>().WhenInjectedInto<Creature.ICreatureFactory>();
        }

        private void InstallHealthBindings()
        {
            Container.Bind<CreatureHealthManager.Factory>().WhenInjectedInto<Creature.ICreatureFactory>();
            Container.Bind<IHealthView.Factory>().WhenInjectedInto<CreatureHealthManager.Factory>();
            Container.Bind<ITransformGetter>().To<InGameUi>().FromInstance(_inGameUi).WhenInjectedInto<IHealthView.Factory>();
            Container.Bind<HealthBar>().FromInstance(_healthBarPrefab).WhenInjectedInto<IHealthView.Factory>();
        }
    }
}