using System.Collections.Generic;
using GameConfigs;
using Zenject;
using LevelObjectType;

namespace LevelObject {
    public sealed class LevelObjectCreator : IBuildingsCreator, IUnitsCreator, IProjectilesCreator {
        #region DI
            private ILevelObjectFactory _iLevelObjectFactory;
            private IBuildingsPool _iBuildingsPool;
            private IUnitsPool _iUnitsPool;
            private IStorageTransformPool _iStorageTransformPool;
            private LevelConfigs _levelConfigs;
            private HexagonObjectConfigs _hexagonObjectConfigs;
        #endregion

        [Inject]
        private void Construct (
            IBuildingsPool iBuildingsPool, 
            IUnitsPool iUnitsPool, 
            ILevelObjectFactory iLevelObjectFactory,
            IStorageTransformPool iStorageTransformPool, 
            LevelConfigs levelConfigs,
            HexagonObjectConfigs hexagonObjectConfigs
            ) {
            // Set DI
            _iBuildingsPool = iBuildingsPool;
            _iUnitsPool = iUnitsPool;
            _iLevelObjectFactory = iLevelObjectFactory;
            _iStorageTransformPool = iStorageTransformPool;

            // Set configurations
            _levelConfigs = levelConfigs;
            _hexagonObjectConfigs = hexagonObjectConfigs;
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

        public IHexagonObjectElement CreateSomeHexagonObjectElements<T>(T type, int numberObjects = 0) where T : System.Enum {
            var prefabs = _hexagonObjectConfigs.GetHexagonObjectPrefabs(type);

            List<IHexagonObjectElement> hexagonObjectElementsList;

            if (numberObjects == 0) numberObjects = _levelConfigs.NumberObjectsCreatedInCaseOfShortage;

            if (prefabs.Length == 1) {
                hexagonObjectElementsList = _iLevelObjectFactory.CreateObjects<IHexagonObjectElement> (
                    prefab: prefabs[0], 
                    number: numberObjects, 
                    trParentObject: _iStorageTransformPool.GetHexagonObjectTransformPool(),
                    size: _levelConfigs.HexagonObjectSize
                );
            } else {
                hexagonObjectElementsList = _iLevelObjectFactory.CreateRandomObjects<IHexagonObjectElement> (
                    prefabs: prefabs, 
                    number: numberObjects, 
                    trParentObject: _iStorageTransformPool.GetHexagonObjectTransformPool(),
                    size: _levelConfigs.HexagonObjectSize
                );
            }

            foreach (var hexagonObjectElement in hexagonObjectElementsList) {
                hexagonObjectElement.SetHexagonObjectType(type);
            }

            _iBuildingsPool.AddNewHexagonObjectElementsInPool(type, hexagonObjectElementsList);

            return hexagonObjectElementsList[0];
        }
    }
}

public interface IBuildingsCreator {
    public IHexagonControl CreateSomeHexagonControllers();
    public IHexagonObjectElement CreateSomeHexagonObjectElements<T>(T type, int numberObjects = 0) where T : System.Enum;
    public IHexagonObjectControl CreateSomeHexagonObjectControllers();
}

public interface IUnitsCreator {

}

public interface IProjectilesCreator {

}