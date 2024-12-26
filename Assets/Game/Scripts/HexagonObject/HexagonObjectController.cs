using System;
using LevelObjectsPool;
using UnityEngine;
using Zenject;

public sealed class HexagonObjectController : MonoBehaviour, IHexagonObjectControl {
    private bool _isHexagonObjectActive;
    private bool _isItImprovedYet;
    private Enum _hexagonObjectType;
    private IHexagonObjectElement _mainObject;
    private IHexagonObjectElement _decorationObject;

    #region DI
        IStorageTransformPool _iStorageTransformPool;
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

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void SetHexagonObjectType(Enum type) {
        _hexagonObjectType = type;

        // Set _isItImprovedYet haw true, if improvements are not available at this hexagonObject
    }

    public Enum GetHexagonObjectType() {
        return _hexagonObjectType;
    }

    public bool IsItImprovedYet() {
        return _isItImprovedYet;
    }

    public void SetMainObject(IHexagonObjectElement mainObject) {
        _mainObject = mainObject;

        _mainObject.SpawnEffectFinished += MainHexagonObjectWorkStart;
        _mainObject.DestroyEffectFinished += RestoreAndHide;

        _mainObject.SetParentObject(transform);
    }

    public void SetDecorationObject(IHexagonObjectElement decorationObject) {
        _decorationObject = decorationObject;

        _decorationObject.SetParentObject(transform);
    }

    public void SetObjectActive(bool isActive) {
        if (isActive) {
            _mainObject.SpawnEffectEnable();
            _decorationObject.SpawnEffectEnable();
        } else {
            _mainObject.StopAllActions();
            _mainObject.SetHexagonObjectWorkActive(false);
            _mainObject.DestroyEffectEnable();

            _decorationObject.StopAllActions();
            _decorationObject.SetHexagonObjectWorkActive(false);
            _decorationObject.DestroyEffectEnable();
        }
    }

    private void MainHexagonObjectWorkStart() {
        _mainObject.SpawnEffectFinished -= MainHexagonObjectWorkStart;

        _mainObject.SetHexagonObjectWorkActive(true);
        _decorationObject.SetHexagonObjectWorkActive(true);
    }

    private void RestoreAndHide() {
        _mainObject.DestroyEffectFinished -= RestoreAndHide;

        _mainObject.RestoreAndHide();
        _decorationObject.RestoreAndHide();

        _mainObject = null;
        _decorationObject = null;

        gameObject.SetActive(false);

        transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _isHexagonObjectActive = false;
    }

    public bool IsHexagonObjectControllerActive() {
        return _isHexagonObjectActive;
    }
}

public interface IHexagonObjectControl {
    public bool IsHexagonObjectControllerActive();
    public void SetParentObject(Transform parentObject);
    public void SetHexagonObjectType(Enum type);
    public Enum GetHexagonObjectType();
    public bool IsItImprovedYet();
    public void SetDecorationObject(IHexagonObjectElement decorationObject);
    public void SetMainObject(IHexagonObjectElement mainObject);
    public void SetObjectActive(bool isActive);
}