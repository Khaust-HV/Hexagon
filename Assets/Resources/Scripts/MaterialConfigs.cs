using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/MaterialConfigs", fileName = "LevelMaterialConfigs")]
    public sealed class MaterialConfigs : ScriptableObject {
        [field: Header("Main settings")]
        [field: SerializeField] public Shader SpawnAndDestroyShaderEffect { get; private set; }
        [field: SerializeField] public float Metallic { get; private set; }
        [field: SerializeField] public float Smoothness { get; private set; }
        [field: Header("Spawn effect settings")]
        [field: SerializeField] public float SpawnEffectTime { get; private set; }
        [field: SerializeField] public float NoiseScaleSpawn { get; private set; }
        [field: SerializeField] public float NoiseStrengthSpawn { get; private set; }
        [field: SerializeField] public float StartCutoffHeightSpawn { get; private set; }
        [field: SerializeField] public float FinishCutoffHeightSpawn { get; private set; }
        [field: SerializeField] public float EdgeWidthSpawn { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color EdgeColorSpawn { get; private set; }
        [field: Header("Destroy effect settings")]
        [field: SerializeField] public float DestroyEffectTime { get; private set; }
        [field: SerializeField] public float NoiseScaleDestroy { get; private set; }
        [field: SerializeField] public float NoiseStrengthDestroy { get; private set; }
        [field: SerializeField] public float StartCutoffHeightDestroy { get; private set; }
        [field: SerializeField] public float FinishCutoffHeightDestroy { get; private set; }
        [field: SerializeField] public float EdgeWidthDestroy { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color EdgeColorDestroy { get; private set; }
    }
}