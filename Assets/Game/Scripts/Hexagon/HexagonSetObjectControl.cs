using UnityEngine;

namespace Hexagon {
    public sealed class HexagonSetObjectControl : MonoBehaviour {
        [Header("Hexagon object points")]
        [SerializeField] private Transform _firstObjectPoint;
        [SerializeField] private Transform _secondObjectPoint;

        private IHexagonObjectControl _currentObject;
        private IHexagonObjectControl _oldObject;

        private bool _isHexagonUpsideDown;

        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl) {
            if (_oldObject != null) {/* _oldObject disassemble and hide */}

            if (_currentObject != null) _oldObject = _currentObject;

            _currentObject = iHexagonObjectControl;

            if (_isHexagonUpsideDown) {/* Set _currentObject parent and _firstObjectPoint position */}
            else {/* Set _currentObject parent and _secondObjectPoint position */}

            _isHexagonUpsideDown = !_isHexagonUpsideDown;

            if (_oldObject != null) {/* _oldObject active destroy effect */}
            // _currentObject active spawn effect
        }
    }
}