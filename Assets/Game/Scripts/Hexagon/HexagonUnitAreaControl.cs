using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonUnitAreaControl : MonoBehaviour {
        #region Hexagon Config Settings
            private float _minTimeSquadForHexagon;
            private float _maxTimeSquadForHexagon;
        #endregion

        public event Action<bool> DestroyHexagon;

        private HexagonUnitDetectionArea _hexagonUnitDetectionArea;
        private MeshCollider _mcUnitsArea;
        private IEnumerator _iEDestroyBecauseSquad;

        public int HexagonID { get; private set; }

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            _minTimeSquadForHexagon = hexagonConfigs.MinTimeSquadForHexagon;
            _maxTimeSquadForHexagon = hexagonConfigs.MaxTimeSquadForHexagon;

            _hexagonUnitDetectionArea = transform.GetChild(0).GetComponent<HexagonUnitDetectionArea>();
            _hexagonUnitDetectionArea.UnitDetected += IsUnitsInAreaDetected;
            _mcUnitsArea = _hexagonUnitDetectionArea.GetComponent<MeshCollider>();
        }

        public void SetHexagonID(int hexagonID) {
            HexagonID = hexagonID;
        }

        public void SetUnitAreaActive(bool isActive) {
            if (isActive) {
                _mcUnitsArea.enabled = true;
            } else {
                _mcUnitsArea.enabled = false;
                StopCoroutine(_iEDestroyBecauseSquad);
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
            float timeToDestroy = UnityEngine.Random.Range(_minTimeSquadForHexagon, _maxTimeSquadForHexagon);

            yield return new WaitForSeconds(timeToDestroy);

            DestroyHexagon?.Invoke(true);
        }
    }
}