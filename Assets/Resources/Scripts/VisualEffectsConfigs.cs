using UnityEngine;
using UnityEngine.VFX;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/VisualEffectsConfigs", fileName = "VisualEffectsConfigs")]
    public sealed class VisualEffectsConfigs : ScriptableObject {
        [field: Header("Default material settings")]
        [field: SerializeField] public float DefaultMetallic { get; private set; }
        [field: SerializeField] public float DefaultSmoothness { get; private set; }
        [field: Space(25)]
        
        [field: Header("General material pool")]
        [field: SerializeField] public Material DissolveNonUV { get; private set; }
        [field: SerializeField] public Material DissolveAndHitWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndHitAndEmission1TextureWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndHitAndEmission2TextureWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndHitAndEmission3TexturesWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndEmissionFullObjectAndVerticalNoiceGhost { get; private set; }
        [field: SerializeField] public Material EmissionFullObjectGhost { get; private set; }
        [field: SerializeField] public Material DissolveAndHologramGhost { get; private set; }
        [field: Space(25)]

        // [field: Header("Specialty material pool")]
        // // In the process of coming up
        // [field: Space(25)]

        [field: Header("Default hit effect settings")]
        [field: SerializeField] public float DefaultHitEffectTime { get; private set; }
         [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHitEffectColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default dissolve settings for spawn effect")]
        [field: SerializeField] public float DefaultSpawnNoiseScale { get; private set; }
        [field: SerializeField] public float DefaultSpawnNoiseStrength { get; private set; }
        [field: SerializeField] public float DefaultSpawnEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultSpawnEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default dissolve settings for destroy effect")]
        [field: SerializeField] public float DefaultDestroyNoiseScale { get; private set; }
        [field: SerializeField] public float DefaultDestroyNoiseStrength { get; private set; }
        [field: SerializeField] public float DefaultDestroyEdgeWidth { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultDestroyEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Destroy hexagon and hexagonObject effect settings")]
        [field: SerializeField] public VisualEffectAsset DestroyHexagonOrHexagonObjectVFXEffect { get; private set; }
        [field: SerializeField] public Texture2D DefaultDestroyTextureParticle { get; private set; }
        [field: SerializeField] public float DefaultDestroySizeParticles { get; private set; }
        [field: SerializeField] public int DefaultDestroyVFXNumberParticles { get; private set; }
        [field: SerializeField] public Vector3 DefaultDestroyVFXStartVelocity { get; private set; }
        [field: SerializeField] public float DefaultDestroyVFXLinearDrag { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultDestroyVFXEmissionColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default hologram and dissolve settings for preview and spawn effect")]
        [field: SerializeField] public float DefaultHologramEdgeWidth { get; private set; }
        [field: SerializeField] public float DefaultHologramAnimationSpeed { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramFresnelColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramEdgeColor { get; private set; }
        [field: Space(25)]

        [field: Header("Object with one emission effect settings")]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ElectricalEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color RedCrystalEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color BlueCrystalEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color GreenCrystalEmissionColor { get; private set; }
        [field: Space(25)]

        // [field: Header("Object with two emission effect settings")]
        // // In the process of coming up
        // [field: Space(25)]

        [field: Header("Object with three emission effect settings")]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color GlitcheFirstEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color GlitcheSecondEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color GlitcheThirdEmissionColor { get; private set; }
        [field: Space(25)]

        [field: Header("ShieldAura visual settings")]
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
        [field: Space(10)]
        [field: SerializeField] public VisualEffectAsset ShieldAuraShieldSpawn { get; private set; }
        [field: SerializeField] public Vector3 ShieldAuraStartVelocity { get; private set; }
        [field: SerializeField] public float ShieldAuraLinearDrag { get; private set; }
        [field: SerializeField] public float ShieldAuraBaseAlpha { get; private set; }
        [field: SerializeField] public int ShieldAuraParticlesNumberForShieldDestroy { get; private set; }
    }
}