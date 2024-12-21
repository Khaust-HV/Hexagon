using LevelObjectsPool;
using Hexagon;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class LevelManager : IHexagonTarget, IGenerateLevel {
    #region Level Config Settings
        private float _hexagonSize;
        private AlgorithmOfLevelBuilding _algorithmOfLevelBuilding;
        private int _numberOfRings;
    #endregion
    
    #region DI
        private IBuildingsPool _iBuildingsPool;
        private IUnitsPool _iUnitsPool;
        private IInteractingWithObject _iInteractingWithObject;
    #endregion

    [Inject]
    private void Construct(IBuildingsPool iBuildingsPool, IUnitsPool iUnitsPool, IInteractingWithObject iInteractingWithObject, LevelConfigs levelConfigs) {
        // Set DI
        _iBuildingsPool = iBuildingsPool;
        _iUnitsPool = iUnitsPool;
        _iInteractingWithObject = iInteractingWithObject;

        // Set configurations
        _hexagonSize = levelConfigs.HexagonSize;
        _algorithmOfLevelBuilding = levelConfigs.AlgorithmOfLevelBuilding;
        _numberOfRings = levelConfigs.NumberOfRings;
    }

    public bool IsMakeThisHexagonAsTarget(int hexagonID) {
        return true; // FIX IT !
    }

    public void SetThisHexagonTargetActive(int hexagonID, bool isActive) {
        if (isActive) _iBuildingsPool.GetHexagonControllerByID(hexagonID).CameraLooking += HexagonDestroyOrRotation;
        else _iBuildingsPool.GetHexagonControllerByID(hexagonID).CameraLooking -= HexagonDestroyOrRotation;
    }

    private void HexagonDestroyOrRotation() {
        _iInteractingWithObject.CancelChoosingToBuildOrImprove();
    }

    public void GenerateLevel() {
        SpreadHexagons();
        RandomSetHexagonType(); // FIX IT !
    }

    private void SpreadHexagons() {
        switch (_algorithmOfLevelBuilding) {
            case AlgorithmOfLevelBuilding.Circular:
                float hexagonRadius = _hexagonSize * 1.2f;
                float xOffset = hexagonRadius * 1.5f;
                float zOffset = hexagonRadius * Mathf.Sqrt(3) * 0.86f;

                int hexagonNumber = 0;

                _iBuildingsPool.GetDisableHexagonController().SetHexagonPositionAndID(Vector3.zero, hexagonNumber++);

                for (int ring = 1; ring <= _numberOfRings; ring++) {
                    for (int side = 0; side < 6; side++) {
                        for (int step = 0; step < ring; step++) {
                            float x = (ring - step) * xOffset * Mathf.Cos(Mathf.PI / 3 * side) + step * xOffset * Mathf.Cos(Mathf.PI / 3 * (side + 1));
                            float z = (ring - step) * zOffset * Mathf.Sin(Mathf.PI / 3 * side) + step * zOffset * Mathf.Sin(Mathf.PI / 3 * (side + 1));

                            Vector3 offset = new Vector3(x, 0, z);

                            _iBuildingsPool.GetDisableHexagonController().SetHexagonPositionAndID(offset, hexagonNumber++);
                        }
                    }
                }
            break;
        }
    }

    private void RandomSetHexagonType() { // FIX IT !
        for (int i = 0; i < _iBuildingsPool.GetNumberHexagonControllers(); i++) {
            if (!_iBuildingsPool.GetHexagonControllerByID(i).IsHexagonControllerActive()) continue;

            int randomType = Random.Range(0, 5);

            _iBuildingsPool.GetHexagonControllerByID(i).SetHexagonType((HexagonType)randomType);
        }
    }
}

public interface IHexagonTarget {
    public bool IsMakeThisHexagonAsTarget(int hexagonID);
    public void SetThisHexagonTargetActive(int hexagonID, bool isActive);
}

public interface IGenerateLevel {
    public void GenerateLevel();
}

public enum AlgorithmOfLevelBuilding {
    Circular,
}