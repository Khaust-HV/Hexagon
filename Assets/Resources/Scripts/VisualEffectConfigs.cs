using UnityEngine;
using UnityEngine.VFX;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/VisualEffectConfigs", fileName = "VisualEffectConfigs")]
    public sealed class VisualEffectConfigs : ScriptableObject {
        [field: Header("Destroy hexagon or hexagonObject VFX effect settings")]
        [field: SerializeField] public VisualEffectAsset DestroyHexagonOrHexagonObjectVFXEffect { get; private set; }
        [field: SerializeField] public float DestroyVFXCutoffHeight { get; private set; }
        [field: SerializeField] public int DestroyVFXNumberParticles { get; private set; }
        [field: SerializeField] public Mesh DestroyVFXParticleMesh { get; private set; }
        [field: Space(50)]

        [field: Header("ShieldAura shield spawn VFX effect settings")]
        [field: SerializeField] public VisualEffectAsset ShieldAuraShieldSpawn { get; private set; }
        [field: SerializeField] public float ShieldAuraDestroyStartCutoffHeight { get; private set; }
        [field: SerializeField] public float ShieldAuraDestroyFinishCutoffHeight { get; private set; }
        [field: SerializeField] public Mesh ShieldAuraShieldFragmentMesh { get; private set; }
        [field: SerializeField] public int ShieldAuraParticlesNumberForShieldDestroy { get; private set; }
    }
}