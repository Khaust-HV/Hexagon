using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour, ICameraMove, ICameraRaycast
{
    [Header("Camera raycast settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _raycastUIDistance;
    [SerializeField] private LayerMask _UILayer;
    [SerializeField] private float _raycastHexagonDistance;
    [SerializeField] private LayerMask _HexagonLayer;
    [Header("Camera move control")]
    [SerializeField] float _smoothSpeed;
    [SerializeField] private float _sensitivityMove;
    [SerializeField] private float _sensitivityZoom;
    [SerializeField] private float _timeToStopMoveing;
    [Header("Camera map borders")]
    [SerializeField] float _maxHeight;
    [SerializeField] float _minHeight;
    [SerializeField] float _westBorder;
    [SerializeField] float _eastBorder;
    [SerializeField] float _northBorder;
    [SerializeField] float _southBorder;

    private CameraState _cameraState;
    private float _currentSensitivityMove;
    private float _currentSensitivityZoom;
    private IEnumerator _iECameraMoveing;
    private bool _isIECameraMoveingActive;
    private float _currentTimeToStopMoveing;
    private bool _isTimerToStopMovementActive;
    private Vector3 _newMovePosition;
    private Vector3 _newZoomPosition;

    private void Start() {
        _newMovePosition = transform.position;
        _newZoomPosition = transform.position;

        SetSensitivityWithHeight();
    }

    private IEnumerator CameraMovementStart() {

        while (true) {
            if (_isTimerToStopMovementActive && Time.time > _currentTimeToStopMoveing) CameraMovementStop();

            switch (_cameraState) {
            case CameraState.CameraMoveing:
                MoveingCamera();
            break;

            case CameraState.CameraZooming:
                ZoomingCamera();
            break;

            case CameraState.CameraLookingOnTarget:
                LookingCamera();
            break;
            }

            yield return null;
        }
    }

    private void CameraMovementStop() {
        Debug.Log("Movement Stoped");
        StopCoroutine(_iECameraMoveing);

        _isTimerToStopMovementActive = false;
        _isIECameraMoveingActive = false;

        SwitchCameraState(CameraState.CameraIsStatic);
    }

    private void MoveingCamera() {
        Vector3 stepPosition = Vector3.Lerp(transform.position, _newMovePosition, _smoothSpeed);

        transform.position = stepPosition;
    }

    private void ZoomingCamera() {
        Vector3 stepPosition = Vector3.Lerp(transform.position, _newZoomPosition, _smoothSpeed);

        transform.position = stepPosition;

        SetSensitivityWithHeight();
    }

    private void LookingCamera() {
        //Looked on target
    }

    private void SetSensitivityWithHeight() {
        float percentageOfMaxHeight = transform.position.y * 100 / _maxHeight;

        _currentSensitivityZoom = _sensitivityZoom * percentageOfMaxHeight / 100;
        _currentSensitivityMove = _sensitivityMove * percentageOfMaxHeight / 100;
    }

    private Vector3 CheckMapBorder(Vector3 cameraPosition) {
        float x = Mathf.Clamp(cameraPosition.x, _westBorder, _eastBorder);
        float y = Mathf.Clamp (cameraPosition.y, _minHeight, _maxHeight);
        float z = Mathf.Clamp(cameraPosition.z, _southBorder, _northBorder);

        return new Vector3(x, y, z);
    }

    public void SetNewMovePosition(Vector3 vec3) {        
        _newMovePosition = CheckMapBorder(_newMovePosition + vec3 * _currentSensitivityMove * Time.deltaTime);
    }

    public void SetNewZoomPosition(Vector3 vec3) {
        _newZoomPosition = CheckMapBorder(_newZoomPosition + vec3 * _currentSensitivityZoom * Time.deltaTime);
    }

    public void SwitchCameraState(CameraState cameraState) {
        Debug.Log("Current State = " + cameraState);

        _newMovePosition = transform.position;
        _newZoomPosition = transform.position;

        _cameraState = cameraState;
    }

    public void SetCameraMovementActive(bool isActive) {
        if (isActive) {
            _isTimerToStopMovementActive = false;

            if (!_isIECameraMoveingActive) {
                Debug.Log("Movement Started");
                StartCoroutine(_iECameraMoveing = CameraMovementStart());
                _isIECameraMoveingActive = true;
            }
        }
        else {
            if (!_isTimerToStopMovementActive) {
                _currentTimeToStopMoveing = Time.time + _timeToStopMoveing;
                _isTimerToStopMovementActive = true;
            } 
        }
    }

    public bool ScreenPositionIntoRayFromCamera(Vector2 position, RaycastCheckTargetType checkType, out RaycastHit hit) {
        Ray ray = _camera.ScreenPointToRay(position);

        Debug.DrawRay(ray.origin, ray.direction * _raycastHexagonDistance, Color.red, 10f); // FIX IT !
        
        switch (checkType) {
            case RaycastCheckTargetType.CheckHexagon:
                return Physics.Raycast(ray, out hit, _raycastHexagonDistance, _HexagonLayer);
            // break;

            case RaycastCheckTargetType.CheckUI:
                return Physics.Raycast(ray, out hit, _raycastUIDistance, _UILayer);
            // break;

            default: 
                hit = new RaycastHit();
                return false;
            // break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(_westBorder, 0, _northBorder);
        Vector3 topRight = new Vector3(_eastBorder, 0, _northBorder);
        Vector3 bottomLeft = new Vector3(_westBorder, 0, _southBorder);
        Vector3 bottomRight = new Vector3(_eastBorder, 0, _southBorder);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}

public interface ICameraMove {
    public void SwitchCameraState(CameraState cameraState);
    public void SetCameraMovementActive(bool isActive);
    public void SetNewMovePosition(Vector3 vec3);
    public void SetNewZoomPosition(Vector3 vec3);
}

public interface ICameraRaycast {
    public bool ScreenPositionIntoRayFromCamera(Vector2 position, RaycastCheckTargetType checkType, out RaycastHit hit);
}

public enum RaycastCheckTargetType {
    CheckHexagon,
    CheckUI
}

public enum CameraState {
    CameraIsStatic,
    CameraMoveing,
    CameraZooming,
    CameraLookingOnTarget,
}