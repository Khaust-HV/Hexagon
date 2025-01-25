using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonObjectControl {
    [RequireComponent(typeof(VisualEffect))]
    public class HexagonObjectAura : MonoBehaviour, IHexagonObjectPart {
        public event Action HexagonObjectPartIsRestore;

        protected Enum _hexagonObjectPartType;
        protected Enum _hexagonObjectType;

        private bool _isHexagonObjectPartUsed;
        private bool _isObjectWaitingToSpawn;

        private IEnumerator _spawnEffectStarted;

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
        }

        protected virtual void SetBaseConfiguration() {
            // Overridden by an heir
        }

        public virtual void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType) {
            // Overridden by an heir
        }

        public virtual void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
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
            gameObject.SetActive(true);

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted());
        }

        private IEnumerator SpawnEffectStarted() {
            _isObjectWaitingToSpawn = true;

            yield return new WaitForSeconds(_levelConfigs.DefaultSpawnTimeAllObject);

            SetHexagonObjectWorkActive(true);

            _isObjectWaitingToSpawn = false;
        }

        public void DestroyEffectEnable(bool _isFastDestroy) {
            SetHexagonObjectWorkActive(false);

            if (_isObjectWaitingToSpawn) {
                StopCoroutine(_spawnEffectStarted);

                _isObjectWaitingToSpawn = false;
            }

            StartCoroutine(DestroyEffectStarted());
        }

        private IEnumerator DestroyEffectStarted() {
            yield return new WaitForSeconds(_levelConfigs.DefaultDestroyTimeAllObject);

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

public enum AuraEfficiencyType {
    LowEfficiency,
    StandardEfficiency,
    HighEfficiency,
    ReallyHighEfficiency
}