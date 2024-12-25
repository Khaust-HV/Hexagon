using System;
using System.Collections;
using GameConfigs;
using LevelObjectsPool;
using UnityEngine;
using Zenject;

public class HexagonObjectElement : MonoBehaviour, IHexagonObjectElement {
    #region HexagonObject Configs Settings
        // Spawn effect settings
        private float _spawnEffectTime;
        private float _noiseScaleSpawn;
        private float _noiseStrengthSpawn;
        private float _startCutoffHeightSpawn;
        private float _finishCutoffHeightSpawn;
        private float _edgeWidthSpawn;
        private Color _edgeColorSpawn;
        // Destroy effect settings
        private float _destroyEffectTime;
        private float _noiseScaleDestroy;
        private float _noiseStrengthDestroy;
        private float _startCutoffHeightDestroy;
        private float _finishCutoffHeightDestroy;
        private float _edgeWidthDestroy;
        private Color _edgeColorDestroy;
    #endregion
    
    protected Enum _hexagonObjectElementType;
    private bool _isHexagonObjectElementActive;
    private Material _material;
    private MeshRenderer _meshRenderer;

    #region DI
        IStorageTransformPool _iStorageTransformPool;
    #endregion

    [Inject]
    private void Construct(IStorageTransformPool iStorageTransformPool, MaterialConfigs materialConfigs) {
        // Set DI
        _iStorageTransformPool = iStorageTransformPool;

        // Set configurations
        _material = new Material(materialConfigs.SpawnAndDestroyShaderEffect);
        _material.SetFloat("_Metallic", materialConfigs.Metallic);
        _material.SetFloat("_Smoothness", materialConfigs.Smoothness);

        _spawnEffectTime = materialConfigs.SpawnEffectTime;
        _noiseScaleSpawn = materialConfigs.NoiseScaleSpawn;
        _noiseStrengthSpawn = materialConfigs.NoiseStrengthSpawn;
        _startCutoffHeightSpawn = materialConfigs.StartCutoffHeightSpawn;
        _finishCutoffHeightSpawn = materialConfigs.FinishCutoffHeightSpawn;
        _edgeWidthSpawn = materialConfigs.EdgeWidthSpawn;
        _edgeColorSpawn = materialConfigs.EdgeColorSpawn;

        _destroyEffectTime = materialConfigs.DestroyEffectTime;
        _noiseScaleDestroy = materialConfigs.NoiseScaleDestroy;
        _noiseStrengthDestroy = materialConfigs.NoiseStrengthDestroy;
        _startCutoffHeightDestroy = materialConfigs.StartCutoffHeightDestroy;
        _finishCutoffHeightDestroy = materialConfigs.FinishCutoffHeightDestroy;
        _edgeWidthDestroy = materialConfigs.EdgeWidthDestroy;
        _edgeColorDestroy = materialConfigs.EdgeColorDestroy;

        // Set component
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = _material;
    }

    public void SetParentObject(Transform parentObject) {
        _isHexagonObjectElementActive = true;

        transform.SetParent(parentObject);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(true);
    }

    public Enum GetHexagonObjectType() {
        return _hexagonObjectElementType;
    }

    public void SpawnEffectEnable() {
        _material.SetFloat("_NoiseScale", _noiseScaleSpawn);
        _material.SetFloat("_NoiseStrength", _noiseStrengthSpawn);
        _material.SetFloat("_CutoffHeight", _startCutoffHeightSpawn);
        _material.SetFloat("_EdgeWidth", _edgeWidthSpawn);
        _material.SetColor("_EdgeColor", _edgeColorSpawn);

        StartCoroutine(SpawnEffectStarted());
    }

    private IEnumerator SpawnEffectStarted() {
        float elapsedTime = 0f;

        while (elapsedTime < _spawnEffectTime) {
            float currentValue = Mathf.Lerp(_startCutoffHeightSpawn, _finishCutoffHeightSpawn, elapsedTime / _spawnEffectTime);

            _material.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _material.SetFloat("_CutoffHeight", _finishCutoffHeightSpawn);
    }

    public void DestroyEffectEnable() {
        _material.SetFloat("_NoiseScale", _noiseScaleDestroy);
        _material.SetFloat("_NoiseStrength", _noiseStrengthDestroy);
        _material.SetFloat("_CutoffHeight", _startCutoffHeightDestroy);
        _material.SetFloat("_EdgeWidth", _edgeWidthDestroy);
        _material.SetColor("_EdgeColor", _edgeColorDestroy);

        StartCoroutine(DestroyEffectStarted());
    }

    private IEnumerator DestroyEffectStarted() {
        float elapsedTime = 0f;

        while (elapsedTime < _destroyEffectTime) {
            float currentValue = Mathf.Lerp(_startCutoffHeightDestroy, _finishCutoffHeightDestroy, elapsedTime / _destroyEffectTime);

            _material.SetFloat("_CutoffHeight", currentValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _material.SetFloat("_CutoffHeight", _finishCutoffHeightDestroy);
    }

    public void StopAllActions() {
        StopAllCoroutines();
    }

    public void RestoreAndHide() {
        gameObject.SetActive(false);

        transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _material.SetFloat("_CutoffHeight", 1f);

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
    public Enum GetHexagonObjectType();
}