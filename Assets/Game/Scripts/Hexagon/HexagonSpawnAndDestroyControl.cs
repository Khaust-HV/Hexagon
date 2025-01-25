using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonControl {
    public sealed class HexagonSpawnAndDestroyControl : MonoBehaviour {
        [Header("Dissolve effect settings")]
        [SerializeField] private float _spawnStartCutoffHeight;
        [SerializeField] private float _spawnFinishCutoffHeight;
        [SerializeField] private float _destroyStartCutoffHeight;
        [SerializeField] private float _destroyFinishCutoffHeight;
        [Header("HexagonLP settings")]
        [SerializeField] private GameObject _hexagonLP;
        [Header("Fragile hexagon settings")]
        [SerializeField] private GameObject _fragileHexagon;
        [SerializeField] private Transform[] _trFragileHexagonParts;
        [Header("Destroyed hexagon settings")]
        [SerializeField] private GameObject _destroyedHexagon;
        [SerializeField] private Transform[] _trDestroyedHexagonParts;

        public event Action HexagonSpawnFinished;
        public event Action HexagonIsRestoreAndHide;

        private bool _isObjectWaitingToSpawn;

        // HexagonLP settings
        private MeshRenderer _mrHexagonLP;

        // Fragile hexagon settings
        private Rigidbody[] _rbFragileHexagonParts;
        private MeshCollider[] _mcFragileHexagonParts;

        // Destroyed hexagon settings
        private Rigidbody[] _rbDestroyedHexagonParts;

        private VisualEffect _visualEffect;

        #region DI
            private HexagonConfigs _hexagonConfigs;
            private MaterialConfigs _materialConfigs;
            private VisualEffectConfigs _visualEffectConfigs;
            private LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct (
            HexagonConfigs hexagonConfigs, 
            MaterialConfigs materialConfigs, 
            VisualEffectConfigs visualEffectConfigs,
            LevelConfigs levelConfigs
            ) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;
            _materialConfigs = materialConfigs;
            _visualEffectConfigs = visualEffectConfigs;
            _levelConfigs = levelConfigs;

            // Set components
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

            _visualEffect = GetComponent<VisualEffect>();
            
            SetDestroyHexagonObjectVFXConfiguration();
        }

        private void SetDestroyHexagonObjectVFXConfiguration() {
            _visualEffect.visualEffectAsset = _visualEffectConfigs.DestroyHexagonOrHexagonObjectVFXEffect;
            _visualEffect.SetInt("NumberParticles", _visualEffectConfigs.DestroyVFXNumberParticles);
            _visualEffect.SetMesh("ParticleMesh", _visualEffectConfigs.DestroyVFXParticleMesh);
            _visualEffect.SetMesh("ObjectMesh", _mrHexagonLP.GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetFloat("LifeTimeParticle", _materialConfigs.DestroyEffectTime);
            _visualEffect.SetFloat("SizeParticle", _levelConfigs.HexagonSize);
            _visualEffect.SetFloat("Metallic", _materialConfigs.BaseMetallic);
            _visualEffect.SetFloat("Smoothness", _materialConfigs.BaseSmoothness);
            _visualEffect.SetFloat("NoiseScale", _materialConfigs.SpawnNoiseScale);
            _visualEffect.SetFloat("NoiseStrength", _materialConfigs.SpawnNoiseStrength);
            _visualEffect.SetFloat("CutoffHeight", _visualEffectConfigs.DestroyVFXCutoffHeight);
            _visualEffect.SetFloat("EdgeWidth", _materialConfigs.SpawnEdgeWidth);
            _visualEffect.SetVector4("EdgeColor", _materialConfigs.SpawnEdgeColor);
        }

        public void SpawnEffectEnable(Material material) {
            material.SetFloat("_NoiseScale", _materialConfigs.SpawnNoiseScale);
            material.SetFloat("_NoiseStrength", _materialConfigs.SpawnNoiseStrength);
            material.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
            material.SetFloat("_EdgeWidth", _materialConfigs.SpawnEdgeWidth);
            material.SetColor("_EdgeColor", _materialConfigs.SpawnEdgeColor);

            StartCoroutine(SpawnEffectStarted(material));
        }

        private IEnumerator SpawnEffectStarted(Material material) {
            _isObjectWaitingToSpawn = true;

            float elapsedTime = 0f;

            float spawnEffectTime = _materialConfigs.SpawnEffectTime;

            while (elapsedTime < spawnEffectTime) {
                float currentValue = Mathf.Lerp(_spawnStartCutoffHeight, _spawnFinishCutoffHeight, elapsedTime / spawnEffectTime);

                material.SetFloat("_CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            material.SetFloat("_CutoffHeight", _spawnFinishCutoffHeight);

            HexagonSpawnFinished?.Invoke();

            _isObjectWaitingToSpawn = false;
        }

        public void DestroyEffectEnable(Material material, bool isHexagonAutoRestore) {
            StopAllCoroutines();

            material.SetFloat("_NoiseScale", _materialConfigs.DestroyNoiseScale);
            material.SetFloat("_NoiseStrength", _materialConfigs.DestroyNoiseStrength);
            material.SetFloat("_EdgeWidth", _materialConfigs.DestroyEdgeWidth);
            material.SetColor("_EdgeColor", _materialConfigs.DestroyEdgeColor);

            if (_isObjectWaitingToSpawn) {
                material.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

                _visualEffect.SendEvent("DestroyEffect");

                _isObjectWaitingToSpawn = false;

                StartCoroutine(DestroyEffectStarted(material, isHexagonAutoRestore, true));
            } else {
                material.SetFloat("_CutoffHeight", _destroyStartCutoffHeight);

                _hexagonLP.SetActive(false);

                StartCoroutine(DestroyEffectStarted(material, isHexagonAutoRestore, false));
            }
        }

        private IEnumerator DestroyEffectStarted(Material material, bool isHexagonAutoRestore, bool _isFastDestroy) {
            if (_isFastDestroy) yield return new WaitForSeconds(_materialConfigs.DestroyEffectTime);
            else {
                float elapsedTime = 0f;

                float destroyEffectTime = _materialConfigs.DestroyEffectTime;

                while (elapsedTime < destroyEffectTime) {
                    float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime / destroyEffectTime);

                    material.SetFloat("_CutoffHeight", currentValue);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                material.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);
            }

            if (!isHexagonAutoRestore) RestoreAndHide();
        }

        public void DestroyPlannedHexagon() {
            for (int i = 0; i < _mcFragileHexagonParts.Length; i++) {
                _mcFragileHexagonParts[i].enabled = true;
                _rbFragileHexagonParts[i].isKinematic = false;
                _rbFragileHexagonParts[i].AddExplosionForce (
                    _hexagonConfigs.ForcePlannedExplosion, 
                    _rbDestroyedHexagonParts[i].transform.position + UnityEngine.Random.onUnitSphere * _hexagonLP.transform.localScale.x, 
                    _hexagonLP.transform.localScale.x, 
                    1f, 
                    ForceMode.Impulse
                );
            }
        }

        public void DestroyNonPlannedHexagon() {
            _fragileHexagon.SetActive(false);
            _destroyedHexagon.SetActive(true);

            for (int i = 0; i < _rbDestroyedHexagonParts.Length; i++) {
                _rbDestroyedHexagonParts[i].AddExplosionForce (
                    _hexagonConfigs.ForceNonPlannedExplosion, 
                    _rbDestroyedHexagonParts[i].transform.position + UnityEngine.Random.onUnitSphere * _hexagonLP.transform.localScale.x, 
                    _hexagonLP.transform.localScale.x, 
                    1f, 
                    ForceMode.Impulse
                );
            }
        }

        public void RestoreAndHide() {
            StopAllCoroutines();

            gameObject.SetActive(false);

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

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            HexagonIsRestoreAndHide?.Invoke();
        }
    }
}