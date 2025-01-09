using LevelObjectsPool;
using Hexagon;
using System.Collections.Generic;
using GameConfigs;
using Zenject;

public sealed class LevelObjectCreator : IBuildingsCreate, IUnitsCreate, IProjectilesCreate {
    private LevelConfigs _levelConfigs;
    private HexagonObjectConfigs _hexagonObjectConfigs;

    #region DI
        private ILevelObjectFactory _iLevelObjectFactory;
        private IBuildingsPool _iBuildingsPool;
        private IUnitsPool _iUnitsPool;
        private IProjectilesPool _iProjectilesPool;
        private IStorageTransformPool _iStorageTransformPool;
    #endregion

    [Inject]
    private void Construct (
        IBuildingsPool iBuildingsPool, 
        IUnitsPool iUnitsPool, 
        IProjectilesPool iProjectilesPool, 
        ILevelObjectFactory iLevelObjectFactory,
        IStorageTransformPool iStorageTransformPool, 
        LevelConfigs levelConfigs,
        HexagonObjectConfigs hexagonObjectConfigs
        ) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;
        _iProjectilesPool = iProjectilesPool;
        _iLevelObjectFactory = iLevelObjectFactory;
        _iStorageTransformPool = iStorageTransformPool;

        // Set configurations
        _levelConfigs = levelConfigs;
        _hexagonObjectConfigs = hexagonObjectConfigs;
    }

    public IHexagonControl CreateSomeHexagonControllers() {
        List<IHexagonControl> hexagonControllersList = _iLevelObjectFactory.CreateObjects<IHexagonControl> (
            prefab: _levelConfigs.HexagonControllerPrefab, 
            number: _levelConfigs.NumberObjectsCreatedInCaseOfShortage, 
            trParentObject: _iStorageTransformPool.GetHexagonTransformPool(),
            size: _levelConfigs.HexagonSize
        );

        _iBuildingsPool.AddNewHexagonControllersInPool(hexagonControllersList);

        return hexagonControllersList[0];
    }

    public IHexagonObjectElement CreateSomeHexagonObjectElements<T>(T type) where T : System.Enum {
        var prefabs = _hexagonObjectConfigs.GetHexagonObjectPrefabs(type);

        List<IHexagonObjectElement> hexagonObjectsList;

        int numberObjects = type switch {
            CoreHexagonObjectsType.MainCore => 1,
            _ => _levelConfigs.NumberObjectsCreatedInCaseOfShortage
        };

        if (prefabs.Length == 1) {
            hexagonObjectsList = _iLevelObjectFactory.CreateObjects<IHexagonObjectElement> (
                prefab: prefabs[0], 
                number: numberObjects, 
                trParentObject: _iStorageTransformPool.GetHexagonObjectTransformPool(),
                size: _levelConfigs.HexagonObjectSize
            );
        } else {
            hexagonObjectsList = _iLevelObjectFactory.CreateRandomObjects<IHexagonObjectElement> (
                prefabs: prefabs, 
                number: numberObjects, 
                trParentObject: _iStorageTransformPool.GetHexagonObjectTransformPool(),
                size: _levelConfigs.HexagonObjectSize
            );
        }

        _iBuildingsPool.AddNewHexagonObjectElementsInPool(type, hexagonObjectsList);

        return hexagonObjectsList[0];
    }

    public IHexagonObjectControl CreateSomeHexagonObjectControllers() {
        List<IHexagonObjectControl> hexagonObjectControllersList = _iLevelObjectFactory.CreateObjects<IHexagonObjectControl> (
            prefab: _levelConfigs.HexagonObjectsControllerPrefab, 
            number: _levelConfigs.NumberObjectsCreatedInCaseOfShortage, 
            trParentObject: _iStorageTransformPool.GetHexagonObjectTransformPool(),
            size: _levelConfigs.HexagonObjectSize
        );

        _iBuildingsPool.AddNewHexagonObjectControllersInPool(hexagonObjectControllersList);

        return hexagonObjectControllersList[0];
    }
}

public interface IBuildingsCreate {
    public IHexagonControl CreateSomeHexagonControllers();
    public IHexagonObjectElement CreateSomeHexagonObjectElements<T>(T type) where T : System.Enum;
    public IHexagonObjectControl CreateSomeHexagonObjectControllers();
}

public interface IUnitsCreate {
    
}

public interface IProjectilesCreate {
    
}