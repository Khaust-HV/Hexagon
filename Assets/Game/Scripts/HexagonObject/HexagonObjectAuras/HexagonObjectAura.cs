using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonObjectControl {
    public class HexagonObjectAura : MonoBehaviour, IHexagonObjectPart {
        [Header("Renderer settings")]
        [SerializeField] protected MeshRenderer[] _mrBaseObject;
        [Header("Animation settings")]
        [SerializeField] protected bool _isObjectHaveAnimation;
        [SerializeField] protected Animator[] _animBaseObject;

        public event Action HexagonObjectPartIsRestore;

        protected Enum _hexagonObjectPartType;
        protected Enum _hexagonObjectType;

        private bool _isHexagonObjectPartUsed;

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

        protected virtual void SetHexagonObjectWorkActive(bool isActive) {
            // Override and implement the object activity switching functionality

            if (!isActive) StopAllCoroutines();
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

            gameObject.SetActive(true);
        }

        public virtual void SetPowerTheAura(float power) {
            // Overridden by an heir
        }

        public virtual void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            // Overridden by an heir
        }

        public void SpawnEffectEnable() {
            // Enable spawn effect VFX Graph

            foreach (var mrObject in _mrBaseObject) {
                mrObject.enabled = false;
            }

            gameObject.SetActive(true);

            StartCoroutine(SpawnEffectStarted());
        }

        private IEnumerator SpawnEffectStarted() {
            yield return new WaitForSeconds(_materialConfigs.SpawnEffectTime);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.enabled = true;
            }

            SetHexagonObjectWorkActive(true);
        }

        public void DestroyEffectEnable() {
            // Enable destroy effect VFX Graph

            SetHexagonObjectWorkActive(false);

            StopAllCoroutines();

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
}