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

        #region DI
            private HexagonConfigs _hexagonConfigs;
        #endregion

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;
            // Set component
            _mrHexagonLP = _hexagonLP.GetComponent<MeshRenderer>();
        }

        public void SetHexagonType(Material material, HexagonType hexagonType, bool rotateShadow = false) {
            _mrHexagonLP.material = material;

            foreach (var mrFragileHexagonPart in _mrFragileHexagonParts) {
                mrFragileHexagonPart.material = material;
            }

            foreach (var mrDestroyedHexagonPart in _mrDestroyedHexagonParts) {
                mrDestroyedHexagonPart.material = material;
            }

            _hexagonLP.SetActive(true);

            switch (hexagonType) {
                case HexagonType.Default:
                    material.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                break;

                case HexagonType.Shadow:
                    material.SetColor("_BaseColor", _hexagonConfigs.ShadowHexagonColor);
                    IsRotation = rotateShadow;
                    IsCollapses = false;
                    IsFragile = false;
                break;

                case HexagonType.Random:
                    material.SetColor("_BaseColor", _hexagonConfigs.RandomHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                break;

                case HexagonType.Fragile:
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                break;

                case HexagonType.Temporary:
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.TemporaryHexagonColor);
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                break;
            }
            material.SetFloat("_CutoffHeight", 1f);
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