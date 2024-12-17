using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class LevelManager : IHexagonTarget, IGenerateLevel {
    #region Level Settings
        private float _hexagonSize;
        private AlgorithmOfLevelBuilding _algorithmOfLevelBuilding;
        private int _numberOfRings;
    #endregion

    private int _sumNumberHexagons;
    
    #region DI
        private IBuildingsPool _iBuildingsPool;
        private IUnitsPool _iUnitsPool;
    #endregion

    [Inject]
    private void Construct(IBuildingsPool iBuildingsPool, IUnitsPool iUnitsPool, LevelConfigs levelConfigs) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;

        // Set configurations
        _hexagonSize = levelConfigs.HexagonSize;
        _algorithmOfLevelBuilding = levelConfigs.AlgorithmOfLevelBuilding;
        _numberOfRings = levelConfigs.NumberOfRings;
    }

    public bool SetHexagonTarget(int hexagonID) {
        return true; // FIX IT !
    }

    public void GenerateLevel() {
        SpreadHexagons();
        RandomSetHexagonType(); // FIX IT !
    }

    public void SetSumNumberHexagons(int sumNumberHexagons) {
        _sumNumberHexagons = sumNumberHexagons;
    }

    private void SpreadHexagons() {
        switch (_algorithmOfLevelBuilding) {
            case AlgorithmOfLevelBuilding.Circular:
                float hexagonRadius = _hexagonSize * 1.2f;
                float xOffset = hexagonRadius * 1.5f;
                float zOffset = hexagonRadius * Mathf.Sqrt(3) * 0.86f;

                int hexagonNumber = 0;

                _iBuildingsPool.GetHexagonByID(hexagonNumber).SetPositionAndID(Vector3.zero, hexagonNumber++);

                for (int ring = 1; ring <= _numberOfRings; ring++) {
                    for (int side = 0; side < 6; side++) {
                        for (int step = 0; step < ring; step++) {
                            float x = (ring - step) * xOffset * Mathf.Cos(Mathf.PI / 3 * side) + step * xOffset * Mathf.Cos(Mathf.PI / 3 * (side + 1));
                            float z = (ring - step) * zOffset * Mathf.Sin(Mathf.PI / 3 * side) + step * zOffset * Mathf.Sin(Mathf.PI / 3 * (side + 1));

                            Vector3 offset = new Vector3(x, 0, z);

                            _iBuildingsPool.GetHexagonByID(hexagonNumber).SetPositionAndID(offset, hexagonNumber++);
                        }
                    }
                }
            break;
        }
    }

    private void RandomSetHexagonType() { // FIX IT !
        for (int i = 0; i < _sumNumberHexagons; i++) {
            int randomType = Random.Range(0, 5);

            _iBuildingsPool.GetHexagonByID(i).SetHexagonTypeAndEnable((HexagonType)randomType);
        }
    }
}

public interface IHexagonTarget {
    public bool SetHexagonTarget(int hexagonID);
}

public interface IGenerateLevel {
    public void SetSumNumberHexagons(int sumNumberHexagons);
    public void GenerateLevel();
}

public enum AlgorithmOfLevelBuilding {
    Circular,
}