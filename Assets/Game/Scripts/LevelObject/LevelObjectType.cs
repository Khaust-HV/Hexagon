namespace LevelObjectType {
    #region Hexagon Objects Types
        public enum DecorationHexagonObjectsType {
            // Ganeral decoration
            Biome,

            // Mine decoration
            TreeBiome,
            StoneBiome,
            MetalBiome,
            ElectricalBiome,
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
            ElectricalSource,
            OilSource,
            RedCrystalSource,
            BlueCrystalSource,
            GreenCrystalSource,
            GlitcheSource,

            // Extraction of resources
            TreeMining,
            StoneMining,
            MetalMining,
            ElectricalMining,
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

        public enum ElementAuraType {
            FireAura,
            FrostAura,
            ElectricAura,
            NatureAura
        }

        public enum StatsAuraType {
            ShieldAura,
            AttackRangePositiveAura,
            AttackRangeNegativeAura,
            CamoAura,
            AttackStrengthPositiveAura,
            AttackStrengthNegativeAura,
            GlobalHealingAura,
            AttackSpeedPositioveAura,
            AttackSpeedNegativeAura
        }

        public enum BuildAuraType {
            BuildingEfficiencyPositiveAura,
            BuildingEfficiencyNegativeAura,
            ConstructionSpeedPositiveAura,
            ConstructionSpeedNegativeAura
        }

        public enum TrailAuraType {
            MovementSpeedPositiveAura,
            MovementSpeedNegativeAura,
            FlightDisableAura
        }
    #endregion


    #region Units types
        // Units types
    #endregion
}