using UnityEngine;

namespace Hexagon {
    public sealed class HexagonSetObjectControl : MonoBehaviour {
        [Header("Hexagon object points")]
        [SerializeField] private Transform _firstObjectPoint;
        [SerializeField] private Transform _secondObjectPoint;

        public IHexagonObjectControl CurrentObject { get; private set; }

        private IHexagonObjectControl _oldObject;

        private bool _isHexagonUpsideDown;

        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl) {
            if (_oldObject != null) _oldObject.RestoreAndHide();

            if (CurrentObject != null) _oldObject = CurrentObject;

            CurrentObject = iHexagonObjectControl;

            if (_isHexagonUpsideDown) CurrentObject.SetParentObject(_firstObjectPoint);
            else CurrentObject.SetParentObject(_secondObjectPoint);

            _isHexagonUpsideDown = !_isHexagonUpsideDown;

            if (_oldObject != null) _oldObject.SetObjectActive(false);
            CurrentObject.SetObjectActive(true);
        }
    }
}