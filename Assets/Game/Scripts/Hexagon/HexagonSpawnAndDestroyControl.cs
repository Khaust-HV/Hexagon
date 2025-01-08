using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonSpawnAndDestroyControl : MonoBehaviour {
        [Header("HexagonLP settings")]
        [SerializeField] private GameObject _hexagonLP;
        [Header("Fragile hexagon settings")]
        [SerializeField] private GameObject _fragileHexagon;
        [SerializeField] private Transform[] _trFragileHexagonParts;

        [Header("Destroyed hexagon settings")]
        [SerializeField] private GameObject _destroyedHexagon;
        [SerializeField] private Transform[] _trDestroyedHexagonParts;

        #region Hexagon Configs Settings
            // Destroy settings
            private float _timeToRestoreAndHideParts;
            private float _forcePlannedExplosion;
            private float _forceNonPlannedExplosion;
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

        public event Action RestoreHexagon;

        // HexagonLP settings
        private MeshRenderer _mrHexagonLP;

        // Fragile hexagon settings
        private Rigidbody[] _rbFragileHexagonParts;
        private MeshCollider[] _mcFragileHexagonParts;

        // Destroyed hexagon settings
        private Rigidbody[] _rbDestroyedHexagonParts;

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs, MaterialConfigs materialConfigs) {
            // Set configurations
            _timeToRestoreAndHideParts = hexagonConfigs.TimeToRestoreAndHideParts;
            _forcePlannedExplosion = hexagonConfigs.ForcePlannedExplosion;
            _forceNonPlannedExplosion = hexagonConfigs.ForceNonPlannedExplosion;

            _spawnEffectTime = materialConfigs.SpawnEffectTime;
            _noiseScaleSpawn = materialConfigs.SpawnNoiseScale;
            _noiseStrengthSpawn = materialConfigs.SpawnNoiseStrength;
            _startCutoffHeightSpawn = materialConfigs.SpawnStartCutoffHeight;
            _finishCutoffHeightSpawn = materialConfigs.SpawnFinishCutoffHeight;
            _edgeWidthSpawn = materialConfigs.SpawnEdgeWidth;
            _edgeColorSpawn = materialConfigs.SpawnEdgeColor;

            _destroyEffectTime = materialConfigs.DestroyEffectTime;
            _noiseScaleDestroy = materialConfigs.DestroyNoiseScale;
            _noiseStrengthDestroy = materialConfigs.DestroyNoiseStrength;
            _startCutoffHeightDestroy = materialConfigs.DestroyStartCutoffHeight;
            _finishCutoffHeightDestroy = materialConfigs.DestroyFinishCutoffHeight;
            _edgeWidthDestroy = materialConfigs.DestroyEdgeWidth;
            _edgeColorDestroy = materialConfigs.DestroyEdgeColor;

            // Set component
            _mrHexagonLP = _hexagonLP.GetComponent<MeshRenderer>();

            _rbFragileHexagonParts = new Rigidbody[_trFragileHexagonParts.Length];
            _mcFragileHexagonParts = new MeshCollider[_trFragileHexagonParts.Length];
            for (int i = 0; i < _trFragileHexagonParts.Length; i++) {
            _rbFragileHexagonParts[i] = _trFragileHexagonParts[i].GetComponent<Rigidbody>();
            _mcFragileHexagonParts[i] = _trFragileHexagonParts[i].GetComponent<MeshCollider>();
            }

            _rbDestroyedHexagonParts = new Rigidbody[_trDestroyedHexagonParts.Length];
            for (int i = 0; i < _trDestroyedHexagonParts.Length; i++) {
                _rbDestroyedHexagonParts[i] = _trDestroyedHexagonParts[i].GetComponent<Rigidbody>();
            }
        }

        public void SpawnEffectEnable(Material material) {
            material.SetFloat("_NoiseScale", _noiseScaleSpawn);
            material.SetFloat("_NoiseStrength", _noiseStrengthSpawn);
            material.SetFloat("_CutoffHeight", _startCutoffHeightSpawn);
            material.SetFloat("_EdgeWidth", _edgeWidthSpawn);
            material.SetColor("_EdgeColor", _edgeColorSpawn);

            StartCoroutine(SpawnEffectStarted(material));
        }

        private IEnumerator SpawnEffectStarted(Material material) {
            float elapsedTime = 0f;

            while (elapsedTime < _spawnEffectTime) {
                float currentValue = Mathf.Lerp(_startCutoffHeightSpawn, _finishCutoffHeightSpawn, elapsedTime / _spawnEffectTime);

                material.SetFloat("_CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            material.SetFloat("_CutoffHeight", _finishCutoffHeightSpawn);
        }

        public void DestroyEffectEnable(Material material) {
            material.SetFloat("_NoiseScale", _noiseScaleDestroy);
            material.SetFloat("_NoiseStrength", _noiseStrengthDestroy);
            material.SetFloat("_CutoffHeight", _startCutoffHeightDestroy);
            material.SetFloat("_EdgeWidth", _edgeWidthDestroy);
            material.SetColor("_EdgeColor", _edgeColorDestroy);

            StartCoroutine(DestroyEffectStarted(material));
        }

        private IEnumerator DestroyEffectStarted(Material material) {
            float elapsedTime = 0f;

            while (elapsedTime < _destroyEffectTime) {
                float currentValue = Mathf.Lerp(_startCutoffHeightDestroy, _finishCutoffHeightDestroy, elapsedTime / _destroyEffectTime);

                material.SetFloat("_CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            material.SetFloat("_CutoffHeight", _finishCutoffHeightDestroy);
        }

        public void DestroyPlannedHexagon() {
            _hexagonLP.SetActive(false);

            for (int i = 0; i < _mcFragileHexagonParts.Length; i++) {
                _mcFragileHexagonParts[i].enabled = true;
                _rbFragileHexagonParts[i].isKinematic = false;
                _rbFragileHexagonParts[i].AddExplosionForce (
                    _forcePlannedExplosion, 
                    _rbDestroyedHexagonParts[i].transform.position + UnityEngine.Random.onUnitSphere * _hexagonLP.transform.localScale.x, 
                    _hexagonLP.transform.localScale.x, 
                    1f, 
                    ForceMode.Impulse
                );
            }

            StartCoroutine(DestroyParts());
        }

        public void DestroyNonPlannedHexagon() {
            _hexagonLP.SetActive(false);
            _fragileHexagon.SetActive(false);
            _destroyedHexagon.SetActive(true);

            for (int i = 0; i < _rbDestroyedHexagonParts.Length; i++) {
                _rbDestroyedHexagonParts[i].AddExplosionForce (
                    _forceNonPlannedExplosion, 
                    _rbDestroyedHexagonParts[i].transform.position + UnityEngine.Random.onUnitSphere * _hexagonLP.transform.localScale.x, 
                    _hexagonLP.transform.localScale.x, 
                    1f, 
                    ForceMode.Impulse
                );
            }
            
            StartCoroutine(DestroyParts());
        }

        private IEnumerator DestroyParts() {
            yield return new WaitForSeconds(_timeToRestoreAndHideParts);

            RestoreAndHide();
        }

        public void StopAllActions() {
            StopAllCoroutines();
        }

        private void RestoreAndHide() {
            // HexagonLP restore
            _mrHexagonLP.enabled = true;

            // FragileHexagon restore
            _fragileHexagon.SetActive(false);

            for (int i = 0; i < _trFragileHexagonParts.Length; i++) {
                _mcFragileHexagonParts[i].enabled = false;
                _rbFragileHexagonParts[i].isKinematic = true;

                _trFragileHexagonParts[i].localPosition = Vector3.zero;
                _trFragileHexagonParts[i].localRotation = Quaternion.identity;
            }

            // DestroyedHexagon restore
            _destroyedHexagon.SetActive(false);

            for (int i = 0; i < _trDestroyedHexagonParts.Length; i++) {
                _rbDestroyedHexagonParts[i].isKinematic = true;

                _trDestroyedHexagonParts[i].localPosition = Vector3.zero;
                _trDestroyedHexagonParts[i].localRotation = Quaternion.identity;
            }

            transform.position = Vector3.zero;

            gameObject.SetActive(false);

            RestoreHexagon?.Invoke();
        }
    }
}