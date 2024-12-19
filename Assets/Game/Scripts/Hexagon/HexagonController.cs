using System;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonController : MonoBehaviour, IHexagonControl {
        [Header("Hexagon object points")]
        [SerializeField] private Transform _firstObjectPoint;
        [SerializeField] private Transform _secondObjectPoint;

        #region Hexagon Config Settings
            private int _minNumberRotationsForHexagon;
            private int _maxNumberRotationsForHexagon;
        #endregion

        public event Action CameraLooking;

        private HexagonType _hexagonType;
        private int _currentAvailableNumberRotations;
        private bool _isHexagonActive;

        private IHexagonObjectControl _currentObject;
        private IHexagonObjectControl _oldObject;

        private HexagonTypeControl _hexagonTypeControl;
        private HexagonRotationControl _hexagonRotationControl;
        private HexagonDestroyControl _hexagonDestroyControl;
        private HexagonUnitAreaControl _hexagonUnitAreaControl;

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            _minNumberRotationsForHexagon = hexagonConfigs.MinNumberRotationsForHexagon;
            _maxNumberRotationsForHexagon = hexagonConfigs.MaxNumberRotationsForHexagon;

            _hexagonTypeControl = GetComponent<HexagonTypeControl>();
            _hexagonRotationControl = GetComponent<HexagonRotationControl>();
            _hexagonDestroyControl = GetComponent<HexagonDestroyControl>();
            _hexagonUnitAreaControl = transform.GetChild(0).GetComponent<HexagonUnitAreaControl>();

            _hexagonRotationControl.HexagonRandomRotation += CheckingBeforeRotate;
            _hexagonDestroyControl.RestoreHexagon += RestoreHexagon;
            _hexagonUnitAreaControl.DestroyHexagon += DestroyHexagon;
        }

        public void SetPositionAndID(Vector3 position, int id) {
            transform.position = position;
            _hexagonUnitAreaControl.SetHexagonID(id);
        }

        public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false) {
            _hexagonType = hexagonType;
            
            gameObject.SetActive(true);

            _isHexagonActive = true;

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

            // Set new object

            _hexagonRotationControl.StartRotation();
        }

        private void DestroyHexagon(bool isPlanned) {
            StopActivity();

            CameraLooking?.Invoke(); // If a player has taken a focus but the hexagon is destroyed

            if (isPlanned) {
                _hexagonDestroyControl.DestroyPlannedHexagon();
            }
            else {
                _hexagonDestroyControl.DestroyNonPlannedHexagon();
            }
        }

        private void StopActivity() {
            _hexagonRotationControl.StopAllRotation();
        }

        private void RestoreHexagon() {
            _isHexagonActive = false;
        }

        public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl) {
            _currentObject = iHexagonObjectControl;

            //Set object parent and position
        }
    }

    public interface IHexagonControl {
        public void SetPositionAndID(Vector3 position, int id);
        public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false);
        public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl);
        public event Action CameraLooking;
    }
}