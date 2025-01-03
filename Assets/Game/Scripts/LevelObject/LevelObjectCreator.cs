using LevelObjectsPool;
using Hexagon;
using System.Collections.Generic;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class LevelObjectCreator : IBuildingsCreate, IUnitsCreate, IProjectilesCreate {
    #region Level Configs Settings
        private int _numberObjectsCreatedInCaseOfShortage;
        private GameObject _hexagonControllerPrefab;
        private float _hexagonSize;
        private GameObject _hexagonObjectsControllerPrefab;
        private float _hexagonObjectSize;
    #endregion

    #region DI
        private ILevelObjectFactory _iLevelObjectFactory;
        private IBuildingsPool _iBuildingsPool;
        private IUnitsPool _iUnitsPool;
        private IProjectilesPool _iProjectilesPool;
    #endregion

    [Inject]
    private void Construct (
        IBuildingsPool iBuildingsPool, 
        IUnitsPool iUnitsPool, 
        IProjectilesPool iProjectilesPool, 
        ILevelObjectFactory iLevelObjectFactory, 
        LevelConfigs levelConfigs) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;
        _iProjectilesPool = iProjectilesPool;
        _iLevelObjectFactory = iLevelObjectFactory;

        // Set configurations
        _numberObjectsCreatedInCaseOfShortage = levelConfigs.NumberObjectsCreatedInCaseOfShortage;
        _hexagonControllerPrefab = levelConfigs.HexagonControllerPrefab;
        _hexagonSize = levelConfigs.HexagonSize;
        _hexagonObjectsControllerPrefab = levelConfigs.HexagonObjectsControllerPrefab;
        _hexagonObjectSize = levelConfigs.HexagonObjectSize;
    }

    public IHexagonControl CreateSomeHexagonControllers() {
        List<IHexagonControl> hexagonControllersList = _iLevelObjectFactory.CreateObjects<IHexagonControl> (
            prefab: _hexagonControllerPrefab, 
            number: _numberObjectsCreatedInCaseOfShortage, 
            trParentObject: _iBuildingsPool.GetHexagonTransformPool(),
            size: _hexagonSize
        );

        _iBuildingsPool.AddNewHexagonControllersInPool(hexagonControllersList);

        return hexagonControllersList[0];
    }

    public IHexagonObjectElement CreateSomeHexagonObjects<T>(T type) where T : System.Enum {
        List<IHexagonObjectElement> hexagonObjectsList = _iLevelObjectFactory.CreateRandomObjects<IHexagonObjectElement> (
            prefabs: GetHexagonObjectPrefabs(type), 
            number: _numberObjectsCreatedInCaseOfShortage, 
            trParentObject: _iBuildingsPool.GetHexagonObjectTransformPool(),
            size: _hexagonObjectSize
        );

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
            prefab: _hexagonObjectsControllerPrefab, 
            number: _numberObjectsCreatedInCaseOfShortage, 
            trParentObject: _iBuildingsPool.GetHexagonObjectTransformPool(),
            size: _hexagonObjectSize
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