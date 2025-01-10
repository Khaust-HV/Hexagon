using UnityEngine;

namespace HexagonControl {
    public sealed class HexagonSetObjectControl : MonoBehaviour {
        [Header("Hexagon object points")]
        [SerializeField] private Transform _firstObjectPoint;
        [SerializeField] private Transform _secondObjectPoint;

        public IHexagonObjectControl CurrentObject { get; private set; }

        private bool _isHexagonUpsideDown;

        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl) {
            if (CurrentObject != null) CurrentObject.SetObjectActive(false);

            CurrentObject = iHexagonObjectControl;

            if (_isHexagonUpsideDown) CurrentObject.SetParentObject(_firstObjectPoint);
            else CurrentObject.SetParentObject(_secondObjectPoint);

            _isHexagonUpsideDown = !_isHexagonUpsideDown;

            CurrentObject.SetObjectActive(true);
        }

        public void DestroyCurrentHexagonObject() {
            if (CurrentObject != null) CurrentObject.SetObjectActive(false);
            
            _isHexagonUpsideDown = false;
        }
    }
}