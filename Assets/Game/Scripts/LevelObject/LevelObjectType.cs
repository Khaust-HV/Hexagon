namespace LevelObjectType {
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
            MetalVanguardCitadel,
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