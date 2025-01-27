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
            protected VisualEffectsConfigs _visualEffectsConfigs;
            protected HexagonObjectConfigs _hexagonObjectConfigs;
            protected LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct (
            IStorageTransformPool iStorageTransformPool, 
            VisualEffectsConfigs visualEffectsConfigs, 
            HexagonObjectConfigs hexagonObjectConfigs,
            LevelConfigs levelConfigs
            ) {
            // Set DI
            _iStorageTransformPool = iStorageTransformPool;

            // Set configurations
            _visualEffectsConfigs = visualEffectsConfigs;
            _hexagonObjectConfigs = hexagonObjectConfigs;
            _levelConfigs = levelConfigs;

            // Set components
            _visualEffect = GetComponent<VisualEffect>();
            
            SetDestroyHexagonObjectVFXConfiguration();
        }

        private void SetDestroyHexagonObjectVFXConfiguration() {
            _visualEffect.visualEffectAsset = _visualEffectsConfigs.DestroyHexagonOrHexagonObjectVFXEffect;
            _visualEffect.SetInt("NumberParticles", _visualEffectsConfigs.DefaultDestroyVFXNumberParticles);
            _visualEffect.SetTexture("DestroyBuildTextureParticle", _levelConfigs.DefaultDestroyTextureParticle);
            _visualEffect.SetMesh("ObjectMesh", _mrBaseObject[0].GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetFloat("LifeTimeParticle", _levelConfigs.DefaultDestroyTimeAllObject);
            _visualEffect.SetFloat("SizeParticle", _levelConfigs.SizeAllObject * _levelConfigs.DefaultDestroySizeParticles);
            _visualEffect.SetVector4("EmissionColor", _visualEffectsConfigs.DefaultDestroyVFXEmissionColor);
            _visualEffect.SetVector3("StartVelocity", _visualEffectsConfigs.DefaultDestroyVFXStartVelocity);
            _visualEffect.SetFloat("LinearDrag", _visualEffectsConfigs.DefaultDestroyVFXLinearDrag);
        }

        protected virtual void SetBaseConfiguration() {
            _spawnEffectTime = _levelConfigs.DefaultSpawnTimeAllObject;

            _baseMaterial = new Material(_visualEffectsConfigs.DissolveWithUV);
            _baseMaterial.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _baseMaterial.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);

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

        public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType){
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void SpawnEffectEnable() {
            gameObject.SetActive(true);

            _baseMaterial.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultSpawnNoiseScale);
            _baseMaterial.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultSpawnNoiseStrength);
            _baseMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
            _baseMaterial.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultSpawnEdgeWidth);
            _baseMaterial.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultSpawnEdgeColor);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_baseMaterial, _spawnEffectTime));
        }

        public void HologramSpawnEffectEnable() {
            gameObject.SetActive(true);

            _hologramMaterial.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_hologramMaterial, _levelConfigs.DefaultHologramSpawnTimeAllObject));
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

            _baseMaterial.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _baseMaterial.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);
            _baseMaterial.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultDestroyEdgeWidth);
            _baseMaterial.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultDestroyEdgeColor);

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
            if (_isFastDestroy) yield return new WaitForSeconds(_levelConfigs.DefaultDestroyTimeAllObject);
            else {
                float elapsedTime = 0f;

                float destroyEffectTime = _levelConfigs.DefaultDestroyTimeAllObject;

                while (elapsedTime <destroyEffectTime) {
                    float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime / destroyEffectTime);

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
                _hologramMaterial = new Material(_visualEffectsConfigs.HologramAndDissolve);
                _hologramMaterial.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
                _hologramMaterial.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
                _hologramMaterial.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultSpawnNoiseScale);
                _hologramMaterial.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultSpawnNoiseStrength);
                _hologramMaterial.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultHologramEdgeWidth);
                _hologramMaterial.SetFloat("_AnimationSpeed", _visualEffectsConfigs.DefaultHologramAnimationSpeed);
                _hologramMaterial.SetColor("_BaseColor", _visualEffectsConfigs.DefaultHologramColor);
                _hologramMaterial.SetColor("_FresnelColor", _visualEffectsConfigs.DefaultHologramFresnelColor);
                _hologramMaterial.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultHologramEdgeColor);
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
    public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType);
    public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart);
    public void SpawnEffectEnable();
    public void DestroyEffectEnable(bool _isFastDestroy);
    public void RestoreAndHide();
    public void MakeObjectHologram();
    public void MakeObjectNormal();
    public void HologramSpawnEffectEnable();
}