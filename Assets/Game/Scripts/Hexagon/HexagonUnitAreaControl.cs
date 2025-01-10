using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonControl {
    public sealed class HexagonUnitAreaControl : MonoBehaviour {
        public event Action<bool> DestroyHexagon;

        private HexagonUnitDetectionArea _hexagonUnitDetectionArea;
        private SphereCollider _scUnitsArea;
        private IEnumerator _iEDestroyBecauseSquad;

        public int HexagonID { get; private set; }

        #region DI
            private HexagonConfigs _hexagonConfigs;
        #endregion

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;

            // Set component
            _hexagonUnitDetectionArea = transform.GetChild(0).GetComponent<HexagonUnitDetectionArea>();
            _hexagonUnitDetectionArea.UnitDetected += IsUnitsInAreaDetected;
            _scUnitsArea = _hexagonUnitDetectionArea.GetComponent<SphereCollider>();
        }

        public void SetHexagonID(int hexagonID) {
            HexagonID = hexagonID;
        }

        public void SetUnitAreaActive(bool isActive) {
            if (isActive) {
                _scUnitsArea.enabled = true;
            } else {
                _scUnitsArea.enabled = false;
                if (_iEDestroyBecauseSquad != null) IsUnitsInAreaDetected(false);
            }
        }

        private void IsUnitsInAreaDetected(bool isDetected) {
            if (isDetected) {
                StartCoroutine(_iEDestroyBecauseSquad = DestroyBecauseSquad());
            } else {
                StopCoroutine(_iEDestroyBecauseSquad);
            }
        }

        private IEnumerator DestroyBecauseSquad() {
            float timeToDestroy = UnityEngine.Random.Range(_hexagonConfigs.MinTimeUnitInAreaForHexagon, _hexagonConfigs.MaxTimeUnitInAreaForHexagon);

            yield return new WaitForSeconds(timeToDestroy);

            DestroyHexagon?.Invoke(true);
        }
    }
}