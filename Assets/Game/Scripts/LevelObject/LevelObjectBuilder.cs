using Zenject;
using LevelObjectType;
using System;

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

        public IHexagonObjectControl CreateHexagonObject<T>(T hexagonObjectType) where T : System.Enum {
            switch (hexagonObjectType) {
                case MineHexagonObjectsType mineType:
                    IHexagonObjectPart mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(mineType);

                    DecorationHexagonObjectsType decorationType = mineType switch {
                        MineHexagonObjectsType.TreeSource => DecorationHexagonObjectsType.TreeBiome,
                        MineHexagonObjectsType.StoneSource => DecorationHexagonObjectsType.StoneBiome,
                        MineHexagonObjectsType.MetalSource => DecorationHexagonObjectsType.MetalBiome,
                        MineHexagonObjectsType.ElectricalSource => DecorationHexagonObjectsType.ElectricalBiome,
                        MineHexagonObjectsType.OilSource => DecorationHexagonObjectsType.OilBiome,
                        MineHexagonObjectsType.RedCrystalSource => DecorationHexagonObjectsType.RedCrystalBiome,
                        MineHexagonObjectsType.BlueCrystalSource => DecorationHexagonObjectsType.BlueCrystalBiome,
                        MineHexagonObjectsType.GreenCrystalSource => DecorationHexagonObjectsType.GreenCrystalBiome,
                        MineHexagonObjectsType.GlitcheSource => DecorationHexagonObjectsType.GlitcheBiome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject subtype of {mineType}")
                    };

                    IHexagonObjectPart decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(decorationType);

                    IHexagonObjectControl hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case HeapHexagonObjectsType heapType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(heapType);
                    
                    decorationType = heapType switch {
                        HeapHexagonObjectsType.NormalObjects => DecorationHexagonObjectsType.Biome,
                        HeapHexagonObjectsType.QuestObjects => DecorationHexagonObjectsType.Biome,
                        HeapHexagonObjectsType.Lake => DecorationHexagonObjectsType.LakeBiome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject subtype of {heapType}")
                    };

                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(decorationType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case CoreHexagonObjectsType coreType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(coreType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                case BuildebleFieldHexagonObjectsType:
                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(DecorationHexagonObjectsType.Biome);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetDecorationObject(decorationObject);

                    return hexagonObject;
                // break;

                case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(unBuildableFieldType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                case RiverHexagonObjectsType riverType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(riverType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);

                    return hexagonObject;
                // break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject type of {hexagonObjectType}");
                // break;
            }
        }

        public IHexagonObjectControl CreateHexagonObject<T>(T hexagonObjectType, T auraType) where T : System.Enum {
            switch (hexagonObjectType) {
                case MineHexagonObjectsType mineType:
                    IHexagonObjectPart mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(mineType);

                    DecorationHexagonObjectsType decorationType = mineType switch {
                        MineHexagonObjectsType.TreeSource => DecorationHexagonObjectsType.TreeBiome,
                        MineHexagonObjectsType.StoneSource => DecorationHexagonObjectsType.StoneBiome,
                        MineHexagonObjectsType.MetalSource => DecorationHexagonObjectsType.MetalBiome,
                        MineHexagonObjectsType.ElectricalSource => DecorationHexagonObjectsType.ElectricalBiome,
                        MineHexagonObjectsType.OilSource => DecorationHexagonObjectsType.OilBiome,
                        MineHexagonObjectsType.RedCrystalSource => DecorationHexagonObjectsType.RedCrystalBiome,
                        MineHexagonObjectsType.BlueCrystalSource => DecorationHexagonObjectsType.BlueCrystalBiome,
                        MineHexagonObjectsType.GreenCrystalSource => DecorationHexagonObjectsType.GreenCrystalBiome,
                        MineHexagonObjectsType.GlitcheSource => DecorationHexagonObjectsType.GlitcheBiome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject subtype of {mineType}")
                    };

                    IHexagonObjectPart decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(decorationType);

                    IHexagonObjectPart auraObject = GetAuraObject(auraType);

                    IHexagonObjectControl hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                case HeapHexagonObjectsType heapType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(heapType);
                    
                    decorationType = heapType switch {
                        HeapHexagonObjectsType.NormalObjects => DecorationHexagonObjectsType.Biome,
                        HeapHexagonObjectsType.QuestObjects => DecorationHexagonObjectsType.Biome,
                        HeapHexagonObjectsType.Lake => DecorationHexagonObjectsType.LakeBiome,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject subtype of {heapType}")
                    };

                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(decorationType);

                    auraObject = GetAuraObject(auraType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetDecorationObject(decorationObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                case CoreHexagonObjectsType coreType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(coreType);

                    auraObject = GetAuraObject(auraType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                case BuildebleFieldHexagonObjectsType:
                    decorationObject = _iBuildingsPool.GetDisableHexagonObjectPart(DecorationHexagonObjectsType.Biome);

                    auraObject = GetAuraObject(auraType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetDecorationObject(decorationObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(unBuildableFieldType);

                    auraObject = GetAuraObject(auraType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                case RiverHexagonObjectsType riverType:
                    mainObject = _iBuildingsPool.GetDisableHexagonObjectPart(riverType);

                    auraObject = GetAuraObject(auraType);

                    hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                    hexagonObject.SetHexagonObjectType(hexagonObjectType);
                    hexagonObject.SetMainObject(mainObject);
                    hexagonObject.SetAuraObject(auraObject);

                    return hexagonObject;
                // break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObjects type of {hexagonObjectType} and {auraType}");
                // break;
            }
        }

        private IHexagonObjectPart GetAuraObject<T>(T auraType) where T : Enum {
            switch (auraType) {
                case ElementAuraType elementAuraType:
                    return _iBuildingsPool.GetDisableHexagonObjectPart(elementAuraType);
                // break;

                case StatsAuraType statsAuraType:
                    return _iBuildingsPool.GetDisableHexagonObjectPart(statsAuraType);
                // break;

                case BuildAuraType buildAuraType:
                    return _iBuildingsPool.GetDisableHexagonObjectPart(buildAuraType);
                // break;

                case TrailAuraType trailAuraType:
                    return _iBuildingsPool.GetDisableHexagonObjectPart(trailAuraType);
                // break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Failed to create a hexagonObject type of {auraType}");
                // break;
            }
        }
    }
}

public interface IBuilder {
    public IHexagonObjectControl CreateHexagonObject<T>(T hexagonObjectType) where T : System.Enum;
    public IHexagonObjectControl CreateHexagonObject<T>(T hexagonObjectType, T auraType) where T : System.Enum;
}