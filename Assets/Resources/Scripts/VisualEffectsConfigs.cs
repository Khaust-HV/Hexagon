using UnityEngine;
using UnityEngine.VFX;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/VisualEffectsConfigs", fileName = "VisualEffectsConfigs")]
    public sealed class VisualEffectsConfigs : ScriptableObject {
        [field: Header("Default material settings")]
        [field: SerializeField] public float DefaultMetallic { get; private set; }
        [field: SerializeField] public float DefaultSmoothness { get; private set; }
        [field: Space(25)]
        
        [field: Header("Shaders pool")]
        [field: SerializeField] public Material DissolveNonUV { get; private set; }
        [field: SerializeField] public Material DissolveWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndEmissionTextureWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndEmission3TexturesWithUV { get; private set; }
        [field: SerializeField] public Material HologramAndDissolve { get; private set; }
        [field: SerializeField] public Material EmissionFullObject { get; private set; }
        [field: SerializeField] public Material DissolveAndEmissionFullObjectAndVerticalNoice { get; private set; }
        [field: Space(25)]

        [field: Header("Default dissolve settings for spawn effect")] // DissolveNonUV/WithUV shader
        [field: SerializeField] public float DefaultSpawnNoiseScale { get; private set; }
        [field: SerializeField] public float DefaultSpawnNoiseStrength { get; private set; }
        [field: SerializeField] public float DefaultSpawnEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultSpawnEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default dissolve settings for destroy effect")] // DissolveNonUV/WithUV shader
        [field: SerializeField] public float DefaultDestroyNoiseScale { get; private set; }
        [field: SerializeField] public float DefaultDestroyNoiseStrength { get; private set; }
        [field: SerializeField] public float DefaultDestroyEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultDestroyEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Destroy hexagon and hexagonObject effect settings")]
        [field: SerializeField] public VisualEffectAsset DestroyHexagonOrHexagonObjectVFXEffect { get; private set; }
        [field: SerializeField] public int DefaultDestroyVFXNumberParticles { get; private set; }
        [field: SerializeField] public Vector3 DefaultDestroyVFXStartVelocity { get; private set; }
        [field: SerializeField] public float DefaultDestroyVFXLinearDrag { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultDestroyVFXEmissionColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default hologram and dissolve settings for preview and spawn effect")] // HologramAndDissolve shader
        [field: SerializeField] public float DefaultHologramEdgeWidth { get; private set; }
        [field: SerializeField] public float DefaultHologramAnimationSpeed { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramFresnelColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Blue/Red/Green crystals/electrical emission effect settings")] // DissolveAndEmissionTextureWithUV shader
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
        [field: Space(25)]

        [field: Header("Glitche emission effect settings")] // DissolveAndEmission3TexturesWithUV shader
        [field: SerializeField] public Texture2D FirstGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color FirstGlitcheEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D SecondGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color SecondGlitcheEmissionColor { get; private set; }
        [field: SerializeField] public Texture2D ThirdGlitcheEmissionTexture { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ThirdGlitcheEmissionColor { get; private set; }
        [field: Space(25)]

        [field: Header("ShieldAura visual settings")] // DissolveAndEmissionFullObjectAndVerticalNoice shader
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
        [field: Header("Shield spawn and destroy effect settings")] // EmissionFullObjectForVFX shader
        [field: SerializeField] public VisualEffectAsset ShieldAuraShieldSpawn { get; private set; }
        [field: SerializeField] public Vector3 ShieldAuraStartVelocity { get; private set; }
        [field: SerializeField] public float ShieldAuraLinearDrag { get; private set; }
        [field: SerializeField] public int ShieldAuraParticlesNumberForShieldDestroy { get; private set; }
    }
}