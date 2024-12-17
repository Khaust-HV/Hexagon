using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class GameplaySceneInstaller : MonoInstaller {
    [Header("Level configs")]
    [SerializeField] private LevelConfigs _levelConfigs;
    [Header("DI prefabs")]
    [SerializeField] private GameObject _cameraControllerPrefab;

    public override void InstallBindings() {
        Application.targetFrameRate = 120;

        LevelConfigsBind();

        ManagersInit();

        OtherDependencesInit();
    }

    private void LevelConfigsBind() {
        Container.Bind<LevelConfigs>().FromInstance(_levelConfigs).AsSingle().NonLazy();
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
        
        Container.BindInterfacesAndSelfTo<CameraController>().FromComponentInNewPrefab(_cameraControllerPrefab).AsSingle().NonLazy();
    }
}