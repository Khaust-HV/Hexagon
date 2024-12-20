using Hexagon;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LevelObjectsPool {
    public sealed class LevelObjectPool : IBuildingsPool, IUnitsPool, IProjectilesPool {
        #region Transform Pools
            private Transform _trHexagonsPool;
            private Transform _trHexagonObjectsPool;
            private Transform _trUnitsPool;
            private Transform _trSquadsPool;
        #endregion

        private Dictionary<System.Type, Dictionary<System.Enum, List<IHexagonObjectElement>>> _hexagonObjectsStorage;

        private List<IHexagonControl> _hexagonControllersList;
        private List<IHexagonObjectControl> _hexagonObjectContrlollersList;
        private List<IHexagonControl> _squadsList;

        #region DI 
            private IBuildingsCreate _iBuildingsCreate;
            private IUnitsCreate _iUnitsCreate;
            private IProjectilesCreate _iProjectilesCreate;
        #endregion

        [Inject]
        private void Construct(IBuildingsCreate iBuildingsCreate, IUnitsCreate iUnitsCreate, IProjectilesCreate iProjectilesCreate) {
            _iBuildingsCreate = iBuildingsCreate;
            _iUnitsCreate = iUnitsCreate;
            _iProjectilesCreate = iProjectilesCreate;

            Transform objectPool = new GameObject("LevelObjectPool").transform;

            _trHexagonsPool = new GameObject("Hexagons").transform;
            _trHexagonsPool.SetParent(objectPool);

            _trHexagonObjectsPool = new GameObject("HexagonObjects").transform;
            _trHexagonObjectsPool.SetParent(objectPool);

            _trUnitsPool = new GameObject("Units").transform;
            _trUnitsPool.SetParent(objectPool);

            _trSquadsPool = new GameObject("Squads").transform;
            _trSquadsPool.SetParent(objectPool);

            _hexagonObjectsStorage = new Dictionary<System.Type, Dictionary<System.Enum, List<IHexagonObjectElement>>>();

            _hexagonControllersList = new List<IHexagonControl>();
            _hexagonObjectContrlollersList = new List<IHexagonObjectControl>();
            _squadsList = new List<IHexagonControl>();
        }

        public Transform GetHexagonTransformPool() {
            return _trHexagonsPool;
        }

        public IHexagonControl GetHexagonControllerByID(int hexagonID) {
            return _hexagonControllersList[hexagonID];
        }

        public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList) {
            _hexagonControllersList.AddRange(hexagonControllersList);
        }

        public Transform GetHexagonObjectTransformPool() {
            return _trHexagonObjectsPool;
        }

        public IHexagonObjectElement GetDisableHexagonObjectElement<T>(T type) where T : System.Enum {
            if (_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage) && 
                typeStorage.TryGetValue(type, out var elements)) {
                    foreach (var element in elements) {
                        if (element.IsHexagonObjectElementActive()) return element;
                    }

                    return _iBuildingsCreate.CreateSomeHexagonObjects(type);
            }

            return null;
        }

        public void AddNewHexagonObjectsInPool<T>(T type, List<IHexagonObjectElement> hexagonObjectList) where T : System.Enum {
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

            return _iBuildingsCreate.CreateSomeHexagonObjectControllers();
        }

        public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectContrlollersList) {
            _hexagonObjectContrlollersList.AddRange(hexagonObjectContrlollersList);
        }
    }

    public interface IBuildingsPool {
        public Transform GetHexagonTransformPool();
        public IHexagonControl GetHexagonControllerByID(int hexagonID);
        public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList);

        public Transform GetHexagonObjectTransformPool();
        public IHexagonObjectElement GetDisableHexagonObjectElement<T>(T type) where T : System.Enum;
        public void AddNewHexagonObjectsInPool<T>(T type, List<IHexagonObjectElement> hexagonObjectList) where T : System.Enum;
        public IHexagonObjectControl GetDisableHexagonObjectController();
        public void AddNewHexagonObjectControllersInPool(List<IHexagonObjectControl> hexagonObjectContrlollersList);
    }

    public interface IUnitsPool {
        
    }

    public interface IProjectilesPool {

    }

    #region Hexagon Objects Types
        public enum DecorationHexagonObjectsType {

        }

        public enum MineHexagonObjectsType {

        }

        public enum HeapHexagonObjectsType {

        }

        public enum CoreHexagonObjectsType {

        }

        public enum BuildebleFieldHexagonObjectsType {

        }

        public enum UnBuildebleFieldHexagonObjectsType {

        }

        public enum LiquidHexagonObjectsType {

        }
    #endregion


    #region Units types
        // Units types
    #endregion
}