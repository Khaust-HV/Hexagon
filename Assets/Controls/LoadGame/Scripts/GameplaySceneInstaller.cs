using LevelObject;
using GameConfigs;
using UnityEngine;
using Zenject;
using CameraControl;
using Managers;

public sealed class GameplaySceneInstaller : MonoInstaller {
    [Header("Configurations")]
    [SerializeField] private LevelConfigs _levelConfigs;
    [SerializeField] private HexagonConfigs _hexagonConfigs;
    [SerializeField] private CameraConfigs _cameraConfigs;
    [SerializeField] private VisualEffectsConfigs _visualEffectsConfigs;
    [SerializeField] private HexagonObjectConfigs _hexagonObjectConfigs;
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
        Container.Bind<CameraConfigs>().FromInstance(_cameraConfigs).AsSingle().NonLazy();
        Container.Bind<VisualEffectsConfigs>().FromInstance(_visualEffectsConfigs).AsSingle().NonLazy();
        Container.Bind<HexagonObjectConfigs>().FromInstance(_hexagonObjectConfigs).AsSingle().NonLazy();
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