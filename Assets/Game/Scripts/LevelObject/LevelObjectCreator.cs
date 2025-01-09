using LevelObjectsPool;
using Hexagon;
using System.Collections.Generic;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class LevelObjectCreator : IBuildingsCreate, IUnitsCreate, IProjectilesCreate {
    private LevelConfigs _levelConfigs;

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
        LevelConfigs levelConfigs) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;
        _iProjectilesPool = iProjectilesPool;
        _iLevelObjectFactory = iLevelObjectFactory;
        _iStorageTransformPool = iStorageTransformPool;

        // Set configurations
        _levelConfigs = levelConfigs;
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

    public IHexagonObjectElement CreateSomeHexagonObjects<T>(T type) where T : System.Enum {
        var prefabs = GetHexagonObjectPrefabs(type);

        List<IHexagonObjectElement> hexagonObjectsList = null;

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

        _iBuildingsPool.AddNewHexagonObjectsInPool(type, hexagonObjectsList);

        return hexagonObjectsList[0];
    }

    private GameObject[] GetHexagonObjectPrefabs<T>(T type) where T : System.Enum {
        // switch (type) {
        //     case DecorationHexagonObjectsType decorationType:
        //         switch (decorationType) { 

        //         }
        //     // break;

        //     case MineHexagonObjectsType mineType:
        //         switch (mineType) {

        //         }

        //     // break;

        //     case HeapHexagonObjectsType heapType:
        //         switch (heapType) {

        //         }
        //     // break;

        //     case CoreHexagonObjectsType coreType:
        //         switch (coreType) {

        //         }
        //     // break;

        //     case BuildebleFieldHexagonObjectsType buildableFieldType:
        //         switch (buildableFieldType) {

        //         }
        //     // break;

        //     case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
        //         switch (unBuildableFieldType) {

        //         }
        //     // break;

        //     case LiquidHexagonObjectsType liquidType:
        //         switch (liquidType) {

        //         }
        //     // break;
        // }

        return new GameObject[0]; // FIX IT !
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
    public IHexagonObjectElement CreateSomeHexagonObjects<T>(T type) where T : System.Enum;
    public IHexagonObjectControl CreateSomeHexagonObjectControllers();
}

public interface IUnitsCreate {
    
}

public interface IProjectilesCreate {
    
}