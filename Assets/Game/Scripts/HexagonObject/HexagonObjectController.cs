using System;
using UnityEngine;
using Zenject;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class HexagonObjectController : MonoBehaviour, IHexagonObjectControl {
        private bool _isHexagonObjectUsed;
        private bool _isItImprovedYet;
        
        public event Action HexagonControllerIsRestore;
        public event Action MainObjectInHexagonControllerIsDestroyed;

        private Enum _hexagonObjectType;

        private IHexagonObjectPart _mainObject;
        private IHexagonObjectPart _decorationObject;
        private IHexagonObjectPart _hologramObject;
        private IHexagonObjectPart _auraObject;

        #region DI
            private IStorageTransformPool _iStorageTransformPool;
        #endregion

        [Inject]
        private void Construct(IStorageTransformPool iStorageTransformPool) {
            // Set DI
            _iStorageTransformPool = iStorageTransformPool;
        }

        public void SetParentObject(Transform parentObject) {
            gameObject.SetActive(true);

            transform.SetParent(parentObject);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void SetHexagonObjectType(Enum type) {
            _hexagonObjectType = type;

            _isHexagonObjectUsed = true;

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

        public void SetMainObject(IHexagonObjectPart mainObject) {
            _mainObject = mainObject;

            _mainObject.HexagonObjectPartIsRestored += MainObjectIsRestored;
            _mainObject.HexagonObjectPartIsDestroyed += MainObjectIsDestroyed;

            _mainObject.SetParentObject(transform);
        }

        public void SetDecorationObject(IHexagonObjectPart decorationObject) {
            _decorationObject = decorationObject;

            _decorationObject.HexagonObjectPartIsRestored += DecorationObjectIsRestore;

            _decorationObject.SetParentObject(transform);
        }

        public void SetAuraObject(IHexagonObjectPart auraObject) {
            _auraObject = auraObject;

            _auraObject.HexagonObjectPartIsRestored += AuraObjectIsRestore;

            _auraObject.SetHexagonObjectType(_hexagonObjectType);

            _auraObject.SetParentObject(transform);
        }

        public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType) {
            if (_auraObject != null) _auraObject.SetAuraEfficiency(auraEfficiencyType);
        }
        
        public void SetHologramObject(IHexagonObjectPart hologramObject) {
            RestoreHologramObject();

            _hologramObject = hologramObject;

            _hologramObject.SetParentObject(transform);

            _hologramObject.MakeObjectHologram();

            _hologramObject.HologramSpawnEffectEnable();
        }

        public void SetMainObjectFromHologramObject() {
            if (_mainObject != null) {
                _mainObject.HexagonObjectPartIsRestored -= MainObjectIsRestored;
                _mainObject.HexagonObjectPartIsDestroyed -= MainObjectIsDestroyed;

                _mainObject.DestroyEffectEnable(false);
            }

            _hologramObject.MakeObjectNormal();

            _mainObject = _hologramObject;

            _mainObject.HexagonObjectPartIsRestored += MainObjectIsRestored;
            _mainObject.HexagonObjectPartIsDestroyed += MainObjectIsDestroyed;

            if (_auraObject != null) _auraObject.ApplyAuraToHexagonObjectElement(_mainObject);

            _isItImprovedYet = true;

            _hologramObject = null;

            _mainObject.SpawnEffectEnable();
        }

        private void MainObjectIsDestroyed() {
            _mainObject.HexagonObjectPartIsRestored -= MainObjectIsRestored;
            _mainObject.HexagonObjectPartIsDestroyed -= MainObjectIsDestroyed;

            _mainObject.DestroyEffectEnable(true);

            _mainObject = null;

            MainObjectInHexagonControllerIsDestroyed?.Invoke();
        }

        public void RestoreHologramObject() {
            if (_hologramObject != null) {
                _hologramObject.MakeObjectNormal();
                
                _hologramObject.RestoreAndHide();

                _hologramObject = null;
            }
        }

        public void SetObjectActive(bool isActive, bool _isFastDestroy = false) {
            switch (_hexagonObjectType) {
                case MineHexagonObjectsType:
                case HeapHexagonObjectsType:
                    if (isActive) {
                        _mainObject.SpawnEffectEnable();
                        
                        _decorationObject.SpawnEffectEnable();

                        AuraObjectSetActive(true);
                    } else {
                        if (_mainObject != null) _mainObject.DestroyEffectEnable(_isFastDestroy);

                        _decorationObject.DestroyEffectEnable(_isFastDestroy);

                        AuraObjectSetActive(false, _isFastDestroy);

                        RestoreHologramObject();
                    }
                break;

                case UnBuildebleFieldHexagonObjectsType:
                case CoreHexagonObjectsType:
                case RiverHexagonObjectsType:
                    if (isActive) {
                        _mainObject.SpawnEffectEnable();

                        AuraObjectSetActive(true);
                    } else {
                        if (_mainObject != null) _mainObject.DestroyEffectEnable(_isFastDestroy);

                        AuraObjectSetActive(false, _isFastDestroy);

                        RestoreHologramObject();
                    }
                break;

                case BuildebleFieldHexagonObjectsType:
                    if (isActive) {
                        _decorationObject.SpawnEffectEnable();

                        AuraObjectSetActive(true);
                    } else {
                        if (_mainObject != null) _mainObject.DestroyEffectEnable(_isFastDestroy);

                        _decorationObject.DestroyEffectEnable(_isFastDestroy);

                        AuraObjectSetActive(false, _isFastDestroy);

                        RestoreHologramObject();
                    }
                break;
            }
        }

        private void AuraObjectSetActive(bool isActive, bool _isFastDestroy = false) {
            if (_auraObject != null) {
                if (isActive) {
                    if (_mainObject != null) _auraObject.ApplyAuraToHexagonObjectElement(_mainObject);

                    _auraObject.SpawnEffectEnable();
                }
                else _auraObject.DestroyEffectEnable(_isFastDestroy);   
            }
        }

        private void MainObjectIsRestored() {
            _mainObject.HexagonObjectPartIsRestored -= MainObjectIsRestored;
            _mainObject.HexagonObjectPartIsDestroyed -= MainObjectIsDestroyed;

            _mainObject = null;

            RestoreAndHide();
        }

        private void DecorationObjectIsRestore() {
            _decorationObject.HexagonObjectPartIsRestored -= DecorationObjectIsRestore;

            _decorationObject = null;

            RestoreAndHide();
        }

        private void AuraObjectIsRestore() {
            _auraObject.HexagonObjectPartIsRestored -= AuraObjectIsRestore;

            _auraObject = null;

            RestoreAndHide();
        }

        private void RestoreAndHide() {
            if (_mainObject != null || _decorationObject != null || _auraObject != null) return;

            gameObject.SetActive(false);

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            HexagonControllerIsRestore?.Invoke();

            _isItImprovedYet = false;

            _isHexagonObjectUsed = false;
        }

        public bool IsHexagonObjectControllerUsed() {
            return _isHexagonObjectUsed;
        }
    }
}

public interface IHexagonObjectControl {
    public event Action HexagonControllerIsRestore;
    public event Action MainObjectInHexagonControllerIsDestroyed;
    public bool IsHexagonObjectControllerUsed();
    public void SetParentObject(Transform parentObject);
    public void SetHexagonObjectType(Enum type);
    public Enum GetHexagonObjectType();
    public bool IsItImprovedYet();
    public void SetMainObject(IHexagonObjectPart mainObject);
    public void SetDecorationObject(IHexagonObjectPart decorationObject);
    public void SetAuraObject(IHexagonObjectPart auraObject);
    public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType);
    public void SetHologramObject(IHexagonObjectPart hologramObject);
    public void SetMainObjectFromHologramObject();
    public void RestoreHologramObject();
    public void SetObjectActive(bool isActive, bool _isFastDestroy = false);
}