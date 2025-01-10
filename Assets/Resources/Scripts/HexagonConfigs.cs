using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/HexagonConfigs", fileName = "LevelHexagonConfigs")]
    public sealed class HexagonConfigs : ScriptableObject {
        [field: Header("Rotation settings")]
        [field: SerializeField] public float RotationTime { get; private set; }
        [field: SerializeField] public float MinTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public float MaxTimeForAutoHexagonRotate { get; private set; }
        [field: SerializeField] public int MinNumberRotationsForHexagon { get; private set; }
        [field: SerializeField] public int MaxNumberRotationsForHexagon { get; private set; }
        [field: Space(50)]

        [field: Header("Color hexagon type settings")]
        [field: SerializeField] public Color DefaultHexagonColor { get; private set; }
        [field: SerializeField] public Color ShadowHexagonColor { get; private set; }
        [field: SerializeField] public Color RandomHexagonColor { get; private set; }
        [field: SerializeField] public Color TemporaryHexagonColor { get; private set; }
        [field: Space(50)]

        [field: Header("Unit on hexagon settings")]
        [field: SerializeField] public float MinTimeUnitInAreaForHexagon { get; private set; }
        [field: SerializeField] public float MaxTimeUnitInAreaForHexagon { get; private set; }
        [field: Space(50)]
        
        [field: Header("Destroy settings")]
        [field: SerializeField] public float ForcePlannedExplosion { get; private set; }
        [field: SerializeField] public float ForceNonPlannedExplosion { get; private set; }
    }
}