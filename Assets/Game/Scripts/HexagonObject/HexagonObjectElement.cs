using System.Collections;
using GameConfigs;
using LevelObjectsPool;
using UnityEngine;
using Zenject;

public class HexagonObjectElement : MonoBehaviour, IHexagonObjectElement {
    private MaterialConfigs _materialConfigs;

    private float _spawnEffectTime;

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

        SetTimeForSpawnEffect();

        // Set component
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = _baseMaterial;
    }

    protected virtual void SetTimeForSpawnEffect() {
        _spawnEffectTime = _materialConfigs.SpawnEffectTime;
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
        _baseMaterial.SetFloat("_NoiseScale", _materialConfigs.SpawnNoiseScale);
        _baseMaterial.SetFloat("_NoiseStrength", _materialConfigs.SpawnNoiseStrength);
        _baseMaterial.SetFloat("_CutoffHeight", _materialConfigs.SpawnStartCutoffHeight);
        _baseMaterial.SetFloat("_EdgeWidth", _materialConfigs.SpawnEdgeWidth);
        _baseMaterial.SetColor("_EdgeColor", _materialConfigs.SpawnEdgeColor);

        StartCoroutine(SpawnEffectStarted(_baseMaterial, _spawnEffectTime));
    }

    public void HologramSpawnEffectEnable() {
        _hologramMaterial.SetFloat("_CutoffHeight", _materialConfigs.SpawnStartCutoffHeight);

        StartCoroutine(SpawnEffectStarted(_hologramMaterial, _materialConfigs.HologramSpawnEffectTime));
    }

    private IEnumerator SpawnEffectStarted(Material material, float spawnEffectTime) {
        float elapsedTime = 0f;

        while (elapsedTime < spawnEffectTime) {
            float currentValue = Mathf.Lerp(_materialConfigs.SpawnStartCutoffHeight, _materialConfigs.SpawnFinishCutoffHeight, elapsedTime / spawnEffectTime);

            material.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        material.SetFloat("_CutoffHeight", _materialConfigs.SpawnFinishCutoffHeight);

        SetHexagonObjectWorkActive(true);
    }

    public void DestroyEffectEnable() {
        _baseMaterial.SetFloat("_NoiseScale", _materialConfigs.DestroyNoiseScale);
        _baseMaterial.SetFloat("_NoiseStrength", _materialConfigs.DestroyNoiseStrength);
        _baseMaterial.SetFloat("_CutoffHeight", _materialConfigs.DestroyStartCutoffHeight);
        _baseMaterial.SetFloat("_EdgeWidth", _materialConfigs.DestroyEdgeWidth);
        _baseMaterial.SetColor("_EdgeColor", _materialConfigs.DestroyEdgeColor);

        SetHexagonObjectWorkActive(false);

        StartCoroutine(DestroyEffectStarted());
    }

    private IEnumerator DestroyEffectStarted() {
        float elapsedTime = 0f;

        while (elapsedTime < _materialConfigs.DestroyEffectTime) {
            float currentValue = Mathf.Lerp(_materialConfigs.DestroyStartCutoffHeight, _materialConfigs.DestroyFinishCutoffHeight, elapsedTime / _materialConfigs.DestroyEffectTime);

            _baseMaterial.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _baseMaterial.SetFloat("_CutoffHeight", _materialConfigs.DestroyFinishCutoffHeight);

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

    public void MakeObjectBase() {
        _meshRenderer.material = _baseMaterial;
        _isObjectHologram = false;
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
    public void MakeObjectBase();
    public void HologramSpawnEffectEnable();
}