using UnityEngine;

public sealed class HexagonObjectController : MonoBehaviour {
    
}

public interface IHexagonObjectControl {
    public bool IsHexagonObjectControllerActive();
}

public interface IHexagonObjectElement {
    public bool IsHexagonObjectElementActive();
}