using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonObjectControl {
    [RequireComponent(typeof(VisualEffect))]
    public class HexagonObjectAura : MonoBehaviour, IHexagonObjectPart {
        [Header("Renderer settings")]
        [SerializeField] protected MeshRenderer[] _mrBaseObject;
        [SerializeField] protected bool _isObjectHaveAnimation;

        public event Action HexagonObjectPartIsRestore;

        protected Enum _hexagonObjectPartType;
        protected Enum _hexagonObjectType;

        private bool _isHexagonObjectPartUsed;
        private bool _isObjectWaitingToSpawn;

        private IEnumerator _spawnEffectStarted;

        protected Material _baseMaterial;

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
        }

        protected virtual void SetBaseConfiguration() {
            _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUV);
            _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
            _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }
        }

        public virtual void SetPowerTheAura(float power) {
            // Overridden by an heir
        }

        public virtual void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            // Overridden by an heir
        }

        protected virtual void AuraEffectEnable(AuraVFXEffectType auraVFXEffectType) {
            // Overridden by an heir
        }

        protected virtual void SetHexagonObjectWorkActive(bool isActive) {
            // Overridden by an heir
        }

        public bool IsHexagonObjectPartUsed() {
            return _isHexagonObjectPartUsed;
        }

        public void SetHexagonObjectPartType<T>(T type) where T : Enum {
            _hexagonObjectPartType = type;

            SetBaseConfiguration();
        }

        public void SetHexagonObjectType<T>(T type) where T : Enum {
            _hexagonObjectType = type;
        }

        public void SetParentObject(Transform parentObject) {
            _isHexagonObjectPartUsed = true;

            transform.SetParent(parentObject);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void SpawnEffectEnable() {
            foreach (var mrObject in _mrBaseObject) {
                mrObject.enabled = false;
            }

            gameObject.SetActive(true);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted());
        }

        private IEnumerator SpawnEffectStarted() {
            _isObjectWaitingToSpawn = true;

            yield return new WaitForSeconds(_materialConfigs.SpawnEffectTime);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.enabled = true;
            }

            AuraEffectEnable(AuraVFXEffectType.SpawnEffect);

            SetHexagonObjectWorkActive(true);

            _isObjectWaitingToSpawn = false;
        }

        public void DestroyEffectEnable(bool _isFastDestroy) {
            AuraEffectEnable(AuraVFXEffectType.DestroyEffect);

            SetHexagonObjectWorkActive(false);

            if (_isObjectWaitingToSpawn) {
                StopCoroutine(_spawnEffectStarted);

                _isObjectWaitingToSpawn = false;
            }

            RestoreAndHide();
        }

        public void RestoreAndHide() {
            gameObject.SetActive(false);

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            HexagonObjectPartIsRestore?.Invoke();

            _isHexagonObjectPartUsed = false;
        }

        public void MakeObjectHologram() {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Aura cannot become a hologram {gameObject.name}");
        }
        public void MakeObjectNormal() {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Aura cannot become a hologram {gameObject.name}");
        }
        public void HologramSpawnEffectEnable() {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"Aura cannot become a hologram {gameObject.name}");
        }
    }

    public enum AuraVFXEffectType {
        SpawnEffect,
        DestroyEffect
    }
}