using UnityEngine;

public sealed class HexagonObjectController : MonoBehaviour, IHexagonObjectControl {
    private bool _isHexagonObjectActive;
    
    public bool IsHexagonObjectControllerActive() {
        return _isHexagonObjectActive;
    }
}

public interface IHexagonObjectControl {
    public bool IsHexagonObjectControllerActive();
}

public interface IHexagonObjectElement {
    public bool IsHexagonObjectElementActive();
}