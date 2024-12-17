using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class GameplaySceneInstaller : MonoInstaller {
    [Header("Configuration")]
    [SerializeField] private LevelConfigs _levelConfigs;
    [SerializeField] private HexagonConfigs _hexagonConfigs;
    [Header("DI prefabs")]
    [SerializeField] private GameObject _cameraControllerPrefab;

    public override void InstallBindings() {
        Application.targetFrameRate = 120;

        ConfigsBind();

        ManagersInit();

        OtherDependencesInit();
    }

    private void ConfigsBind() {
        Container.Bind<LevelConfigs>().FromInstance(_levelConfigs).AsSingle().NonLazy();
        Container.Bind<HexagonConfigs>().FromInstance(_hexagonConfigs).AsSingle().NonLazy();
    }

    private void ManagersInit() {
        Container.BindInterfacesTo<PlayerManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<InputManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelManager>().AsSingle().NonLazy();
    }

    private void OtherDependencesInit() {
        Container.BindInterfacesTo<LevelObjectPool>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelObjectCreator>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelObjectBuilder>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelObjectFactory>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<CameraController>().FromComponentInNewPrefab(_cameraControllerPrefab).AsSingle().NonLazy();
    }
}