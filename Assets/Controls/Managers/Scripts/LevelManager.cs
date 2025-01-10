using HexagonControl;
using GameConfigs;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;

namespace Managers {
    public sealed class LevelManager : IHexagonTarget, IGenerateLevel {
        #region DI
            private IBuildingsPool _iBuildingsPool;
            private IUnitsPool _iUnitsPool;
            private IInteractingWithObject _iInteractingWithObject;
            private LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct(IBuildingsPool iBuildingsPool, IUnitsPool iUnitsPool, IInteractingWithObject iInteractingWithObject, LevelConfigs levelConfigs) {
            // Set DI
            _iBuildingsPool = iBuildingsPool;
            _iUnitsPool = iUnitsPool;
            _iInteractingWithObject = iInteractingWithObject;

            // Set configurations
            _levelConfigs = levelConfigs;
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

        public async void GenerateLevel() {
            SpreadHexagons();
            await RandomSetHexagonTypeAsync(); // FIX IT !
        }

        private void CreateNewHexagonObjectForHexagon(IHexagonControl iHexagonControl) {
            // Create new hexagonObject
        }

        private void SpreadHexagons() {
            switch (_levelConfigs.AlgorithmOfLevelBuilding) {
                case AlgorithmOfLevelBuilding.Circular:
                    float hexagonRadius = _levelConfigs.HexagonSize * 1.2f;
                    float xOffset = hexagonRadius * 1.5f;
                    float zOffset = hexagonRadius * Mathf.Sqrt(3) * 0.86f;

                    int hexagonNumber = 0;

                    var hexagonController = _iBuildingsPool.GetDisableHexagonController();
                    if (!hexagonController.IsHexagonControllerAlreadyUsed()) hexagonController.NeedHexagonObject += CreateNewHexagonObjectForHexagon;
                    hexagonController.SetHexagonPositionAndID(Vector3.zero, hexagonNumber++);

                    for (int ring = 1; ring <= _levelConfigs.NumberOfRings; ring++) {
                        for (int side = 0; side < 6; side++) {
                            for (int step = 0; step < ring; step++) {
                                float x = (ring - step) * xOffset * Mathf.Cos(Mathf.PI / 3 * side) + step * xOffset * Mathf.Cos(Mathf.PI / 3 * (side + 1));
                                float z = (ring - step) * zOffset * Mathf.Sin(Mathf.PI / 3 * side) + step * zOffset * Mathf.Sin(Mathf.PI / 3 * (side + 1));

                                Vector3 offset = new Vector3(x, 0, z);

                                hexagonController = _iBuildingsPool.GetDisableHexagonController();
                                if (!hexagonController.IsHexagonControllerAlreadyUsed()) hexagonController.NeedHexagonObject += CreateNewHexagonObjectForHexagon;
                                hexagonController.SetHexagonPositionAndID(offset, hexagonNumber++);
                            }
                        }
                    }
                break;
            }
        }

        public async Task RandomSetHexagonTypeAsync() { // FIX IT !
            for (int i = 0; i < _iBuildingsPool.GetNumberHexagonControllers(); i++) {
                if (!_iBuildingsPool.GetHexagonControllerByID(i)?.IsHexagonControllerActive() ?? false) continue;

                int randomType = Random.Range(0, 5);

                _iBuildingsPool.GetHexagonControllerByID(i)?.SetHexagonType((HexagonType)randomType);

                await Task.Delay(15);
            }   
        }   
    }

    public enum AlgorithmOfLevelBuilding {
        Circular,
    }
}

public interface IHexagonTarget {
    public bool IsMakeThisHexagonAsTarget(int hexagonID);
    public void SetThisHexagonTargetActive(int hexagonID, bool isActive);
}

public interface IGenerateLevel {
    public void GenerateLevel();
}