using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonObjectControl {
    public class HexagonObjectElement : MonoBehaviour, IHexagonObjectElement {
        [field: SerializeField] protected MeshRenderer MRBaseObject { get; private set; }
        [field: SerializeField] protected bool IsObjectHaveAnimation { get; private set; }
        [field: SerializeField] protected Animator AnimBaseObject { get; private set; }
        [field: SerializeField] protected bool IsObjectHaveEmission { get; private set; }

        protected float _spawnEffectTime;

        private bool _isHexagonObjectElementActive;
        protected bool _isObjectHologram;

        protected Material _baseMaterial;
        private Material _hologramMaterial;

        #region DI
            private IStorageTransformPool _iStorageTransformPool;
            protected MaterialConfigs _materialConfigs;
            protected HexagonObjectConfigs _hexagonObjectConfigs;
        #endregion

        [Inject]
        private void Construct(IStorageTransformPool iStorageTransformPool, MaterialConfigs materialConfigs, HexagonObjectConfigs hexagonObjectConfigs) {
            // Set DI
            _iStorageTransformPool = iStorageTransformPool;

            // Set configurations
            _materialConfigs = materialConfigs;
            _hexagonObjectConfigs = hexagonObjectConfigs;

            SetBaseConfiguration();
        }

        protected virtual void SetBaseConfiguration() {
            _spawnEffectTime = _materialConfigs.SpawnEffectTime;

            _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUV);
            _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
            _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);
            MRBaseObject.material = _baseMaterial;
        }

        protected virtual void SetHexagonObjectWorkActive(bool isActive) {
            if (IsObjectHaveAnimation && _isObjectHologram) {
                AnimBaseObject.Play("IdleNonEffect");
                
                return;
            } else if (_isObjectHologram) return;

            // Override and implement the object activity switching functionality
            Debug.Log($"Base WorkActive {isActive} {gameObject.name}");

            if (isActive) {
                if (IsObjectHaveAnimation) AnimBaseObject.Play("IdleWithEffect");
            } else {
                StopAllCoroutines();

                if (IsObjectHaveAnimation) AnimBaseObject.Play("Nothing");
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
            if (IsObjectHaveAnimation) AnimBaseObject.Play("Nothing");

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
            SetHexagonObjectWorkActive(false);

            _baseMaterial.SetFloat("_NoiseScale", _materialConfigs.DestroyNoiseScale);
            _baseMaterial.SetFloat("_NoiseStrength", _materialConfigs.DestroyNoiseStrength);
            _baseMaterial.SetFloat("_CutoffHeight", _materialConfigs.DestroyStartCutoffHeight);
            _baseMaterial.SetFloat("_EdgeWidth", _materialConfigs.DestroyEdgeWidth);
            _baseMaterial.SetColor("_EdgeColor", _materialConfigs.DestroyEdgeColor);

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

            MRBaseObject.material = _hologramMaterial;
            _isObjectHologram = true;
        }

        public void MakeObjectBase() {
            MRBaseObject.material = _baseMaterial;
            _isObjectHologram = false;
        }

        public void RestoreAndHide() {
            gameObject.SetActive(false);

            MRBaseObject.material = _baseMaterial;
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