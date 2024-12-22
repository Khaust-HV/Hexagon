using System;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonController : MonoBehaviour, IHexagonControl {
        #region Hexagon Config Settings
            private int _minNumberRotationsForHexagon;
            private int _maxNumberRotationsForHexagon;
        #endregion

        public event Action CameraLooking;
        public event Action<int> NeedNewHexagonObject;

        private HexagonType _hexagonType;
        private int _currentAvailableNumberRotations;
        private bool _isHexagonActive;
        private bool _isHexagonAlreadyUsed;

        private HexagonTypeControl _hexagonTypeControl;
        private HexagonRotationControl _hexagonRotationControl;
        private HexagonDestroyControl _hexagonDestroyControl;
        private HexagonUnitAreaControl _hexagonUnitAreaControl;
        private HexagonSetObjectControl _hexagonSetObjectControl;

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _minNumberRotationsForHexagon = hexagonConfigs.MinNumberRotationsForHexagon;
            _maxNumberRotationsForHexagon = hexagonConfigs.MaxNumberRotationsForHexagon;

            // Set component
            _hexagonTypeControl = GetComponent<HexagonTypeControl>();
            _hexagonRotationControl = GetComponent<HexagonRotationControl>();
            _hexagonDestroyControl = GetComponent<HexagonDestroyControl>();
            _hexagonUnitAreaControl = transform.GetChild(0).GetComponent<HexagonUnitAreaControl>();
            _hexagonSetObjectControl = GetComponent<HexagonSetObjectControl>();

            _hexagonRotationControl.HexagonRandomRotation += CheckingBeforeRotate;
            _hexagonDestroyControl.RestoreHexagon += RestoreHexagon;
            _hexagonUnitAreaControl.DestroyHexagon += DestroyHexagon;
        }

        public void SetHexagonPositionAndID(Vector3 position, int id) {
            transform.position = position;
            _hexagonUnitAreaControl.SetHexagonID(id);

            _isHexagonActive = true;
            _isHexagonAlreadyUsed = true;
            gameObject.SetActive(true);
        }

        public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false) {
            _hexagonType = hexagonType;

            _hexagonTypeControl.SetHexagonType(hexagonType, rotateShadow);

            _currentAvailableNumberRotations = UnityEngine.Random.Range(_minNumberRotationsForHexagon, _maxNumberRotationsForHexagon);

            switch (hexagonType) {
                case HexagonType.Random:
                    _hexagonRotationControl.StartRandomRotation();
                break;

                case HexagonType.Fragile:
                    _hexagonUnitAreaControl.SetUnitAreaActive(true);
                break;

                case HexagonType.Temporary:
                    _hexagonRotationControl.StartRandomRotation();
                    _hexagonUnitAreaControl.SetUnitAreaActive(true);
                break;
            }
        }

        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl) {
            _hexagonSetObjectControl.SetHexagonObject(iHexagonObjectControl);

            _hexagonRotationControl.StartRotation();
        }

        private void CheckingBeforeRotate() {
            switch (_hexagonType) {
                case HexagonType.Shadow:
                    if (!_hexagonTypeControl.IsRotation) return;
                break;

                case HexagonType.Fragile:
                case HexagonType.Temporary:
                    if (_currentAvailableNumberRotations - 1 <= 0) {
                        DestroyHexagon(true);

                        return;
                    }
                    _currentAvailableNumberRotations--;
                break;
            }

            CameraLooking?.Invoke(); // If a player has taken a focus but the hexagon is rotation

            NeedNewHexagonObject?.Invoke(_hexagonUnitAreaControl.HexagonID); // Request to levelManager for a new object

            _hexagonRotationControl.StartRotation(); // FIX IT !
        }

        private void DestroyHexagon(bool isPlanned) {
            StopActivity();

            CameraLooking?.Invoke(); // If a player has taken a focus but the hexagon is destroyed

            if (isPlanned) _hexagonDestroyControl.DestroyPlannedHexagon();
            else _hexagonDestroyControl.DestroyNonPlannedHexagon();
        }

        private void StopActivity() {
            _hexagonRotationControl.StopAllRotation();

            switch (_hexagonType) {
                case HexagonType.Fragile:
                case HexagonType.Temporary:
                    _hexagonUnitAreaControl.SetUnitAreaActive(false);
                break;
            }
        }

        private void RestoreHexagon() {
            _isHexagonActive = false;
        }

        public bool IsHexagonControllerActive() {
            return _isHexagonActive;
        }

        public bool IsHexagonControllerAlreadyUsed() {
            return _isHexagonAlreadyUsed;
        }
    }

    public interface IHexagonControl {
        public void SetHexagonPositionAndID(Vector3 position, int id);
        public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false);
        public void SetHexagonObject(IHexagonObjectControl iHexagonObjectControl);
        public event Action CameraLooking;
        public bool IsHexagonControllerActive();
        public event Action<int> NeedNewHexagonObject;
        public bool IsHexagonControllerAlreadyUsed();
    }
}