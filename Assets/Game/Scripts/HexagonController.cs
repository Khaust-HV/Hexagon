using System.Collections;
using UnityEngine;

public sealed class HexagonController : MonoBehaviour, IHexagonControl {
    [Header("Hexagon settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform _firstObjectPoist;
    [SerializeField] private Transform _secondObjectPoist;

    private IHexagonObjectControl _currentObject;
    private IHexagonObjectControl _oldObject;

    public int HexagonID { get; private set; }

    public void SetPositionAndID(Vector3 position, int id) {
        transform.position = position;
        HexagonID = id;

        gameObject.SetActive(true);
    }

    public void SetHexagonType(HexagonType hexagonType) {

    }

    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl) {
        _currentObject = iHexagonObjectControl;

        //Set object parent and position
    }

    private void StartRandomRotation() {
        bool rotateAroundX = Random.Range(0, 2) == 0;
        int direction = Random.Range(0, 2) == 0 ? 1 : -1;

        Vector3 rotationAxis;

        if (rotateAroundX) {
            rotationAxis = Vector3.right * direction;
        } else {
            rotationAxis = Vector3.forward * direction;
        }

        float targetAngle = 180f * direction;

        StartCoroutine(RotateOverTime(rotationAxis, targetAngle, direction));
    }

    private IEnumerator RotateOverTime(Vector3 rotationAxis, float targetAngle, int direction) {
        float rotatedAngle = 0f;
        
        while (Mathf.Abs(rotatedAngle) < Mathf.Abs(targetAngle)) {
            float stepAngle = rotationSpeed * direction * Time.deltaTime;

            if (Mathf.Abs(rotatedAngle + stepAngle) > Mathf.Abs(targetAngle)) {
                stepAngle = targetAngle - rotatedAngle;
            }

            transform.Rotate(rotationAxis, stepAngle, Space.World);

            rotatedAngle += stepAngle;

            yield return null;
        }
    }
}

public interface IHexagonControl {
    public void SetPositionAndID(Vector3 position, int id);
    public void SetHexagonType(HexagonType hexagonType);
    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl);
}

public enum HexagonType {
    Default,
    Shadow,
    Random,
    Fragile,
    Temporary
}