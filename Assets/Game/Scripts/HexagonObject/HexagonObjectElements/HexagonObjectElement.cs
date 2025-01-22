using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonObjectControl {
    [RequireComponent(typeof(VisualEffect))]
    public class HexagonObjectElement : MonoBehaviour, IHexagonObjectPart {
        [Header("Renderer settings")]
        [SerializeField] protected MeshRenderer[] _mrBaseObject;
        [SerializeField] protected bool _isObjectHaveAnimation;
        [Header("Dissolve effect settings")]
        [SerializeField] private float _spawnStartCutoffHeight;
        [SerializeField] private float _spawnFinishCutoffHeight;
        [SerializeField] private float _destroyStartCutoffHeight;
        [SerializeField] private float _destroyFinishCutoffHeight;

        public event Action HexagonObjectPartIsRestore;

        protected Enum _hexagonObjectPartType;

        protected float _spawnEffectTime;

        private bool _isHexagonObjectPartUsed;
        protected bool _isObjectHologram;
        private bool _isObjectWaitingToSpawn;

        private IEnumerator _spawnEffectStarted;

        protected Material _baseMaterial;
        private Material _hologramMaterial;

        private VisualEffect _visualEffect;

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

            // Set components
            _visualEffect = GetComponent<VisualEffect>();
            
            SetDestroyHexagonObjectVFXConfiguration();
        }

        private void SetDestroyHexagonObjectVFXConfiguration() {
            _visualEffect.visualEffectAsset = _materialConfigs.DestroyHexagonOrHexagonObjectVFXEffect;
            _visualEffect.SetInt("NumberParticles", _materialConfigs.DestroyVFXNumberParticles);
            _visualEffect.SetMesh("ParticleMesh", _materialConfigs.DestroyVFXParticleMesh);
            _visualEffect.SetMesh("ObjectMesh", _mrBaseObject[0].GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetFloat("MinParticleLifeTime", _materialConfigs.DestroyVFXMinParticleLifeTime);
            _visualEffect.SetFloat("MaxParticleLifeTime", _materialConfigs.DestroyVFXMaxParticleLifeTime);
            _visualEffect.SetFloat("ObjectSize", transform.localScale.x);
            _visualEffect.SetFloat("Metallic", _materialConfigs.BaseMetallic);
            _visualEffect.SetFloat("Smoothness", _materialConfigs.BaseSmoothness);
            _visualEffect.SetFloat("NoiseScale", _materialConfigs.SpawnNoiseScale);
            _visualEffect.SetFloat("NoiseStrength", _materialConfigs.SpawnNoiseStrength);
            _visualEffect.SetFloat("CutoffHeight", _materialConfigs.DestroyVFXCutoffHeight);
            _visualEffect.SetFloat("EdgeWidth", _materialConfigs.SpawnEdgeWidth);
            _visualEffect.SetVector4("EdgeColor", _materialConfigs.SpawnEdgeColor);
        }

        protected virtual void SetBaseConfiguration() {
            _spawnEffectTime = _materialConfigs.SpawnEffectTime;

            _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUV);
            _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
            _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }
        }

        protected virtual void SetAnimationActive(bool isActive) {
            // Overridden by an heir
        }

        protected virtual void SetHexagonObjectWorkActive(bool isActive) {
            // Overridden by an heir
        }

        public void SetHexagonObjectPartType<T>(T type) where T : Enum {
            _hexagonObjectPartType = type;

            SetBaseConfiguration();
        }

        public void SetHexagonObjectType<T>(T type) where T : Enum {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void SetParentObject(Transform parentObject) {
            _isHexagonObjectPartUsed = true;

            transform.SetParent(parentObject);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void SetPowerTheAura(float power){
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void SpawnEffectEnable() {
            gameObject.SetActive(true);

            _baseMaterial.SetFloat("_NoiseScale", _materialConfigs.SpawnNoiseScale);
            _baseMaterial.SetFloat("_NoiseStrength", _materialConfigs.SpawnNoiseStrength);
            _baseMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
            _baseMaterial.SetFloat("_EdgeWidth", _materialConfigs.SpawnEdgeWidth);
            _baseMaterial.SetColor("_EdgeColor", _materialConfigs.SpawnEdgeColor);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_baseMaterial, _spawnEffectTime));
        }

        public void HologramSpawnEffectEnable() {
            gameObject.SetActive(true);

            _hologramMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_hologramMaterial, _materialConfigs.HologramSpawnEffectTime));
        }

        private IEnumerator SpawnEffectStarted(Material material, float spawnEffectTime) {
            _isObjectWaitingToSpawn = true;

            SetAnimationActive(true);

            float elapsedTime = 0f;

            while (elapsedTime < spawnEffectTime) {
                float currentValue = Mathf.Lerp(_spawnStartCutoffHeight, _spawnFinishCutoffHeight, elapsedTime / spawnEffectTime);

                material.SetFloat("_CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            material.SetFloat("_CutoffHeight", _spawnFinishCutoffHeight);

            SetHexagonObjectWorkActive(true);

            _isObjectWaitingToSpawn = false;
        }

        public void DestroyEffectEnable(bool _isFastDestroy) {
            SetHexagonObjectWorkActive(false);

            _baseMaterial.SetFloat("_NoiseScale", _materialConfigs.DestroyNoiseScale);
            _baseMaterial.SetFloat("_NoiseStrength", _materialConfigs.DestroyNoiseStrength);
            _baseMaterial.SetFloat("_EdgeWidth", _materialConfigs.DestroyEdgeWidth);
            _baseMaterial.SetColor("_EdgeColor", _materialConfigs.DestroyEdgeColor);

            if (_isFastDestroy || _isObjectWaitingToSpawn) {
                _baseMaterial.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

                _visualEffect.SendEvent("DestroyEffect");

                if (_isObjectWaitingToSpawn) {
                    StopCoroutine(_spawnEffectStarted);

                    _isObjectWaitingToSpawn = false;
                }

                SetAnimationActive(false);

                StartCoroutine(DestroyEffectStarted(true));
            } else {
                _baseMaterial.SetFloat("_CutoffHeight", _destroyStartCutoffHeight);

                StartCoroutine(DestroyEffectStarted(false));
            }
        }

        private IEnumerator DestroyEffectStarted(bool _isFastDestroy) {
            if (_isFastDestroy) yield return new WaitForSeconds(_materialConfigs.DestroyEffectTime);
            else {
                float elapsedTime = 0f;

                while (elapsedTime < _materialConfigs.DestroyEffectTime) {
                    float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime / _materialConfigs.DestroyEffectTime);

                    _baseMaterial.SetFloat("_CutoffHeight", currentValue);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                _baseMaterial.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

                SetAnimationActive(false);
            }

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

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _hologramMaterial;
            }

            _isObjectHologram = true;
        }

        public void MakeObjectNormal() {
            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }

            _isObjectHologram = false;
        }

        public void RestoreAndHide() {
            if (_isObjectHologram) {
                foreach (var mrObject in _mrBaseObject) {
                    mrObject.material = _baseMaterial;
                }

                _isObjectHologram = false;
            }

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            HexagonObjectPartIsRestore?.Invoke();

            gameObject.SetActive(false);

            _isHexagonObjectPartUsed = false;
        }

        public bool IsHexagonObjectPartUsed() {
            return _isHexagonObjectPartUsed;
        }
    }
}

public interface IHexagonObjectPart {
    public event Action HexagonObjectPartIsRestore;
    public bool IsHexagonObjectPartUsed();
    public void SetHexagonObjectPartType<T>(T type) where T : Enum;
    public void SetHexagonObjectType<T>(T type) where T : Enum;
    public void SetParentObject(Transform parentObject);
    public void SetPowerTheAura(float power);
    public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart);
    public void SpawnEffectEnable();
    public void DestroyEffectEnable(bool _isFastDestroy);
    public void RestoreAndHide();
    public void MakeObjectHologram();
    public void MakeObjectNormal();
    public void HologramSpawnEffectEnable();
}