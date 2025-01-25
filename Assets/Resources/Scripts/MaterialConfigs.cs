using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/MaterialConfigs", fileName = "MaterialConfigs")]
    public sealed class MaterialConfigs : ScriptableObject {
        [field: Header("BaseMaterial settings")]
        [field: SerializeField] public float BaseMetallic { get; private set; }
        [field: SerializeField] public float BaseSmoothness { get; private set; }
        [field: Space(50)]
        
        [field: Header("General shaders settings")]
        [field: SerializeField] public Shader DissolveNonUV { get; private set; }
        [field: SerializeField] public Shader DissolveWithUV { get; private set; }
        [field: SerializeField] public Shader DissolveAndEmissionTextureWithUV { get; private set; }
        [field: SerializeField] public Shader DissolveAndEmission3TexturesWithUV { get; private set; }
        [field: SerializeField] public Shader HologramAndDissolve { get; private set; }
        [field: SerializeField] public Shader EmissionFullObject { get; private set; }
        [field: SerializeField] public Shader DissolveAndEmissionFullObjectAndVerticalNoice { get; private set; }
        [field: Space(50)]

        [field: Header("Dissolve for spawn effect settings")]
        [field: SerializeField] public float SpawnEffectTime { get; private set; }
        [field: SerializeField] public float SpawnNoiseScale { get; private set; }
        [field: SerializeField] public float SpawnNoiseStrength { get; private set; }
        [field: SerializeField] public float SpawnEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color SpawnEdgeColor { get; private set; }
        [field: Space(50)]

        [field: Header("Dissolve for destroy effect settings")]
        [field: SerializeField] public float DestroyEffectTime { get; private set; }
        [field: SerializeField] public float DestroyNoiseScale { get; private set; }
        [field: SerializeField] public float DestroyNoiseStrength { get; private set; }
        [field: SerializeField] public float DestroyEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DestroyEdgeColor { get; private set; }
        [field: Space(50)]

        [field: Header("Hologram and dissolve for spawn effect settings")]
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
        [field: Space(50)]

        [field: Header("Blue/Red/Green crystals shader settings")]
        [field: SerializeField] public Texture2D ElectricalEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ElectricalEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D RedCrystalEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color RedCrystalEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D BlueCrystalEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color BlueCrystalEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D GreenCrystalEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color GreenCrystalEmissionColor { get; private set; }
        [field: Space(50)]

        [field: Header("Glitche shader settings")]
        [field: SerializeField] public Texture2D FirstGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color FirstGlitcheEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D SecondGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color SecondGlitcheEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D ThirdGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ThirdGlitcheEmissionColor { get; private set; }
        [field: Space(50)]

        [field: Header("ShieldAura shader  settings")]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ShieldAuraEmissionFresnelColor { get; private set; }
        [field: SerializeField] public float ShieldAuraEmissionFresnelPower { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ShieldAuraEmissionColor { get; private set; }
        [field: SerializeField] public float ShieldAuraVerticalNoiceScale { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightLowEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightStandardEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightHighEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightReallyHighEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraSpawnTime { get; private set; }
        [field: SerializeField] public float ShieldAuraDestroyTime { get; private set; }
        [field: SerializeField] public float ShieldAuraStartCutoffHeight { get; private set; }
        [field: SerializeField] public float ShieldAuraFinishCutoffHeight { get; private set; }
    }
}