using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonUnitAreaControl : MonoBehaviour {
        #region Hexagon Config Settings
            private float _minTimeUnitInAreaForHexagon;
            private float _maxTimeUnitInAreaForHexagon;
        #endregion

        public event Action<bool> DestroyHexagon;

        private HexagonUnitDetectionArea _hexagonUnitDetectionArea;
        private SphereCollider _scUnitsArea;
        private IEnumerator _iEDestroyBecauseSquad;

        public int HexagonID { get; private set; }

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            _minTimeUnitInAreaForHexagon = hexagonConfigs.MinTimeUnitInAreaForHexagon;
            _maxTimeUnitInAreaForHexagon = hexagonConfigs.MaxTimeUnitInAreaForHexagon;

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
            float timeToDestroy = UnityEngine.Random.Range(_minTimeUnitInAreaForHexagon, _maxTimeUnitInAreaForHexagon);

            yield return new WaitForSeconds(timeToDestroy);

            DestroyHexagon?.Invoke(true);
        }
    }
}