using LevelObjectsPool;
using UnityEngine;
using Zenject;

public class HexagonObjectElement : MonoBehaviour, IHexagonObjectElement {
    private bool _isHexagonObjectElementActive;

    #region DI
        IStorageTransformPool _iStorageTransformPool;
    #endregion

    [Inject]
    private void Construct(IStorageTransformPool iStorageTransformPool) {
        // Set DI
        _iStorageTransformPool = iStorageTransformPool;
    }

    public void SetParentObject(Transform parentObject) {
        _isHexagonObjectElementActive = true;

        transform.SetParent(parentObject);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void SpawnEffectEnable() {
        
    }

    public void DestroyEffectEnable() {

    }

    public void RestoreAndHide() {
        gameObject.SetActive(false);

        transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Set default progress effects

        _isHexagonObjectElementActive = false;
    }

    public bool IsHexagonObjectElementActive() {
        return _isHexagonObjectElementActive;
    }
}

public interface IHexagonObjectElement {
    public bool IsHexagonObjectElementActive();
    public void SetParentObject(Transform parentObject);
    public void SpawnEffectEnable();
    public void DestroyEffectEnable();
    public void RestoreAndHide();
}