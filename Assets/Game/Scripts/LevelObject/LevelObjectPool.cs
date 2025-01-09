using Hexagon;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LevelObjectsPool {
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
            private IBuildingsCreate _iBuildingsCreate;
            private IUnitsCreate _iUnitsCreate;
            private IProjectilesCreate _iProjectilesCreate;
        #endregion

        [Inject]
        private void Construct(IBuildingsCreate iBuildingsCreate, IUnitsCreate iUnitsCreate, IProjectilesCreate iProjectilesCreate) {
            // Set DI
            _iBuildingsCreate = iBuildingsCreate;
            _iUnitsCreate = iUnitsCreate;
            _iProjectilesCreate = iProjectilesCreate;

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

            return _iBuildingsCreate.CreateSomeHexagonControllers();
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
            if (_hexagonObjectsStorage.TryGetValue(typeof(T), out var typeStorage) && 
                typeStorage.TryGetValue(type, out var elements)) {
                    foreach (var element in elements) {
                        if (!element.IsHexagonObjectElementActive()) return element;
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
        public IHexagonControl GetHexagonControllerByID(int hexagonID);
        public IHexagonControl GetDisableHexagonController();
        public int GetNumberHexagonControllers();
        public void AddNewHexagonControllersInPool(List<IHexagonControl> hexagonControllersList);

        public IHexagonObjectElement GetDisableHexagonObjectElement<T>(T type) where T : System.Enum;
        public void AddNewHexagonObjectsInPool<T>(T type, List<IHexagonObjectElement> hexagonObjectList) where T : System.Enum;
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

    #region Hexagon Objects Types
        public enum DecorationHexagonObjectsType {
            // Ganeral decoration
            Biome,

            // Mine decoration
            TreeBiome,
            StoneBiome,
            MetalBiome,
            ElectricityBiome,
            OilBiome,
            RedCrystalBiome,
            BlueCrystalBiome,
            GreenCrystalBiome,
            GlitcheBiome,
            
            // Lake decoration
            LakeBiome
        }

        public enum MineHexagonObjectsType {
            // Sources of resources
            TreeSource,
            StoneSource,
            MetalSource,
            ElectricitySource,
            OilSource,
            RedCrystalSource,
            BlueCrystalSource,
            GreenCrystalSource,
            GlitcheSource,

            // Extraction of resources
            TreeMining,
            StoneMining,
            MetalMining,
            ElectricityMining,
            OilMining,
            RedCrystalMining,
            BlueCrystalMining,
            GreenCrystalMining,
            GlitcheMining
        }

        public enum HeapHexagonObjectsType {
            NormalObjects,
            QuestObjects,
            Lake
        }

        public enum CoreHexagonObjectsType {
            MainCore,
            ShieldCore
        }

        public enum BuildebleFieldHexagonObjectsType {
            Nothing,

            // Wood towers
            FlamingRainTower,
            VineEnsnareTower,
            FrostArrowTree,
            ChaoticGrove,
            PiercingBallista,
            ReinforcedArcherTower,
            ElectricSapling,
            BurningSpout,

            // Stone towers
            VolcanicEruptionTower,
            EntanglingObelisk,
            FrozenPillar,
            PixelatedMonolith,
            FortifiedCatapult,
            CannonadeBastion,
            ArcLightningTower,
            MoltenFortress,

            // Metal towers
            PlasmaForge,
            BioMechGuardian,
            CryoArtillery,
            GlitchEngine,
            SiegeBastion,
            FortifiedMarksman,
            TeslaOvercharger,
            IgnitionBlaster,

            // Buildings for hiring units
            WarriorHallOfTimber,
            StoneforgeBarracks,
            IronVanguardCitadel,
            ElectrospireComplex,
            OiledMechanismHub,
            InfernalArcaneForge,
            VerdantEnclaveOfNature,
            FrostwovenSanctum
        }

        public enum UnBuildebleFieldHexagonObjectsType {
            // Constructor for roads
            StartOrFinishRoad,
            StraightRoad,
            LongTurnRoad,
            NearTurnRoad,

            // Poorly traveled areas
            HardWay
        }

        public enum RiverHexagonObjectsType {
            // Safe river
            StartOrFinishSafeRiver,
            StraightSafeRiver,
            LongTurnSafeRiver,
            NearTurnSafeRiver,

            // Dangerous river
            StartOrFinishDangerousRiver,
            StraightDangerousRiver,
            LongTurnDangerousRiver,
            NearTurnDangerousRiver
        }
    #endregion


    #region Units types
        // Units types
    #endregion
}