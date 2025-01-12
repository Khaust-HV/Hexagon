using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LevelObject {
    public sealed class LevelObjectPool : IBuildingsPool, IUnitsPool, IProjectilesPool, IStorageTransformPool {
        #region Transform Pools
            private Transform _trHexagonsPool;
            private Transform _trHexagonObjectsPool;
            private Transform _trUnitsPool;
            private Transform _trSquadsPool;
        #endregion

        private Dictionary<System.Type, Dictionary<System.Enum, List<IHexagonObjectElement>>> _hexagonObjectsStorage = new();

        private List<IHexagonControl> _hexagonControllersList = new();
        private List<IHexagonObjectControl> _hexagonObjectContrlollersList = new();
        private List<IHexagonControl> _squadsList = new();

        #region DI 
            private ICreator _iCreator;
        #endregion

        [Inject]
        private void Construct(ICreator iCreator) {
            // Set DI
            _iCreator = iCreator;

            // Set component
            Transform objectPool = new GameObject("LevelObjectPool").transform;
            (_trHexagonsPool = new GameObject("Hexagons").transform).SetParent(objectPool);
            (_trHexagonObjectsPool = new GameObject("HexagonObjects").transform).SetParent(objectPool);
            (_trUnitsPool = new GameObject("Units").transform).SetParent(objectPool);
        }

        public Transform GetHexagonTransformPool() {
            return _trHexagonsPool;
        }

        public IHexagonControl GetHexagonControllerByID(int hexagonID) {
            foreach (var hexagonController in _hexagonControllersList) {
                if (hexagonController.GetHexagonID() == hexagonID) return hexagonController;
            }

            return null;
        }

        public IHexagonControl GetDisableHexagonController() {
            foreach (var hexagonController in _hexagonControllersList) {
                if (!hexagonController.IsHexagonControllerActive()) {
                    return hexagonController;
                }
            }

            return _iCreator.CreateSomeHexagonControllers();
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

        public IHexagonObjectElement GetDisableHexagonObjectElement<T>(T type) where T : System.Enum {
            if (_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage) 
            && typeStorage.TryGetValue(type, out var elements)) {
                foreach (var element in elements) {
                    if (!element.IsHexagonObjectElementActive()) return element;
                }
            }

            return _iCreator.CreateSomeHexagonObjectElements(type);
        }

        public void AddNewHexagonObjectElementsInPool<T>(T type, List<IHexagonObjectElement> hexagonObjectList) where T : System.Enum {
            if (!_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage)) {
                typeStorage = new Dictionary<System.Enum, List<IHexagonObjectElement>>();
                _hexagonObjectsStorage[typeof(T)] = typeStorage;
            }

            if (!typeStorage.TryGetValue(type, out var elements)) {
                elements = new List<IHexagonObjectElement>();
                typeStorage[type] = elements;
            }

            elements.AddRange(hexagonObjectList);
        }

        public IHexagonObjectControl GetDisableHexagonObjectController() {
            foreach (var hexagonObjectController in _hexagonObjectContrlollersList) {
                if (!hexagonObjectController.IsHexagonObjectControllerActive()) return hexagonObjectController;
            }

            return _iCreator.CreateSomeHexagonObjectControllers();
        }

        public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectContrlollersList) {
            _hexagonObjectContrlollersList.AddRange(hexagonObjectContrlollersList);
        }
    }
}

public interface IBuildingsPool {
    public IHexagonControl GetHexagonControllerByID(int hexagonID);
    public IHexagonControl GetDisableHexagonController();
    public int GetNumberHexagonControllers();
    public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList);

    public IHexagonObjectElement GetDisableHexagonObjectElement<T>(T type) where T : System.Enum;
    public void AddNewHexagonObjectElementsInPool<T>(T type, List<IHexagonObjectElement> hexagonObjectList) where T : System.Enum;
    public IHexagonObjectControl GetDisableHexagonObjectController();
    public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectContrlollersList);
}

public interface IUnitsPool {
    
}

public interface IProjectilesPool {

}

public interface IStorageTransformPool {
    public Transform GetHexagonTransformPool();
    public Transform GetHexagonObjectTransformPool();
}