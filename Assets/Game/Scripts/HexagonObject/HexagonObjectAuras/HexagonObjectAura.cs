using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonObjectControl {
    public class HexagonObjectAura : MonoBehaviour, IHexagonObjectPart {
        public event Action HexagonObjectPartIsRestored;
        public event Action HexagonObjectPartIsDestroyed;

        protected Enum _hexagonObjectPartType;
        protected Enum _hexagonObjectType;

        private bool _isHexagonObjectPartUsed;
        private bool _isObjectWaitingToSpawn;

        protected AuraEfficiencyType _auraEfficiencyType;

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

        protected virtual void ApplyAuraEfficiency() {
            // Overridden by an heir
        }

        protected virtual void SetConfigurationFromHexagonObjectType() {
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

        public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType) {
            _auraEfficiencyType = auraEfficiencyType;

            ApplyAuraEfficiency();
        }

        public void SetHexagonObjectPartType<T>(T type) where T : Enum {
            _hexagonObjectPartType = type;

            SetBaseConfiguration();
        }

        public void SetHexagonObjectType<T>(T type) where T : Enum {
            _hexagonObjectType = type;

            SetConfigurationFromHexagonObjectType();
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
            if (_isObjectWaitingToSpawn) {
                StopCoroutine(_spawnEffectStarted);

                _isObjectWaitingToSpawn = false;
            } else SetHexagonObjectWorkActive(false);

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());

            StartCoroutine(DestroyEffectStarted());
        }

        private IEnumerator DestroyEffectStarted() {
            yield return new WaitForSeconds(_levelConfigs.DefaultDestroyTimeAllObject);

            RestoreAndHide();
        }

        public void RestoreAndHide() {
            gameObject.SetActive(false);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            HexagonObjectPartIsRestored?.Invoke();
            HexagonObjectPartIsDestroyed?.Invoke();

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