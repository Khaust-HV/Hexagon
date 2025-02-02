using UnityEngine;
using UnityEngine.VFX;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/VisualEffectsConfigs", fileName = "VisualEffectsConfigs")]
    public sealed class VisualEffectsConfigs : ScriptableObject {
        [field: Header("Default material settings")]
        [field: SerializeField] public float DefaultMetallic { get; private set; }
        [field: SerializeField] public float DefaultSmoothness { get; private set; }
        [field: SerializeField] public float DefaultMetallicForGhost { get; private set; }
        [field: SerializeField] public float DefaultSmoothnessForGhost { get; private set; }
        [field: Space(25)]
        
        [field: Header("General material pool")]
        [field: SerializeField] public Material DissolveNonUV { get; private set; }
        [field: SerializeField] public Material DissolveAndHitWithUV { get; private set; }
        [field: SerializeField] public Material DissolveAndEmissionFullObjectGhost { get; private set; }
        [field: SerializeField] public Material DissolveFromMiddleAndEmissionFullObjectGhost { get; private set; }
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

        [field: Header("General VFX pool")]
        [field: SerializeField] public VisualEffectAsset CreatingParticlesOnMeshAtTheMomentAndContinuousRandomVelocity { get; private set; }
        [field: Space(25)]

        [field: Header("Specialty VFX pool")]
        [field: SerializeField] public VisualEffectAsset ShieldAuraShieldSpawn { get; private set; }
        [field: Space(25)]

        [field: Header("Default hit effect settings")]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHitIntensiveEmissionColor { get; private set; }
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
        [field: SerializeField] public Texture2D DefaultDestroyTextureParticle { get; private set; }
        [field: SerializeField] public float DefaultDestroySizeParticles { get; private set; }
        [field: SerializeField] public int DefaultDestroyVFXNumberParticles { get; private set; }
        [field: SerializeField] public Vector3 DefaultDestroyVFXMaxVelocity { get; private set; }
        [field: SerializeField] public float DefaultDestroyVFXTurbulencePawer { get; private set; }
        [field: SerializeField] public float DefaultDestroyVFXLinearDrag { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultDestroyVFXEmissionColor { get; private set; }
        [field: Space(25)]

        [field: Header("Default hologram and dissolve settings for preview and spawn effect")]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color DefaultHologramFresnelColor { get; private set; }
        [field: SerializeField] public float DefaultHologramFresnelPower { get; private set; }
        [field: SerializeField] public float DefaultHologramEdgeWidth { get; private set; }
        [field: SerializeField] public float DefaultHologramAnimationSpeed { get; private set; }
        [field: SerializeField] public float DefaultHologramAnimationNoise { get; private set; }
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
        [field: SerializeField] public Texture2D ShieldAuraTextureParticle { get; private set; }
        [field: SerializeField] public float ShieldAuraSizeParticles { get; private set; }
        [field: SerializeField] public int ShieldAuraParticlesNumberForShieldDestroy { get; private set; }
        [field: SerializeField] public Vector3 ShieldAuraMaxVelocityParticles { get; private set; }
        [field: SerializeField] public float ShieldAuraTurbulencePawer { get; private set; }
        [field: SerializeField] public float ShieldAuraLinearDrag { get; private set; }
        [field: SerializeField] public float ShieldAuraBaseAlpha { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public float ShieldAuraHeightLowEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightStandardEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightHighEfficiency { get; private set; }
        [field: SerializeField] public float ShieldAuraHeightReallyHighEfficiency { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public float ShieldAuraDestroyStartCutoffHeight { get; private set; }
        [field: SerializeField] public float ShieldAuraDestroyFinishCutoffHeight{ get; private set; }
        [field: SerializeField] public float ShieldAuraShieldDestroyStartCutoffHeight { get; private set; }
        [field: SerializeField] public float ShieldAuraShieldDestroyFinishCutoffHeight{ get; private set; }
        [field: Space(10)]
        [field: SerializeField] public Color ShieldAuraIntensiveEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ShieldAuraEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color ShieldAuraEmissionFresnelColor { get; private set; }
        [field: SerializeField] public float ShieldAuraEmissionFresnelPower { get; private set; }
        [field: SerializeField] public float ShieldAuraVerticalNoiceScale { get; private set; }
        [field: Space(25)]

        [field: Header("AttackRangeAura visual settings")]
        [field: SerializeField] public Texture2D AttackRangeAuraTextureParticle { get; private set; }
        [field: SerializeField] public float AttackRangeAuraSizeParticles { get; private set; }
        [field: SerializeField] public int AttackRangeAuraNumberParticlesAtTheMoment { get; private set; }
        [field: SerializeField] public int AttackRangeAuraNumberParticlesContinuous { get; private set; }
        [field: SerializeField] public float AttackRangeAuraContinuousMaxDelay { get; private set; }
        [field: SerializeField] public Vector3 AttackRangeAuraMinVelocityParticles { get; private set; }
        [field: SerializeField] public Vector3 AttackRangeAuraMaxVelocityParticles { get; private set; }
        [field: SerializeField] public float AttackRangeAuraTurbulencePawer { get; private set; }
        [field: SerializeField] public float AttackRangeAuraLinearDrag { get; private set; }
        [field: SerializeField] public float AttackRangeAuraSpawnBrokenSpaceNoiseStrength { get; private set; }
        [field: SerializeField] public float AttackRangeAuraBaseAlpha { get; private set; }
        [field: Space(10)]
        [field: SerializeField] public float AttackRangeAuraSpawnBrokenSpaceStartCutoffHeight { get; private set; }
        [field: SerializeField] public float AttackRangeAuraSpawnBrokenSpaceFinishCutoffHeight{ get; private set; }
        [field: SerializeField] public float AttackRangeAuraDestroyStartCutoffHeight { get; private set; }
        [field: SerializeField] public float AttackRangeAuraDestroyFinishCutoffHeight{ get; private set; }
        [field: Space(10)]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraPositiveIntensiveEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraPositiveEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraPositiveEmissionFresnelColor { get; private set; }
        [field: SerializeField] public float AttackRangeAuraPositiveEmissionFresnelPower { get; private set; }
        [field: Space(10)]
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraNegativeIntensiveEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraNegativeEmissionColor { get; private set; }
        [field: ColorUsage(true, true)] // For HDR
        [field: SerializeField] public Color AttackRangeAuraNegativeEmissionFresnelColor { get; private set; }
        [field: SerializeField] public float AttackRangeAuraNegativeEmissionFresnelPower { get; private set; }
    }
}