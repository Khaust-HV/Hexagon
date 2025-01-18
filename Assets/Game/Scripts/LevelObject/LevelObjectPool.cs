using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LevelObject {
    public sealed class LevelObjectPool : IBuildingsPool, IUnitsPool, IStorageTransformPool {
        #region Transform Pools
            private Transform _trHexagonsPool;
            private Transform _trHexagonObjectsPool;
            private Transform _trUnitsPool;
            private Transform _trSquadsPool;
        #endregion

        private Dictionary<System.Type, Dictionary<System.Enum, List<IHexagonObjectPart>>> _hexagonObjectsStorage = new();

        private List<IHexagonControl> _hexagonControllersList = new();
        private List<IHexagonObjectControl> _hexagonObjectContrlollersList = new();
        private List<IHexagonControl> _squadsList = new();

        #region DI 
            private IBuildingsCreator _iBuildingsCreator;
        #endregion

        [Inject]
        private void Construct(IBuildingsCreator iBuildingsCreator) {
            // Set DI
            _iBuildingsCreator = iBuildingsCreator;

            // Set component
            Transform objectPool = new GameObject("LevelObjectPool").transform;
            (_trHexagonsPool = new GameObject("Hexagons").transform).SetParent(objectPool);
            (_trHexagonObjectsPool = new GameObject("HexagonObjects").transform).SetParent(objectPool);
            (_trUnitsPool = new GameObject("Units").transform).SetParent(objectPool);
        }

        public Transform GetHexagonTransformPool() {
            return _trHexagonsPool;
        }

        public bool GetHexagonControllerByID(int hexagonID, out IHexagonControl iHexagonControl) {
            foreach (var hexagonController in _hexagonControllersList) {
                if (hexagonController.GetHexagonID() == hexagonID) {
                    iHexagonControl = hexagonController;

                    return true;
                }
            }

            iHexagonControl = null;

            return false;
        }

        public IHexagonControl GetDisableHexagonController() {
            foreach (var hexagonController in _hexagonControllersList) {
                if (!hexagonController.IsHexagonControllerUsed()) {
                    return hexagonController;
                }
            }

            return _iBuildingsCreator.CreateSomeHexagonControllers();
        }

        public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList) {
            _hexagonControllersList.AddRange(hexagonControllersList);
        }

        public int GetNumberHexagonControllers() {
            return _hexagonControllersList.Count;
        }

        public Transform GetHexagonObjectTransformPool() {
            return _trHexagonObjectsPool;
        }

        public IHexagonObjectPart GetDisableHexagonObjectPart<T>(T type) where T : System.Enum {
            if (_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage) 
            && typeStorage.TryGetValue(type, out var parts)) {
                foreach (var part in parts) {
                    if (!part.IsHexagonObjectPartUsed()) return part;
                }
            }

            return _iBuildingsCreator.CreateSomeHexagonObjectParts(type);
        }

        public void AddNewHexagonObjectPartsInPool<T>(T type, List<IHexagonObjectPart> hexagonObjectPartsList) where T : System.Enum {
            if (!_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage)) {
                typeStorage = new Dictionary<System.Enum, List<IHexagonObjectPart>>();
                _hexagonObjectsStorage[typeof(T)] = typeStorage;
            }

            if (!typeStorage.TryGetValue(type, out var parts)) {
                parts = new List<IHexagonObjectPart>();
                typeStorage[type] = parts;
            }

            parts.AddRange(hexagonObjectPartsList);
        }

        public IHexagonObjectControl GetDisableHexagonObjectController() {
            foreach (var hexagonObjectController in _hexagonObjectContrlollersList) {
                if (!hexagonObjectController.IsHexagonObjectControllerUsed()) return hexagonObjectController;
            }

            return _iBuildingsCreator.CreateSomeHexagonObjectControllers();
        }

        public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectControllersList) {
            _hexagonObjectContrlollersList.AddRange(hexagonObjectControllersList);
        }
    }
}

public interface IBuildingsPool {
    public bool GetHexagonControllerByID(int hexagonID, out IHexagonControl iHexagonControl);
    public IHexagonControl GetDisableHexagonController();
    public int GetNumberHexagonControllers();
    public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList);

    public IHexagonObjectPart GetDisableHexagonObjectPart<T>(T type) where T : System.Enum;
    public void AddNewHexagonObjectPartsInPool<T>(T type, List<IHexagonObjectPart> hexagonObjectPartsList) where T : System.Enum;
    public IHexagonObjectControl GetDisableHexagonObjectController();
    public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectControllersList);
}

public interface IUnitsPool {
    
}

public interface IStorageTransformPool {
    public Transform GetHexagonTransformPool();
    public Transform GetHexagonObjectTransformPool();
}