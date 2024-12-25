using LevelObjectsPool;
using UnityEngine;
using Zenject;

public sealed class HexagonObjectController : MonoBehaviour, IHexagonObjectControl {
    private bool _isHexagonObjectActive;
    private IHexagonObjectElement _decorationObject;
    private IHexagonObjectElement _mainObject;

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

    public void SetDecorationObject(IHexagonObjectElement decorationObject) {
        _decorationObject = decorationObject;

        _decorationObject.SetParentObject(transform);
    }

    public void SetMainObject(IHexagonObjectElement mainObject) {
        _mainObject = mainObject;

        _mainObject.SetParentObject(transform);
    }

    public void SetObjectActive(bool isActive) {
        if (isActive) {
            _mainObject.SpawnEffectEnable();
            _decorationObject.SpawnEffectEnable();
        } else {
            _mainObject.DestroyEffectEnable();
            _decorationObject.DestroyEffectEnable();
        }
    }

    public void RestoreAndHide() {
        _mainObject.RestoreAndHide();
        _decorationObject.RestoreAndHide();

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
    public void SetDecorationObject(IHexagonObjectElement decorationObject);
    public void SetMainObject(IHexagonObjectElement mainObject);
    public void SetObjectActive(bool isActive);
    public void RestoreAndHide();
}