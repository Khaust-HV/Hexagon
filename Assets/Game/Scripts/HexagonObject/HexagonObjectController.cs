using System;
using UnityEngine;
using Zenject;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class HexagonObjectController : MonoBehaviour, IHexagonObjectControl {
        private bool _isHexagonObjectActive;
        private bool _isItImprovedYet;
        
        public event Action HexagonControllerIsRestore;
        private Enum _hexagonObjectType;

        private IHexagonObjectElement _mainObject;
        private IHexagonObjectElement _decorationObject;
        private IHexagonObjectElement _hologramObject;

        #region DI
            private IStorageTransformPool _iStorageTransformPool;
        #endregion

        [Inject]
        private void Construct(IStorageTransformPool iStorageTransformPool) {
            // Set DI
            _iStorageTransformPool = iStorageTransformPool;
        }

        public void SetParentObject(Transform parentObject) {
            _isHexagonObjectActive = true;

            gameObject.SetActive(true);

            transform.SetParent(parentObject);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void SetHexagonObjectType(Enum type) {
            _hexagonObjectType = type;

            switch (_hexagonObjectType) {
                
                case HeapHexagonObjectsType:
                case RiverHexagonObjectsType:
                case UnBuildebleFieldHexagonObjectsType:
                case CoreHexagonObjectsType:
                    _isItImprovedYet = true;
                break;

                case BuildebleFieldHexagonObjectsType:
                case MineHexagonObjectsType:
                    _isItImprovedYet = false;
                break;
            }
        }

        public Enum GetHexagonObjectType() {
            return _hexagonObjectType;
        }

        public bool IsItImprovedYet() {
            return _isItImprovedYet;
        }

        public void SetMainObject(IHexagonObjectElement mainObject) {
            _mainObject = mainObject;

            _mainObject.HexagonObjectElementIsRestore += MainObjectIsRestore;

            _mainObject.SetParentObject(transform);
        }

        public void SetDecorationObject(IHexagonObjectElement decorationObject) {
            _decorationObject = decorationObject;

            _decorationObject.HexagonObjectElementIsRestore += DecorationObjectIsRestore;

            _decorationObject.SetParentObject(transform);
        }
        
        public void SetHologramObject(IHexagonObjectElement hologramObject) {
            RestoreHologramObject();

            _hologramObject = hologramObject;

            _hologramObject.SetParentObject(transform);

            _hologramObject.MakeObjectHologram();

            _hologramObject.HologramSpawnEffectEnable();
        }

        public void SetMainObjectFromHologramObject() {
            if (_mainObject != null) _mainObject.DestroyEffectEnable();

            _hologramObject.MakeObjectNormal();

            _mainObject = _hologramObject;

            _hologramObject = null;

            _mainObject.SpawnEffectEnable();
        }

        public void RestoreHologramObject() {
            if (_hologramObject != null) {
                _hologramObject.RestoreAndHide();

                _hologramObject = null;
            }
        }

        public void SetObjectActive(bool isActive) {
            switch (_hexagonObjectType) {
                case MineHexagonObjectsType:
                case HeapHexagonObjectsType:
                    if (isActive) {
                        _mainObject.SpawnEffectEnable();
                        _decorationObject.SpawnEffectEnable();
                    } else {
                        _mainObject.DestroyEffectEnable();

                        _decorationObject.DestroyEffectEnable();

                        RestoreHologramObject();
                    }
                break;

                case UnBuildebleFieldHexagonObjectsType:
                case CoreHexagonObjectsType:
                case RiverHexagonObjectsType:
                    if (isActive) {
                        _mainObject.SpawnEffectEnable();
                    } else {
                        _mainObject.DestroyEffectEnable();

                        RestoreHologramObject();
                    }
                break;

                case BuildebleFieldHexagonObjectsType:
                    if (isActive) {
                        _decorationObject.SpawnEffectEnable();
                    } else {
                        if (_mainObject != null) {
                            _mainObject.DestroyEffectEnable();
                        }

                        _decorationObject.DestroyEffectEnable();

                        RestoreHologramObject();
                    }
                break;
            }
        }

        private void MainObjectIsRestore() {
            _mainObject.HexagonObjectElementIsRestore -= MainObjectIsRestore;

            _mainObject = null;

            RestoreAndHide();
        }

        private void DecorationObjectIsRestore() {
            _decorationObject.HexagonObjectElementIsRestore -= DecorationObjectIsRestore;

            _decorationObject = null;

            RestoreAndHide();
        }

        private void RestoreAndHide() {
            if (_mainObject != null || _decorationObject != null) return;

            gameObject.SetActive(false);

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            HexagonControllerIsRestore?.Invoke();

            _isHexagonObjectActive = false;
        }

        public bool IsHexagonObjectControllerActive() {
            return _isHexagonObjectActive;
        }
    }
}

public interface IHexagonObjectControl {
    public event Action HexagonControllerIsRestore;
    public bool IsHexagonObjectControllerActive();
    public void SetParentObject(Transform parentObject);
    public void SetHexagonObjectType(Enum type);
    public Enum GetHexagonObjectType();
    public bool IsItImprovedYet();
    public void SetMainObject(IHexagonObjectElement mainObject);
    public void SetDecorationObject(IHexagonObjectElement decorationObject);
    public void SetHologramObject(IHexagonObjectElement hologramObject);
    public void SetMainObjectFromHologramObject();
    public void RestoreHologramObject();
    public void SetObjectActive(bool isActive);
}