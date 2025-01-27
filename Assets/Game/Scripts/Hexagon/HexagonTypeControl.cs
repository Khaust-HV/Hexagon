using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonControl {
    public sealed class HexagonTypeControl : MonoBehaviour {
        [Header("HexagonLP settings")]
        [SerializeField] private GameObject _hexagonLP;
        [Header("FragileHexagon settings")]
        [SerializeField] private GameObject _fragileHexagon;
        [SerializeField] private MeshRenderer[] _mrFragileHexagonParts;
        [Header("FragileHexagon settings")]
        [SerializeField] private MeshRenderer[] _mrDestroyedHexagonParts;

        private MeshRenderer _mrHexagonLP;

        public bool IsRotation { get; private set; }
        public bool IsCollapses { get; private set; }
        public bool IsFragile { get; private set; }
        public AuraEfficiencyType AuraEfficiency { get; private set; }

        public MaterialPropertyBlock MaterialPropertyBlock { get; private set; }

        #region DI
            private HexagonConfigs _hexagonConfigs;
        #endregion

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs, VisualEffectsConfigs visualEffectsConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;
            // Set component
            _mrHexagonLP = _hexagonLP.GetComponent<MeshRenderer>();

            Material material = visualEffectsConfigs.DissolveNonUV;
            MaterialPropertyBlock = new MaterialPropertyBlock();
            MaterialPropertyBlock.SetFloat("_Metallic", visualEffectsConfigs.DefaultMetallic);
            MaterialPropertyBlock.SetFloat("_Smoothness", visualEffectsConfigs.DefaultSmoothness);

            _mrHexagonLP.material = material;
            _mrHexagonLP.SetPropertyBlock(MaterialPropertyBlock);

            foreach (var mrFragileHexagonPart in _mrFragileHexagonParts) {
                mrFragileHexagonPart.material = material;
                mrFragileHexagonPart.SetPropertyBlock(MaterialPropertyBlock);
            }

            foreach (var mrDestroyedHexagonPart in _mrDestroyedHexagonParts) {
                mrDestroyedHexagonPart.material = material;
                mrDestroyedHexagonPart.SetPropertyBlock(MaterialPropertyBlock);
            }
        }

        public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false) {
            switch (hexagonType) {
                case HexagonType.Default:
                    MaterialPropertyBlock.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                    AuraEfficiency = AuraEfficiencyType.StandardEfficiency;
                break;

                case HexagonType.Shadow:
                    MaterialPropertyBlock.SetColor("_BaseColor", _hexagonConfigs.ShadowHexagonColor);
                    IsRotation = rotateShadow;
                    IsCollapses = false;
                    IsFragile = false;
                    if (rotateShadow) AuraEfficiency = AuraEfficiencyType.StandardEfficiency;
                    else AuraEfficiency = AuraEfficiencyType.LowEfficiency;
                break;

                case HexagonType.Random:
                    MaterialPropertyBlock.SetColor("_BaseColor", _hexagonConfigs.RandomHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                    AuraEfficiency = AuraEfficiencyType.HighEfficiency;
                break;

                case HexagonType.Fragile:
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    MaterialPropertyBlock.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                    AuraEfficiency = AuraEfficiencyType.HighEfficiency;
                break;

                case HexagonType.Temporary:
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    MaterialPropertyBlock.SetColor("_BaseColor", _hexagonConfigs.TemporaryHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                    AuraEfficiency = AuraEfficiencyType.ReallyHighEfficiency;
                break;
            }

            MaterialPropertyBlock.SetFloat("_CutoffHeight", 1f);

            _mrHexagonLP.SetPropertyBlock(MaterialPropertyBlock);

            foreach (var mrFragileHexagonPart in _mrFragileHexagonParts) {
                mrFragileHexagonPart.SetPropertyBlock(MaterialPropertyBlock);
            }

            foreach (var mrDestroyedHexagonPart in _mrDestroyedHexagonParts) {
                mrDestroyedHexagonPart.SetPropertyBlock(MaterialPropertyBlock);
            }

            _hexagonLP.SetActive(true);
        }
    }

    public enum HexagonType {
        Default,
        Shadow,
        Random,
        Fragile,
        Temporary
    }
}