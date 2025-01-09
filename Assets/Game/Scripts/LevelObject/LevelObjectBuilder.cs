using LevelObjectsPool;
using Zenject;

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
                    _ => throw new System.ArgumentOutOfRangeException(nameof(mineType), "Invalid mine type")
                };

                IHexagonObjectElement decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(decorationType);

                if (mainObject == null || decorationObject == null) return null;

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
                    _ => throw new System.ArgumentOutOfRangeException(nameof(heapType), "Invalid heap type")
                };

                decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(decorationType);

                if (mainObject == null || decorationObject == null) return null;

                hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                hexagonObject.SetHexagonObjectType(type);
                hexagonObject.SetMainObject(mainObject);
                hexagonObject.SetDecorationObject(decorationObject);

                return hexagonObject;
            // break;

            case CoreHexagonObjectsType coreType:
                mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(coreType);

                if (mainObject == null) return null;

                hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                hexagonObject.SetHexagonObjectType(type);
                hexagonObject.SetMainObject(mainObject);

                return hexagonObject;
            // break;

            case BuildebleFieldHexagonObjectsType:
                decorationObject = _iBuildingsPool.GetDisableHexagonObjectElement(DecorationHexagonObjectsType.Biome);

                if (decorationObject == null) return null;

                hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                hexagonObject.SetHexagonObjectType(type);
                hexagonObject.SetDecorationObject(decorationObject);

                return hexagonObject;
            // break;

            case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
                mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(unBuildableFieldType);

                if (mainObject == null) return null;

                hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                hexagonObject.SetHexagonObjectType(type);
                hexagonObject.SetMainObject(mainObject);

                return hexagonObject;
            // break;

            case RiverHexagonObjectsType riverType:
                mainObject = _iBuildingsPool.GetDisableHexagonObjectElement(riverType);

                if (mainObject == null) return null;

                hexagonObject = _iBuildingsPool.GetDisableHexagonObjectController();
                hexagonObject.SetHexagonObjectType(type);
                hexagonObject.SetMainObject(mainObject);

                return hexagonObject;
            // break;

            default:
                throw new System.ArgumentOutOfRangeException("Invalid hexagonObject type");
            // break;
        }
    }
}

public interface IBuilder {
    public IHexagonObjectControl CreateHexagonObject<T>(T type) where T : System.Enum;
}