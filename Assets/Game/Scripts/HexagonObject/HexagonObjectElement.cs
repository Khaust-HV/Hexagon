using System.Collections;
using GameConfigs;
using LevelObjectsPool;
using UnityEngine;
using Zenject;

public class HexagonObjectElement : MonoBehaviour, IHexagonObjectElement {
    #region Material Configs Settings
        private MaterialConfigs _materialConfigs;
        // Spawn effect settings
        private float _spawnEffectTime;
        private float _spawnNoiseScale;
        private float _spawnNoiseStrength;
        private float _spawnStartCutoffHeight;
        private float _spawnFinishCutoffHeight;
        private float _spawnEdgeWidth;
        private Color _spawnEdgeColor;
        // Destroy effect settings
        private float _destroyEffectTime;
        private float _destroyNoiseScale;
        private float _destroyNoiseStrength;
        private float _destroyStartCutoffHeight;
        private float _destroyFinishCutoffHeight;
        private float _destroyEdgeWidth;
        private Color _destroyEdgeColor;
        // Hologram effect settings
        private float _hologramSpawnEffectTime;
    #endregion

    private bool _isHexagonObjectElementActive;
    private bool _isObjectHologram;
    private Material _baseMaterial;
    private Material _hologramMaterial;
    private MeshRenderer _meshRenderer;

    #region DI
        IStorageTransformPool _iStorageTransformPool;
    #endregion

    [Inject]
    private void BaseConstruct(IStorageTransformPool iStorageTransformPool, MaterialConfigs materialConfigs) {
        // Set DI
        _iStorageTransformPool = iStorageTransformPool;

        // Set configurations
        _materialConfigs = materialConfigs;

        _baseMaterial = new Material(materialConfigs.DissolveShaderEffectWithUV);
        _baseMaterial.SetFloat("_Metallic", materialConfigs.BaseMetallic);
        _baseMaterial.SetFloat("_Smoothness", materialConfigs.BaseSmoothness);

        _spawnNoiseScale = materialConfigs.SpawnNoiseScale;
        _spawnNoiseStrength = materialConfigs.SpawnNoiseStrength;
        _spawnStartCutoffHeight = materialConfigs.SpawnStartCutoffHeight;
        _spawnFinishCutoffHeight = materialConfigs.SpawnFinishCutoffHeight;
        _spawnEdgeWidth = materialConfigs.SpawnEdgeWidth;
        _spawnEdgeColor = materialConfigs.SpawnEdgeColor;

        _destroyNoiseScale = materialConfigs.DestroyNoiseScale;
        _destroyNoiseStrength = materialConfigs.DestroyNoiseStrength;
        _destroyStartCutoffHeight = materialConfigs.DestroyStartCutoffHeight;
        _destroyFinishCutoffHeight = materialConfigs.DestroyFinishCutoffHeight;
        _destroyEdgeWidth = materialConfigs.DestroyEdgeWidth;
        _destroyEdgeColor = materialConfigs.DestroyEdgeColor;

        // Set hologram effect configs !

        SetTimeForEffect();

        // Set component
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = _baseMaterial;
    }

    protected virtual void SetTimeForEffect() {
        _spawnEffectTime = _materialConfigs.SpawnEffectTime;
        _destroyEffectTime = _materialConfigs.DestroyEffectTime;
    }

    protected virtual void SetHexagonObjectWorkActive(bool isActive) {
        if (_isObjectHologram) return;

        // Override and implement the object activity switching functionality
        Debug.Log($"Base WorkActive {isActive} {gameObject.name}");

        if (isActive) {

        } else {
            StopAllCoroutines();
        }
    }

    public void SetParentObject(Transform parentObject) {
        _isHexagonObjectElementActive = true;

        transform.SetParent(parentObject);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(true);
    }

    public void SpawnEffectEnable() {
        _baseMaterial.SetFloat("_NoiseScale", _spawnNoiseScale);
        _baseMaterial.SetFloat("_NoiseStrength", _spawnNoiseStrength);
        _baseMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
        _baseMaterial.SetFloat("_EdgeWidth", _spawnEdgeWidth);
        _baseMaterial.SetColor("_EdgeColor", _spawnEdgeColor);

        StartCoroutine(SpawnEffectStarted(_baseMaterial, _spawnEffectTime));
    }

    public void HologramSpawnEffectEnable() {
        _hologramMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);

        StartCoroutine(SpawnEffectStarted(_hologramMaterial, _hologramSpawnEffectTime));
    }

    private IEnumerator SpawnEffectStarted(Material material, float spawnEffectTime) {
        float elapsedTime = 0f;

        while (elapsedTime < spawnEffectTime) {
            float currentValue = Mathf.Lerp(_spawnStartCutoffHeight, _spawnFinishCutoffHeight, elapsedTime / spawnEffectTime);

            material.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        material.SetFloat("_CutoffHeight", _spawnFinishCutoffHeight);

        SetHexagonObjectWorkActive(true);
    }

    public void DestroyEffectEnable() {
        _baseMaterial.SetFloat("_NoiseScale", _destroyNoiseScale);
        _baseMaterial.SetFloat("_NoiseStrength", _destroyNoiseStrength);
        _baseMaterial.SetFloat("_CutoffHeight", _destroyStartCutoffHeight);
        _baseMaterial.SetFloat("_EdgeWidth", _destroyEdgeWidth);
        _baseMaterial.SetColor("_EdgeColor", _destroyEdgeColor);

        SetHexagonObjectWorkActive(false);

        StartCoroutine(DestroyEffectStarted());
    }

    private IEnumerator DestroyEffectStarted() {
        float elapsedTime = 0f;

        while (elapsedTime < _destroyEffectTime) {
            float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime / _destroyEffectTime);

            _baseMaterial.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _baseMaterial.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

        RestoreAndHide();
    }

    public void MakeObjectHologram() {
        if (_hologramMaterial == null) {
            _hologramMaterial = new Material(_materialConfigs.HologramAndDissolveShaderEffect);
            _hologramMaterial.SetFloat("_Metallic", _materialConfigs.HologramMetallic);
            _hologramMaterial.SetFloat("_Smoothness", _materialConfigs.HologramSmoothness);
            _hologramMaterial.SetFloat("_NoiseScale", _materialConfigs.HologramNoiseScale);
            _hologramMaterial.SetFloat("_NoiseStrength", _materialConfigs.HologramNoiseStrength);
            _hologramMaterial.SetFloat("_EdgeWidth", _materialConfigs.HologramEdgeWidth);
            _hologramMaterial.SetFloat("_AnimationSpeed", _materialConfigs.HologramAnimationSpeed);
            _hologramMaterial.SetColor("_BaseColor", _materialConfigs.HologramBaseColor);
            _hologramMaterial.SetColor("_FresnelColor", _materialConfigs.HologramFresnelColor);
            _hologramMaterial.SetColor("_EdgeColor", _materialConfigs.HologramEdgeColor);
        }

        _meshRenderer.material = _hologramMaterial;
        _isObjectHologram = true;
    }

    public void RestoreAndHide() {
        gameObject.SetActive(false);

        _meshRenderer.material = _baseMaterial;
        _isObjectHologram = false;

        transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _baseMaterial.SetFloat("_CutoffHeight", 1f);
        _hologramMaterial.SetFloat("_CutoffHeight", 1f);

        _isHexagonObjectElementActive = false;
    }

    public bool IsHexagonObjectElementActive() {
        return _isHexagonObjectElementActive;
    }
}

public interface IHexagonObjectElement {
    public bool IsHexagonObjectElementActive();
    public void SetParentObject(Transform parentObject);
    public void SpawnEffectEnable();
    public void DestroyEffectEnable();
    public void RestoreAndHide();
    public void MakeObjectHologram();
    public void HologramSpawnEffectEnable();
}