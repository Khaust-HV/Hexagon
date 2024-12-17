using System.Collections.Generic;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class LevelObjectCreator : IBuildingsCreate, IUnitsCreate, IProjectilesCreate {
    #region Level Settings
        private GameObject _hexagonPrefab;
        private float _hexagonSize;
        private AlgorithmOfLevelBuilding _algorithmOfLevelBuilding;
        private int _numberOfRings;
    #endregion

    #region DI
        private LevelObjectFactory _levelObjectFactory;

        private IBuildingsPool _iBuildingsPool;
        private IUnitsPool _iUnitsPool;
        private IProjectilesPool _iProjectilesPool;
    #endregion

    [Inject]
    private void Construct(IBuildingsPool iBuildingsPool, IUnitsPool iUnitsPool, IProjectilesPool iProjectilesPool, LevelConfigs levelConfigs) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;
        _iProjectilesPool = iProjectilesPool;

        // Set configurations
        _hexagonPrefab = levelConfigs.HexagonPrefab;
        _hexagonSize = levelConfigs.HexagonSize;
        _algorithmOfLevelBuilding = levelConfigs.AlgorithmOfLevelBuilding;
        _numberOfRings = levelConfigs.NumberOfRings;

        // Init DI
        _levelObjectFactory = new LevelObjectFactory();
    }

    public int CreateHexagons() {
        int sumNumberHexagons = 0;

        switch (_algorithmOfLevelBuilding) {
            case AlgorithmOfLevelBuilding.Circular:
                int sumFirstNumber = _numberOfRings * (_numberOfRings + 1) / 2;

                sumNumberHexagons = 1 + 6 * sumFirstNumber;

                List<IHexagonControl> hexagonList = _levelObjectFactory.CreateObjects<IHexagonControl> (
                    prefab: _hexagonPrefab, 
                    number: sumNumberHexagons, 
                    trParentObject: _iBuildingsPool.GetHexagonTransformPool(),
                    size: _hexagonSize
                );

                _iBuildingsPool.AddRangeHexagons(hexagonList);
            break;
        } 

        return sumNumberHexagons;
    }
}

public interface IBuildingsCreate {
    public int CreateHexagons();
}

public interface IUnitsCreate {
    
}

public interface IProjectilesCreate {
    
}