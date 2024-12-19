using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hexagon {
    public sealed class HexagonUnitDetectionArea : MonoBehaviour {
        public event Action<bool> UnitDetected;

        private List<GameObject> _listUnitsInArea = new List<GameObject>();

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Unit")) {
                if (_listUnitsInArea.Count == 0) UnitDetected?.Invoke(true);
                _listUnitsInArea.Add(other.gameObject);

                Debug.Log($"Enter {other.name}, all units in the area = {_listUnitsInArea.Count}"); // FIX IT !
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Unit")) {
                _listUnitsInArea.Remove(other.gameObject);
                if (_listUnitsInArea.Count == 0) UnitDetected?.Invoke(false);

                Debug.Log($"Exit {other.name}, all units in the area = {_listUnitsInArea.Count}"); // FIX IT !
            }
        }
    }
}