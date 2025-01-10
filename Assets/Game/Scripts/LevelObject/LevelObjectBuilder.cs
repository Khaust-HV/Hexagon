using Zenject;
using LevelObjectType;

namespace LevelObject {
    public sealed class LevelObjectBuilder : IBuilder {
        #region DI
            private IBuildingsPool _iBuildingsPool;
            private IUnitsPool _iUnitsPool;
        #endregion

        [Inject]
        private void Construct(IBuildingsPool iBuildingsPool, IUnitsPool iUnitsPool) {
            // Set DI
            _iBuildingsPool = iBuildingsPool;
            _iUnitsPool = iUnitsPool;
        }

        public IHexagonObjectControl CreateHexagonObject<T>(T type) where T : System.Enum {
            switch (type) {
                case MineHexagonObjectsType mineType:
                    IHexagonObjectElement mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(mineType);

                    DecorationHexagonObjectsType decorationType = mineType switch {
                        MineHexagonObjectsType.TreeSource => DecorationHexagonObjectsType.TreeBiome,
                        MineHexagonObjectsType.StoneSource => DecorationHexagonObjectsType.StoneBiome,
                        MineHexagonObjectsType.MetalSource => DecorationHexagonObjectsType.MetalBiome,
                        MineHexagonObjectsType.ElectricitySource => DecorationHexagonObjectsType.ElectricityBiome,
                        MineHexagonObjectsType.OilSource => DecorationHexagonObjectsType.OilBiome,
                        MineHexagonObjectsType.RedCrystalSource => DecorationHexagonObjectsType.RedCrystalBiome,
                        MineHexagonObjectsType.BlueCrystalSource => DecorationHexagonObjectsType.BlueCrystalBiome,
                        MineHexagonObjectsType.GreenCrystalSource => DecorationHexagonObjectsType.GreenCrystalBiome,
                        MineHexagonObjectsType.GlitcheSource => DecorationHexagonObjectsType.GlitcheBiome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"Failed to create a hexagonObject subtype of {mineType}")
                    };

                    IHexagonObjectElement decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(decorationType);

                    IHexagonObjectControl hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case HeapHexagonObjectsType heapType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(heapType);
                    
                    decorationType = heapType switch {
                        HeapHexagonObjectsType.NormalObjects => DecorationHexagonObjectsType.Biome,
                        HeapHexagonObjectsType.QuestObjects => DecorationHexagonObjectsType.LakeBiome,
                        HeapHexagonObjectsType.Lake => DecorationHexagonObjectsType.Biome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"Failed to create a hexagonObject subtype of {heapType}")
                    };

                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(decorationType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case CoreHexagonObjectsType coreType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(coreType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                case BuildebleFieldHexagonObjectsType:
                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(DecorationHexagonObjectsType.Biome);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(unBuildableFieldType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                case RiverHexagonObjectsType riverType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(riverType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(type);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"Failed to create a hexagonObject type of {type}");
                // break;
            }
        }
    }
}

public interface IBuilder {
    public IHexagonObjectControl CreateHexagonObject<T>(T type) where T : System.Enum;
}