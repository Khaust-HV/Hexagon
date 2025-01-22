using System;
using GameConfigs;
using HexagonControl;
using UnityEngine;
using Zenject;

namespace HexagonControl {
    public sealed class HexagonController : MonoBehaviour, IHexagonControl {
        public event Action CameraLooking;
        public event Action<IHexagonControl> NeedHexagonObject;

        private HexagonType _hexagonType;
        private int _currentAvailableNumberRotations;
        private bool _isHexagonUsed;
        private bool _isHexagonDestroyed;
        private Material _material;

        private HexagonTypeControl _hexagonTypeControl;
        private HexagonRotationControl _hexagonRotationControl;
        private HexagonSpawnAndDestroyControl _hexagonSpawnAndDestroyControl;
        private HexagonUnitAreaControl _hexagonUnitAreaControl;
        private HexagonSetObjectControl _hexagonSetObjectControl;

        #region DI
            private HexagonConfigs _hexagonConfigs;
        #endregion

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs, MaterialConfigs materialConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;

            _material = new Material(materialConfigs.DissolveShaderEffectNonUV);
            _material.SetFloat("_Metallic", materialConfigs.BaseMetallic);
            _material.SetFloat("_Smoothness", materialConfigs.BaseSmoothness);

            // Set component
            _hexagonTypeControl = GetComponent<HexagonTypeControl>();
            _hexagonRotationControl = GetComponent<HexagonRotationControl>();
            _hexagonSpawnAndDestroyControl = GetComponent<HexagonSpawnAndDestroyControl>();
            _hexagonUnitAreaControl = transform.GetChild(0).GetComponent<HexagonUnitAreaControl>();
            _hexagonSetObjectControl = GetComponent<HexagonSetObjectControl>();

            _hexagonRotationControl.HexagonRandomRotation += CheckingBeforeRotate;
            _hexagonSpawnAndDestroyControl.HexagonSpawnFinished += HexagonEnable;
            _hexagonSpawnAndDestroyControl.HexagonIsRestoreAndHide += HexagonIsRestoreAndHide;
            _hexagonSetObjectControl.HexagonControllerIsRestore += HexagonControllerIsRestore;
            _hexagonUnitAreaControl.DestroyHexagon += DestroyHexagon;
        }

        public void SetHexagonPositionAndID(Vector3 position, int id) {
            transform.position = position;
            _hexagonUnitAreaControl.SetHexagonID(id);

            _isHexagonUsed = true;
        }

        public int GetHexagonID() {
            return _hexagonUnitAreaControl.HexagonID;
        }

        public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false) {
            _hexagonType = hexagonType;

            _hexagonTypeControl.SetHexagonType(_material, hexagonType, rotateShadow);

            _currentAvailableNumberRotations = UnityEngine.Random.Range(_hexagonConfigs.MinNumberRotationsForHexagon, _hexagonConfigs.MaxNumberRotationsForHexagon);
        }

        public void SetHexagonActive(bool isActive) {
            if (isActive) {
                gameObject.SetActive(true);

                _hexagonSpawnAndDestroyControl.SpawnEffectEnable(_material);
            } else DestroyHexagon(false);
        }

        public bool SetHexagonObject (
            IHexagonObjectControl iHexagonObjectControl, 
            bool setWithoutRotation, 
            DirectionalRotationType directionalRotationType = DirectionalRotationType.Random
            ) {
            if (_hexagonRotationControl.IsHexagonRotation) return false; // Prevent set a new object during rotation

            if (setWithoutRotation) _hexagonSetObjectControl.SetHexagonObject(iHexagonObjectControl, setWithoutRotation);
            else {
                _hexagonSetObjectControl.SetHexagonObject(iHexagonObjectControl);

                _hexagonRotationControl.HexagonRotationEnable(directionalRotationType);
            }

            switch (_hexagonType) {
                case HexagonType.Default:
                    iHexagonObjectControl.SetPowerTheAura(_hexagonConfigs.StandardPower);
                break;

                case HexagonType.Shadow:
                    if (_hexagonTypeControl.IsRotation) iHexagonObjectControl.SetPowerTheAura(_hexagonConfigs.StandardPower);
                    else iHexagonObjectControl.SetPowerTheAura(_hexagonConfigs.LowPower);
                break;

                case HexagonType.Temporary:
                    iHexagonObjectControl.SetPowerTheAura(_hexagonConfigs.ReallyHighPower);
                break;

                case HexagonType.Random:
                case HexagonType.Fragile:
                    iHexagonObjectControl.SetPowerTheAura(_hexagonConfigs.HighPower);
                break;
            }

            return true;
        }

        private void HexagonEnable() {
            switch (_hexagonType) {
                case HexagonType.Random:
                    _hexagonRotationControl.RotationFromTimeEnable();
                break;

                case HexagonType.Fragile:
                    _hexagonUnitAreaControl.SetUnitAreaActive(true);
                break;

                case HexagonType.Temporary:
                    _hexagonRotationControl.RotationFromTimeEnable();
                    _hexagonUnitAreaControl.SetUnitAreaActive(true);
                break;
            }

            NeedHexagonObject?.Invoke(this); // Request to levelManager for a new object
        }

        private void CheckingBeforeRotate() {
            if (_hexagonRotationControl.IsHexagonRotation) throw new Exception($"The Hexagon {_hexagonUnitAreaControl.HexagonID} is already rotating");

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

            NeedHexagonObject?.Invoke(this); // Request to levelManager for a new object
        }

        public bool GetHexagonObjectController(out IHexagonObjectControl iHexagonObjectControl) {
            if (_hexagonSetObjectControl.CurrentObject != null) {
                iHexagonObjectControl = _hexagonSetObjectControl.CurrentObject;

                return true;
            }

            iHexagonObjectControl = null;

            return false;
        }

        private void DestroyHexagon(bool isPlanned) {
            if (_isHexagonDestroyed) return;

            _isHexagonDestroyed = true;

            CameraLooking?.Invoke(); // If a player has taken a focus but the hexagon is destroyed

            _hexagonRotationControl.StopAllActions();

            switch (_hexagonType) {
                case HexagonType.Fragile:
                case HexagonType.Temporary:
                    _hexagonUnitAreaControl.SetUnitAreaActive(false);
                break;
            }

            if (isPlanned) _hexagonSpawnAndDestroyControl.DestroyPlannedHexagon();
            else _hexagonSpawnAndDestroyControl.DestroyNonPlannedHexagon();

            if (_hexagonSetObjectControl.CurrentObject == null) _hexagonSpawnAndDestroyControl.DestroyEffectEnable(_material, false);
            else {
                _hexagonSpawnAndDestroyControl.DestroyEffectEnable(_material, true);

                _hexagonSetObjectControl.DestroyCurrentHexagonObject();
            }
        }

        private void HexagonControllerIsRestore() {
            _hexagonSpawnAndDestroyControl.RestoreAndHide();
        }

        private void HexagonIsRestoreAndHide() {
            _isHexagonDestroyed = false;
            _isHexagonUsed = false;
        }

        public bool IsHexagonControllerUsed() {
            return _isHexagonUsed;
        }
    }
}

public interface IHexagonControl {
    public event Action CameraLooking;
    public event Action<IHexagonControl> NeedHexagonObject;
    public bool IsHexagonControllerUsed();
    public void SetHexagonActive(bool isActive);
    public void SetHexagonPositionAndID(Vector3 position, int id);
    public void SetHexagonType(HexagonType hexagonType, bool rotateShadow = false);
    public int GetHexagonID();
    public bool SetHexagonObject (
        IHexagonObjectControl iHexagonObjectControl, 
        bool setWithoutRotation, 
        DirectionalRotationType directionalRotationType = DirectionalRotationType.Random
    );
    public bool GetHexagonObjectController(out IHexagonObjectControl iHexagonObjectControl);
}