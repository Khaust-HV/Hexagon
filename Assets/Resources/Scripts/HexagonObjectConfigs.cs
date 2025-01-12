using UnityEngine;
using LevelObjectType;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/HexagonObjectConfigs", fileName = "LevelHexagonObjectConfigs")]
    public sealed class HexagonObjectConfigs : ScriptableObject {
        // Category of Decorations
        [field: Header("Biome settings <Decoration>")]
        [field: SerializeField] public GameObject[] BiomePrefabs { get; private set; }
        
        [field: Header("LakeBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] LakeBiomePrefabs { get; private set; }

        [field: Header("TreeBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] TreeBiomePrefabs { get; private set; }

        [field: Header("StoneBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] StoneBiomePrefabs { get; private set; }

        [field: Header("MetalBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] MetalBiomePrefabs { get; private set; }

        [field: Header("ElectricityBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] ElectricityBiomePrefabs { get; private set; }

        [field: Header("OilBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] OilBiomePrefabs { get; private set; }

        [field: Header("RedCrystalBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] RedCrystalBiomePrefabs { get; private set; }

        [field: Header("BlueCrystalBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] BlueCrystalBiomePrefabs { get; private set; }

        [field: Header("GreenCrystalBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] GreenCrystalBiomePrefabs { get; private set; }
        
        [field: Header("GlitcheBiome settings <Decoration>")]
        [field: SerializeField] public GameObject[] GlitcheBiomePrefabs { get; private set; }
        [field: Space(50)]

        // Category of Mine
        [field: Header("TreeSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] TreeSourcePrefabs { get; private set; }
        [field: Header("StoneSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] StoneSourcePrefabs { get; private set; }
        [field: Header("MetalSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] MetalSourcePrefabs { get; private set; }
        [field: Header("ElectricitySource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] ElectricitySourcePrefabs { get; private set; }
        [field: Header("OilSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] OilSourcePrefabs { get; private set; }
        [field: Header("RedCrystalSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] RedCrystalSourcePrefabs { get; private set; }
        [field: Header("BlueCrystalSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] BlueCrystalSourcePrefabs { get; private set; }
        [field: Header("GreenCrystalSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] GreenCrystalSourcePrefabs { get; private set; }
        [field: Header("GlitcheSource settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] GlitcheSourcePrefabs { get; private set; }
        [field: Header("TreeMining settings <Mine> <Source>")]
        [field: SerializeField] public GameObject[] TreeMiningPrefabs { get; private set; }
        [field: Space(15)]

        [field: Header("StoneMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] StoneMiningPrefabs { get; private set; }
        [field: Header("MetalMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] MetalMiningPrefabs { get; private set; }
        [field: Header("ElectricityMinig settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] ElectricityMinigPrefabs { get; private set; }
        [field: Header("OilMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] OilMiningPrefabs { get; private set; }
        [field: Header("RedCrystalMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] RedCrystalMiningPrefabs { get; private set; }
        [field: Header("BlueCrystalMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] BlueCrystalMiningPrefabs { get; private set; }
        [field: Header("GreenCrystalMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] GreenCrystalMiningPrefabs { get; private set; }
        [field: Header("GlitcheMining settings <Mine> <Mining>")]
        [field: SerializeField] public GameObject[] GlitcheMiningPrefabs { get; private set; }
        [field: Space(50)]

        // Category of BuildebleField
        [field: Header("FlamingRainTower settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] FlamingRainTowerPrefabs { get; private set; }
        [field: Header("VineEnsnareTower settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] VineEnsnareTowerPrefabs { get; private set; }
        [field: Header("FrostArrowTree settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] FrostArrowTreePrefabs { get; private set; }
        [field: Header("ChaoticGrove settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] ChaoticGrovePrefabs { get; private set; }
        [field: Header("PiercingBallista settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] PiercingBallistaPrefabs { get; private set; }
        [field: Header("ReinforcedArcherTower settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] ReinforcedArcherTowerPrefabs { get; private set; }
        [field: Header("ElectricSapling settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] ElectricSaplingPrefabs { get; private set; }
        [field: Header("BurningSpout settings <BuildebleField> <Tree>")]
        [field: SerializeField] public GameObject[] BurningSpoutPrefabs { get; private set; }
        [field: Space(15)]

        [field: Header("VolcanicEruptionTower settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] VolcanicEruptionTowerPrefabs { get; private set; }
        [field: Header("EntanglingObelisk settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] EntanglingObeliskPrefabs { get; private set; }
        [field: Header("FrozenPillar settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] FrozenPillarPrefabs { get; private set; }
        [field: Header("PixelatedMonolith settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] PixelatedMonolithPrefabs { get; private set; }
        [field: Header("FortifiedCatapult settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] FortifiedCatapultPrefabs { get; private set; }
        [field: Header("CannonadeBastion settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] CannonadeBastionPrefabs { get; private set; }
        [field: Header("ArcLightningTower settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] ArcLightningTowerPrefabs { get; private set; }
        [field: Header("MoltenFortress settings <BuildebleField> <Stone>")]
        [field: SerializeField] public GameObject[] MoltenFortressPrefabs { get; private set; }
        [field: Space(15)]

        [field: Header("PlasmaForge settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] PlasmaForgePrefabs { get; private set; }
        [field: Header("BioMechGuardian settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] BioMechGuardianPrefabs { get; private set; }
        [field: Header("CryoArtillery settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] CryoArtilleryPrefabs { get; private set; }
        [field: Header("GlitchEngine settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] GlitchEnginePrefabs { get; private set; }
        [field: Header("SiegeBastion settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] SiegeBastionPrefabs { get; private set; }
        [field: Header("FortifiedMarksman settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] FortifiedMarksmanPrefabs { get; private set; }
        [field: Header("TeslaOvercharger settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] TeslaOverchargerPrefabs { get; private set; }
        [field: Header("IgnitionBlaster settings <BuildebleField> <Metal>")]
        [field: SerializeField] public GameObject[] IgnitionBlasterPrefabs { get; private set; }
        [field: Space(15)]

        [field: Header("WarriorHallOfTimber settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] WarriorHallOfTimberPrefabs { get; private set; }
        [field: Header("StoneforgeBarracks settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] StoneforgeBarracksPrefabs { get; private set; }
        [field: Header("IronVanguardCitadel settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] MetalVanguardCitadelPrefabs { get; private set; }
        [field: Header("ElectrospireComplex settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] ElectrospireComplexPrefabs { get; private set; }
        [field: Header("OiledMechanismHub settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] OiledMechanismHubPrefabs { get; private set; }
        [field: Header("InfernalArcaneForge settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] InfernalArcaneForgePrefabs { get; private set; }
        [field: Header("VerdantEnclaveOfNature settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] VerdantEnclaveOfNaturePrefabs { get; private set; }
        [field: Header("FrostwovenSanctum settings <BuildebleField> <HireUnits>")]
        [field: SerializeField] public GameObject[] FrostwovenSanctumPrefabs { get; private set; }
        [field: Space(50)]

        // Category of UnBuildebleField
        [field: Header("StartOrFinishRoad settings <UnBuildebleField>")]
        [field: SerializeField] public GameObject[] StartOrFinishRoadPrefabs { get; private set; }
        [field: Header("StraightRoad settings <UnBuildebleField>")]
        [field: SerializeField] public GameObject[] StraightRoadPrefabs { get; private set; }
        [field: Header("LongTurnRoad settings <UnBuildebleField>")]
        [field: SerializeField] public GameObject[] LongTurnRoadPrefabs { get; private set; }
        [field: Header("NearTurnRoad settings <UnBuildebleField>")]
        [field: SerializeField] public GameObject[] NearTurnRoadPrefabs { get; private set; }
        [field: Header("HardWay settings <UnBuildebleField>")]
        [field: SerializeField] public GameObject[] HardWayPrefabs { get; private set; }
        [field: Space(50)]

        // Category of Core
        [field: Header("MainCore settings <Core>")]
        [field: SerializeField] public GameObject[] MainCorePrefabs { get; private set; }
        [field: Header("ShieldCore settings <Core>")]
        [field: SerializeField] public GameObject[] ShieldCorePrefabs { get; private set; }
        [field: Space(50)]

        // Category of Heap
        [field: Header("NormalObjects settings <Heap>")]
        [field: SerializeField] public GameObject[] NormalObjectsPrefabs { get; private set; }
        [field: Header("Lake settings <Heap>")]
        [field: SerializeField] public GameObject[] LakePrefabs { get; private set; }
        [field: Header("QuestObjects settings <Heap>")]
        [field: SerializeField] public GameObject[] QuestObjectsPrefabs { get; private set; }
        [field: Space(50)]

        // Category of River
        [field: Header("StartOrFinishSafeRiver settings <River> <SafeRiver>")]
        [field: SerializeField] public GameObject[] StartOrFinishSafeRiverPrefabs { get; private set; }
        [field: Header("StraightSafeRiver settings <River> <SafeRiver>")]
        [field: SerializeField] public GameObject[] StraightSafeRiverPrefabs { get; private set; }
        [field: Header("LongTurnSafeRiver settings <River> <SafeRiver>")]
        [field: SerializeField] public GameObject[] LongTurnSafeRiverPrefabs { get; private set; }
        [field: Header("NearTurnSafeRiver settings <River> <SafeRiver>")]
        [field: SerializeField] public GameObject[] NearTurnSafeRiverPrefabs { get; private set; }
        [field: Space(15)]

        [field: Header("StartOrFinishDangerousRiver settings <River> <DangerousRiver>")]
        [field: SerializeField] public GameObject[] StartOrFinishDangerousRiverPrefabs { get; private set; }
        [field: Header("StraightDangerousRiver settings <River> <DangerousRiver>")]
        [field: SerializeField] public GameObject[] StraightDangerousRiverPrefabs { get; private set; }
        [field: Header("LongTurnDangerousRiver settings <River> <DangerousRiver>")]
        [field: SerializeField] public GameObject[] LongTurnDangerousRiverPrefabs { get; private set; }
        [field: Header("NearTurnDangerousRiver settings <River> <DangerousRiver>")]
        [field: SerializeField] public GameObject[] NearTurnDangerousRiverPrefabs { get; private set; }
        [field: Space(50)]

        [field: Header("EmptyHexagonObjects")]
        [field: SerializeField] public GameObject[] MainEmpty { get; private set; }
        [field: SerializeField] public GameObject[] DecorationEmpty { get; private set; }
        
        [field: Space(15)]
        [field: SerializeField] public Color DecorationTypeTextColor { get; private set; }
        [field: SerializeField] public Color MineTypeTextColor { get; private set; }
        [field: SerializeField] public Color BuildebleFieldTypeTextColor { get; private set; }
        [field: SerializeField] public Color UnBuildebleFieldTypeTextColor { get; private set; }
        [field: SerializeField] public Color CoreTypeTextColor { get; private set; }
        [field: SerializeField] public Color HeapTypeTextColor { get; private set; }
        [field: SerializeField] public Color RiverTypeTextColor { get; private set; }


        public GameObject[] GetHexagonObjectPrefabs<T>(T type) where T : System.Enum {
            switch (type) {
                case DecorationHexagonObjectsType decorationType:
                    var prefabs =  decorationType switch {
                        DecorationHexagonObjectsType.Biome => BiomePrefabs,
                        DecorationHexagonObjectsType.LakeBiome => LakeBiomePrefabs,
                        DecorationHexagonObjectsType.TreeBiome => TreeBiomePrefabs,
                        DecorationHexagonObjectsType.StoneBiome => StoneBiomePrefabs,
                        DecorationHexagonObjectsType.MetalBiome => MetalBiomePrefabs,
                        DecorationHexagonObjectsType.ElectricityBiome => ElectricityBiomePrefabs,
                        DecorationHexagonObjectsType.OilBiome => OilBiomePrefabs,
                        DecorationHexagonObjectsType.RedCrystalBiome => RedCrystalBiomePrefabs,
                        DecorationHexagonObjectsType.BlueCrystalBiome => BlueCrystalBiomePrefabs,
                        DecorationHexagonObjectsType.GreenCrystalBiome => GreenCrystalBiomePrefabs,
                        DecorationHexagonObjectsType.GlitcheBiome => GlitcheBiomePrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {decorationType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return DecorationEmpty;

                    return prefabs;
                // break;

                case MineHexagonObjectsType mineType:
                    prefabs =  mineType switch {
                        MineHexagonObjectsType.TreeSource => TreeSourcePrefabs,
                        MineHexagonObjectsType.StoneSource => StoneSourcePrefabs,
                        MineHexagonObjectsType.MetalSource => MetalSourcePrefabs,
                        MineHexagonObjectsType.ElectricitySource => ElectricitySourcePrefabs,
                        MineHexagonObjectsType.OilSource => OilSourcePrefabs,
                        MineHexagonObjectsType.RedCrystalSource => RedCrystalSourcePrefabs,
                        MineHexagonObjectsType.BlueCrystalSource => BlueCrystalSourcePrefabs,
                        MineHexagonObjectsType.GreenCrystalSource => GreenCrystalSourcePrefabs,
                        MineHexagonObjectsType.GlitcheSource => GlitcheSourcePrefabs,

                        MineHexagonObjectsType.TreeMining => TreeMiningPrefabs,
                        MineHexagonObjectsType.StoneMining => StoneMiningPrefabs,
                        MineHexagonObjectsType.MetalMining => MetalMiningPrefabs,
                        MineHexagonObjectsType.ElectricityMining => ElectricityMinigPrefabs,
                        MineHexagonObjectsType.OilMining => OilMiningPrefabs,
                        MineHexagonObjectsType.RedCrystalMining => RedCrystalMiningPrefabs,
                        MineHexagonObjectsType.BlueCrystalMining => BlueCrystalMiningPrefabs,
                        MineHexagonObjectsType.GreenCrystalMining => GreenCrystalMiningPrefabs,
                        MineHexagonObjectsType.GlitcheMining => GlitcheMiningPrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {mineType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                case BuildebleFieldHexagonObjectsType buildableFieldType:
                    prefabs = buildableFieldType switch {
                        BuildebleFieldHexagonObjectsType.FlamingRainTower => FlamingRainTowerPrefabs,
                        BuildebleFieldHexagonObjectsType.VineEnsnareTower => VineEnsnareTowerPrefabs,
                        BuildebleFieldHexagonObjectsType.FrostArrowTree => FrostArrowTreePrefabs,
                        BuildebleFieldHexagonObjectsType.ChaoticGrove => ChaoticGrovePrefabs,
                        BuildebleFieldHexagonObjectsType.PiercingBallista => PiercingBallistaPrefabs,
                        BuildebleFieldHexagonObjectsType.ReinforcedArcherTower => ReinforcedArcherTowerPrefabs,
                        BuildebleFieldHexagonObjectsType.ElectricSapling => ElectricSaplingPrefabs,
                        BuildebleFieldHexagonObjectsType.BurningSpout => BurningSpoutPrefabs,

                        BuildebleFieldHexagonObjectsType.VolcanicEruptionTower => VolcanicEruptionTowerPrefabs,
                        BuildebleFieldHexagonObjectsType.EntanglingObelisk => EntanglingObeliskPrefabs,
                        BuildebleFieldHexagonObjectsType.FrozenPillar => FrozenPillarPrefabs,
                        BuildebleFieldHexagonObjectsType.PixelatedMonolith => PixelatedMonolithPrefabs,
                        BuildebleFieldHexagonObjectsType.FortifiedCatapult => FortifiedCatapultPrefabs,
                        BuildebleFieldHexagonObjectsType.CannonadeBastion => CannonadeBastionPrefabs,
                        BuildebleFieldHexagonObjectsType.ArcLightningTower => ArcLightningTowerPrefabs,
                        BuildebleFieldHexagonObjectsType.MoltenFortress => MoltenFortressPrefabs,

                        BuildebleFieldHexagonObjectsType.PlasmaForge => PlasmaForgePrefabs,
                        BuildebleFieldHexagonObjectsType.BioMechGuardian => BioMechGuardianPrefabs,
                        BuildebleFieldHexagonObjectsType.CryoArtillery => CryoArtilleryPrefabs,
                        BuildebleFieldHexagonObjectsType.GlitchEngine => GlitchEnginePrefabs,
                        BuildebleFieldHexagonObjectsType.SiegeBastion => SiegeBastionPrefabs,
                        BuildebleFieldHexagonObjectsType.FortifiedMarksman => FortifiedMarksmanPrefabs,
                        BuildebleFieldHexagonObjectsType.TeslaOvercharger => TeslaOverchargerPrefabs,
                        BuildebleFieldHexagonObjectsType.IgnitionBlaster => IgnitionBlasterPrefabs,

                        BuildebleFieldHexagonObjectsType.WarriorHallOfTimber => WarriorHallOfTimberPrefabs,
                        BuildebleFieldHexagonObjectsType.StoneforgeBarracks => StoneforgeBarracksPrefabs,
                        BuildebleFieldHexagonObjectsType.MetalVanguardCitadel => MetalVanguardCitadelPrefabs,
                        BuildebleFieldHexagonObjectsType.ElectrospireComplex => ElectrospireComplexPrefabs,
                        BuildebleFieldHexagonObjectsType.OiledMechanismHub => OiledMechanismHubPrefabs,
                        BuildebleFieldHexagonObjectsType.InfernalArcaneForge => InfernalArcaneForgePrefabs,
                        BuildebleFieldHexagonObjectsType.VerdantEnclaveOfNature => VerdantEnclaveOfNaturePrefabs,
                        BuildebleFieldHexagonObjectsType.FrostwovenSanctum => FrostwovenSanctumPrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {buildableFieldType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                case UnBuildebleFieldHexagonObjectsType unBuildableFieldType:
                    prefabs = unBuildableFieldType switch {
                        UnBuildebleFieldHexagonObjectsType.StartOrFinishRoad => StartOrFinishRoadPrefabs,
                        UnBuildebleFieldHexagonObjectsType.StraightRoad => StraightRoadPrefabs,
                        UnBuildebleFieldHexagonObjectsType.LongTurnRoad => LongTurnRoadPrefabs,
                        UnBuildebleFieldHexagonObjectsType.NearTurnRoad => NearTurnRoadPrefabs,
                        UnBuildebleFieldHexagonObjectsType.HardWay => HardWayPrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {unBuildableFieldType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                case CoreHexagonObjectsType coreType:
                    prefabs = coreType switch {
                        CoreHexagonObjectsType.MainCore => MainCorePrefabs,
                        CoreHexagonObjectsType.ShieldCore => ShieldCorePrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {coreType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                case HeapHexagonObjectsType heapType:
                    prefabs = heapType switch {
                        HeapHexagonObjectsType.NormalObjects => NormalObjectsPrefabs,
                        HeapHexagonObjectsType.Lake => LakePrefabs,
                        HeapHexagonObjectsType.QuestObjects => QuestObjectsPrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {heapType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                case RiverHexagonObjectsType riverType:
                    prefabs = riverType switch {
                        RiverHexagonObjectsType.StartOrFinishSafeRiver => StartOrFinishSafeRiverPrefabs,
                        RiverHexagonObjectsType.StraightSafeRiver => StraightSafeRiverPrefabs,
                        RiverHexagonObjectsType.LongTurnSafeRiver => LongTurnSafeRiverPrefabs,
                        RiverHexagonObjectsType.NearTurnSafeRiver => NearTurnSafeRiverPrefabs,

                        RiverHexagonObjectsType.StartOrFinishDangerousRiver => StartOrFinishDangerousRiverPrefabs,
                        RiverHexagonObjectsType.StraightDangerousRiver => StraightDangerousRiverPrefabs,
                        RiverHexagonObjectsType.LongTurnDangerousRiver => LongTurnDangerousRiverPrefabs,
                        RiverHexagonObjectsType.NearTurnDangerousRiver => NearTurnDangerousRiverPrefabs,
                        _ => throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs subtype of {riverType} in the configuration file")
                    };

                    if (prefabs.Length == 0) return MainEmpty;

                    return prefabs;
                // break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType, $"No prefabs type of {type} in the configuration file");
                // break;
            }
        }
    }
}