using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public sealed class LevelConfigs : ScriptableObject {
        [field: SerializeField] public GameObject HexagonPrefab { get; private set; }
        [field: SerializeField] public float HexagonSize { get; private set; }
        [field: SerializeField] public AlgorithmOfLevelBuilding AlgorithmOfLevelBuilding { get; private set; }
        [field: SerializeField] public int NumberOfRings { get; private set; }
    }

    [CreateAssetMenu(menuName = "Configs/HexagonConfigs", fileName = "HexagonConfigs")]
    public sealed class HexagonConfigs : ScriptableObject {
        [Header("Rotation settings")]
        [field: SerializeField] public int RotationSpeed { get; private set; }
        [field: SerializeField] public float MinTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public float MaxTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public int MinNumberRotationsForHexagon { get; private set; }
        [field: SerializeField] public int MaxNumberRotationsForHexagon { get; private set; }
        [Header("Type settings")]
        [field: SerializeField] public Material DefaultHexagonMaterial { get; private set; }
        [field: SerializeField] public Material ShadowHexagonMaterial { get; private set; }
        [field: SerializeField] public Material RandomHexagonMaterial { get; private set; }
        [field: SerializeField] public Material TemporaryHexagonMaterial { get; private set; }
        [Header("Squad on hexagon settings")]
        [field: SerializeField] public float MinTimeSquadForHexagon { get; private set; }
        [field: SerializeField] public float MaxTimeSquadForHexagon { get; private set; }
        [Header("Destroy settings")]
        [field: SerializeField] public float TimeToDestroyParts { get; private set; }
        [field: SerializeField] public float ForcePlannedExplosion { get; private set; }
        [field: SerializeField] public float ForceNonPlannedExplosion { get; private set; }
    }
}