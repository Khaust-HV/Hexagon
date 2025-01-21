using System;
using UnityEngine;

namespace HexagonControl {
    public sealed class HexagonSetObjectControl : MonoBehaviour {
        [Header("Hexagon object points")]
        [SerializeField] private Transform _firstObjectPoint;
        [SerializeField] private Transform _secondObjectPoint;

        public IHexagonObjectControl CurrentObject { get; private set; }

        public event Action HexagonControllerIsRestore;

        private bool _isHexagonUpsideDown;

        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl, bool setOnTheCurrentSide = false) {
            if (CurrentObject != null) CurrentObject.SetObjectActive(false);

            CurrentObject = iHexagonObjectControl;

            if (setOnTheCurrentSide) {
                if (_isHexagonUpsideDown) CurrentObject.SetParentObject(_secondObjectPoint);
                else CurrentObject.SetParentObject(_firstObjectPoint);
            } else {
                if (_isHexagonUpsideDown) CurrentObject.SetParentObject(_firstObjectPoint);
                else CurrentObject.SetParentObject(_secondObjectPoint);

                _isHexagonUpsideDown = !_isHexagonUpsideDown;
            }

            CurrentObject.SetObjectActive(true);
        }

        public void DestroyCurrentHexagonObject() {
            if (CurrentObject != null) {
                CurrentObject.SetObjectActive(false, true);

                CurrentObject.HexagonControllerIsRestore += LastHexagonControllerIsRestore;
            }
        }

        private void LastHexagonControllerIsRestore() {
            CurrentObject.HexagonControllerIsRestore -= LastHexagonControllerIsRestore;

            HexagonControllerIsRestore?.Invoke();

            CurrentObject = null;

            _isHexagonUpsideDown = false;
        }
    }
}