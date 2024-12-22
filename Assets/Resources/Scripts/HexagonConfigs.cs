using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/HexagonConfigs", fileName = "HexagonConfigs")]
    public sealed class HexagonConfigs : ScriptableObject {
        [field: Header("Rotation settings")]
        [field: SerializeField] public float RotationTime { get; private set; }
        [field: SerializeField] public float MinTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public float MaxTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public int MinNumberRotationsForHexagon { get; private set; }
        [field: SerializeField] public int MaxNumberRotationsForHexagon { get; private set; }
        [field: Header("Type settings")]
        [field: SerializeField] public Material DefaultHexagonMaterial { get; private set; }
        [field: SerializeField] public Material ShadowHexagonMaterial { get; private set; }
        [field: SerializeField] public Material RandomHexagonMaterial { get; private set; }
        [field: SerializeField] public Material TemporaryHexagonMaterial { get; private set; }
        [field: Header("Unit on hexagon settings")]
        [field: SerializeField] public float MinTimeUnitInAreaForHexagon { get; private set; }
        [field: SerializeField] public float MaxTimeUnitInAreaForHexagon { get; private set; }
        [field: Header("Destroy settings")]
        [field: SerializeField] public float TimeDestroyAndSpawnObject { get; private set; }
        [field: SerializeField] public float TimeToDestroyParts { get; private set; }
        [field: SerializeField] public float ForcePlannedExplosion { get; private set; }
        [field: SerializeField] public float ForceNonPlannedExplosion { get; private set; }
    }
}