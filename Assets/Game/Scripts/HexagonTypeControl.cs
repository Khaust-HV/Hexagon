using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class HexagonTypeControl : MonoBehaviour {
    [Header("HexagonLP settings")]
    [SerializeField] private GameObject _hexagonLP;
    [SerializeField] private MeshRenderer _mrHexagonLP;
    [Header("FragileHexagon settings")]
    [SerializeField] private GameObject _fragileHexagon;
    [SerializeField] private MeshRenderer[] _mrFragileHexagonParts;
    [Header("FragileHexagon settings")]
    [SerializeField] private MeshRenderer[] _mrDestroyedHexagonParts;

    #region Hexagon Config Settings
        private Material _defaultHexagonMaterial;
        private Material _shadowHexagonMaterial;
        private Material _randomHexagonMaterial;
        private Material _temporaryHexagonMaterial;
    #endregion

    public bool IsRotation { get; private set; }
    public bool IsCollapses { get; private set; }
    public bool IsFragile { get; private set; }
    public EfficiencyOfBuildingsType EfficiencyOfBuildings { get; private set; }

    [Inject]
    private void Construct(HexagonConfigs hexagonConfigs) {
        _defaultHexagonMaterial = hexagonConfigs.DefaultHexagonMaterial;
        _shadowHexagonMaterial = hexagonConfigs.ShadowHexagonMaterial;
        _randomHexagonMaterial = hexagonConfigs.RandomHexagonMaterial;
        _temporaryHexagonMaterial = hexagonConfigs.TemporaryHexagonMaterial;
    }

    public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false) {
        switch (hexagonType) {
            case HexagonType.Default:
                _hexagonLP.SetActive(true);
                _mrHexagonLP.material = _defaultHexagonMaterial;
                for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                    _mrDestroyedHexagonParts[i].material = _defaultHexagonMaterial;
                }
                IsRotation = true;
                IsCollapses = true;
                IsFragile = false;
                EfficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
            break;

            case HexagonType.Shadow:
                _hexagonLP.SetActive(true);
                _mrHexagonLP.material = _shadowHexagonMaterial;
                for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                    _mrDestroyedHexagonParts[i].material = _shadowHexagonMaterial;
                }
                IsRotation = rotateShadow;
                IsCollapses = false;
                IsFragile = false;
                if (rotateShadow) EfficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
                else EfficiencyOfBuildings = EfficiencyOfBuildingsType.Low;
            break;

            case HexagonType.Random:
                _hexagonLP.SetActive(true);
                _mrHexagonLP.material = _randomHexagonMaterial;
                for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                    _mrDestroyedHexagonParts[i].material = _randomHexagonMaterial;
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
                for (int i = 0; i < _mrFragileHexagonParts.Length; i++) {
                    _mrFragileHexagonParts[i].material = _defaultHexagonMaterial;
                }
                for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                    _mrDestroyedHexagonParts[i].material = _defaultHexagonMaterial;
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
                for (int i = 0; i < _mrFragileHexagonParts.Length; i++) {
                    _mrFragileHexagonParts[i].material = _temporaryHexagonMaterial;
                }
                for (int i = 0; i < _mrDestroyedHexagonParts.Length; i++) {
                    _mrDestroyedHexagonParts[i].material = _temporaryHexagonMaterial;
                }
                IsRotation = true;
                IsCollapses = true;
                IsFragile = true;
                EfficiencyOfBuildings = EfficiencyOfBuildingsType.VeryHigh;
            break;
        }
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