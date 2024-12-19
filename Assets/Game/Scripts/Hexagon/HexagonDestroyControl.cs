using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonDestroyControl : MonoBehaviour {
        [Header("HexagonLP settings")]
        [SerializeField] private GameObject _hexagonLP;
        [Header("Fragile hexagon settings")]
        [SerializeField] private GameObject _fragileHexagon;
        [SerializeField] private Transform[] _trFragileHexagonParts;

        [Header("Destroyed hexagon settings")]
        [SerializeField] private GameObject _destroyedHexagon;
        [SerializeField] private Transform[] _trDestroyedHexagonParts;

        #region Hexagon Config Settings
            private float _timeToDestroyParts;
            private float _forcePlannedExplosion;
            private float _forceNonPlannedExplosion;
        #endregion

        public event Action RestoreHexagon;

        private MeshRenderer _mrHexagonLP;

        private Rigidbody[] _rbFragileHexagonParts;
        private MeshCollider[] _mcFragileHexagonParts;

        private Rigidbody[] _rbDestroyedHexagonParts;

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            _timeToDestroyParts = hexagonConfigs.TimeToDestroyParts;
            _forcePlannedExplosion = hexagonConfigs.ForcePlannedExplosion;
            _forceNonPlannedExplosion = hexagonConfigs.ForceNonPlannedExplosion;

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
            float timePassed = 0f;

            while (timePassed < _timeToDestroyParts) {
                timePassed += Time.deltaTime;
                float percentageTime = timePassed * 100 / _timeToDestroyParts;

                // Set hide effect for parts

                yield return null;
            }

            RestoreAndHide();
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

            RestoreHexagon?.Invoke();
        }
    }
}