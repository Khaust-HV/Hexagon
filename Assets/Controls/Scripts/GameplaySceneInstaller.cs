using UnityEngine;
using Zenject;

public sealed class GameplaySceneInstaller : MonoInstaller {
    [Header("Managers settings")]
    [SerializeField] private GameObject _playerManagerPrefab;
    [SerializeField] private GameObject _inputManagerPrefab;
    [Header("Other settings")]
    [SerializeField] private GameObject _cameraControlPrefab;
    [SerializeField] private GameObject _levelControlPrefab;

    public override void InstallBindings() {
        #region Managers
            Container.BindInterfacesAndSelfTo<PlayerManager>()
            .FromComponentInNewPrefab(_playerManagerPrefab)
            .AsSingle()
            .NonLazy();

            Container.BindInterfacesAndSelfTo<InputManager>()
            .FromComponentInNewPrefab(_inputManagerPrefab)
            .AsSingle()
            .NonLazy();
        #endregion

        #region Other Dependencies
            Container.BindInterfacesAndSelfTo<CameraControl>()
            .FromComponentInNewPrefab(_cameraControlPrefab)
            .AsSingle()
            .NonLazy();

            Container.BindInterfacesAndSelfTo<LevelControl>()
            .FromComponentInNewPrefab(_levelControlPrefab)
            .AsSingle()
            .NonLazy();
        #endregion
    }
}