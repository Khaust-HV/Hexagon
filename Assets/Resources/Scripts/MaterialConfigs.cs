using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/MaterialConfigs", fileName = "LevelMaterialConfigs")]
    public sealed class MaterialConfigs : ScriptableObject {
        [field: Header("BaseMaterial settings")]
        [field: SerializeField] public Shader DissolveShaderEffectNonUV { get; private set; }
        [field: SerializeField] public Shader DissolveShaderEffectWithUV { get; private set; }
        [field: SerializeField] public float BaseMetallic { get; private set; }
        [field: SerializeField] public float BaseSmoothness { get; private set; }
        
        [field: Header("Dissolve for spawn effect settings")]
        [field: SerializeField] public float SpawnEffectTime { get; private set; }
        [field: SerializeField] public float SpawnNoiseScale { get; private set; }
        [field: SerializeField] public float SpawnNoiseStrength { get; private set; }
        [field: SerializeField] public float SpawnStartCutoffHeight { get; private set; }
        [field: SerializeField] public float SpawnFinishCutoffHeight { get; private set; }
        [field: SerializeField] public float SpawnEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color SpawnEdgeColor { get; private set; }

        [field: Header("Dissolve for destroy effect settings")]
        [field: SerializeField] public float DestroyEffectTime { get; private set; }
        [field: SerializeField] public float DestroyNoiseScale { get; private set; }
        [field: SerializeField] public float DestroyNoiseStrength { get; private set; }
        [field: SerializeField] public float DestroyStartCutoffHeight { get; private set; }
        [field: SerializeField] public float DestroyFinishCutoffHeight { get; private set; }
        [field: SerializeField] public float DestroyEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DestroyEdgeColor { get; private set; }

        [field: Header("Hologram and dissolve for spawn effect settings")]
        [field: SerializeField] public Shader HologramAndDissolveShaderEffect { get; private set; }
        [field: SerializeField] public float HologramMetallic { get; private set; }
        [field: SerializeField] public float HologramSmoothness { get; private set; }
        [field: SerializeField] public float HologramSpawnEffectTime { get; private set; }
        [field: SerializeField] public float HologramNoiseScale { get; private set; }
        [field: SerializeField] public float HologramNoiseStrength { get; private set; }
        [field: SerializeField] public float HologramEdgeWidth { get; private set; }
        [field: SerializeField] public float HologramAnimationSpeed { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color HologramBaseColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color HologramFresnelColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color HologramEdgeColor { get; private set; }
    }
}