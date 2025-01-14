using HexagonControl;
using GameConfigs;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;
using LevelObjectType;

namespace Managers {
    public sealed class LevelManager : IHexagonTarget, IGenerateLevel {
        #region DI
            private IBuildingsPool _iBuildingsPool;
            private IUnitsPool _iUnitsPool;
            private IInteractingWithObject _iInteractingWithObject;
            private IBuilder _iBuilder;
            private LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct (
            IBuildingsPool iBuildingsPool, 
            IUnitsPool iUnitsPool, 
            IInteractingWithObject iInteractingWithObject,
            IBuilder iBuilder, 
            LevelConfigs levelConfigs) {
            // Set DI
            _iBuildingsPool = iBuildingsPool;
            _iUnitsPool = iUnitsPool;
            _iInteractingWithObject = iInteractingWithObject;
            _iBuilder = iBuilder;
            // Set configurations
            _levelConfigs = levelConfigs;
        }

        public bool IsMakeThisHexagonAsTarget(int hexagonID) {
            return true; // FIX IT !
        }

        public void SetThisHexagonTargetActive(int hexagonID, bool isActive) {
            if (_iBuildingsPool.GetHexagonControllerByID(hexagonID, out IHexagonControl hexagonController)) {
                if (isActive) hexagonController.CameraLooking += HexagonDestroyOrRotation;
                else hexagonController.CameraLooking -= HexagonDestroyOrRotation;
            }
        }

        private void HexagonDestroyOrRotation() {
            _iInteractingWithObject.CancelChoosingToBuildOrImprove();
        }

        public async void GenerateLevel() {
            SpreadHexagons();
            await SetRandomHexagonTypeAsync(); // FIX IT !
        }

        private void CreateNewHexagonObjectForHexagon(IHexagonControl iHexagonControl) {
            var hexagonObject = _iBuilder.CreateHexagonObject(GetRandomHexagonObjectType()); // FIX IT !
            // var hexagonObject = _iBuilder.CreateHexagonObject(MineHexagonObjectsType.TreeSource); // FIX IT !

            iHexagonControl.SetHexagonObject(hexagonObject);
        }

        private void SpreadHexagons() {
            switch (_levelConfigs.AlgorithmOfLevelBuilding) {
                case AlgorithmOfLevelBuilding.Circular:
                    float hexagonRadius = _levelConfigs.HexagonSize * 1.2f;
                    float xOffset = hexagonRadius * 1.5f;
                    float zOffset = hexagonRadius * Mathf.Sqrt(3) * 0.86f;

                    int hexagonNumber = 0;

                    _iBuildingsPool.GetDisableHexagonController().SetHexagonPositionAndID(Vector3.zero, hexagonNumber++);

                    for (int ring = 1; ring <= _levelConfigs.NumberOfRings; ring++) {
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

        private System.Enum GetRandomHexagonObjectType() { // FIX IT !
            int randomNumberHexagonObjectType = Random.Range(0, 6);
            return randomNumberHexagonObjectType switch {
                0 => (MineHexagonObjectsType)Random.Range(0, 9),
                1 => BuildebleFieldHexagonObjectsType.FlamingRainTower,
                2 => (UnBuildebleFieldHexagonObjectsType)Random.Range(0, 5),
                3 => (CoreHexagonObjectsType)Random.Range(0, 2),
                4 => (HeapHexagonObjectsType)Random.Range(0, 3),
                5 => (RiverHexagonObjectsType)Random.Range(0, 8),
                _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType)
            };
        }

        private async Task SetRandomHexagonTypeAsync() { // FIX IT !
            for (int i = 0; i < _iBuildingsPool.GetNumberHexagonControllers(); i++) {
                if (_iBuildingsPool.GetHexagonControllerByID(i, out IHexagonControl hexagonController)) {
                    int randomType = Random.Range(0, 5);

                    hexagonController.NeedHexagonObject += CreateNewHexagonObjectForHexagon;

                    hexagonController.SetHexagonType((HexagonType)randomType);
                }
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