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
        public EfficiencyOfBuildingsType EfficiencyOfBuildings { get; private set; }

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
            switch (hexagonType) {
                case HexagonType.Default:
                    _hexagonLP.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    _mrHexagonLP.material = material;
                    for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                        _mrDestroyedHexagonParts[i].material = material;
                    }
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                    EfficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
                break;

                case HexagonType.Shadow:
                    _hexagonLP.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.ShadowHexagonColor);
                    _mrHexagonLP.material = material;
                    for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                        _mrDestroyedHexagonParts[i].material = material;
                    }
                    IsRotation = rotateShadow;
                    IsCollapses = false;
                    IsFragile = false;
                    if (rotateShadow) EfficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
                    else EfficiencyOfBuildings = EfficiencyOfBuildingsType.Low;
                break;

                case HexagonType.Random:
                    _hexagonLP.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.RandomHexagonColor);
                    _mrHexagonLP.material = material;
                    for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                        _mrDestroyedHexagonParts[i].material = material;
                    }
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = false;
                    EfficiencyOfBuildings = EfficiencyOfBuildingsType.High;
                break;

                case HexagonType.Fragile:
                    _hexagonLP.SetActive(true);
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.DefaultHexagonColor);
                    for (int i = 0; i < _mrFragileHexagonParts.Length; i++) {
                        _mrFragileHexagonParts[i].material = material;
                    }
                    for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                        _mrDestroyedHexagonParts[i].material = material;
                    }
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                    EfficiencyOfBuildings = EfficiencyOfBuildingsType.High;
                break;

                case HexagonType.Temporary:
                    _hexagonLP.SetActive(true);
                    _mrHexagonLP.enabled = false;
                    _fragileHexagon.SetActive(true);
                    material.SetColor("_BaseColor", _hexagonConfigs.TemporaryHexagonColor);
                    for (int i = 0; i < _mrFragileHexagonParts.Length; i++) {
                        _mrFragileHexagonParts[i].material = material;
                    }
                    for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                        _mrDestroyedHexagonParts[i].material = material;
                    }
                    IsRotation = true;
                    IsCollapses = true;
                    IsFragile = true;
                    EfficiencyOfBuildings = EfficiencyOfBuildingsType.VeryHigh;
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

    public enum EfficiencyOfBuildingsType {
        Low,
        Standard,
        High,
        VeryHigh
    }
}