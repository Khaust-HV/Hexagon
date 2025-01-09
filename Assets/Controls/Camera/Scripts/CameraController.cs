using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour, ICameraMove, ICameraZoom, ICameraSatelliteMovement, ICameraRaycast {
    private CameraConfigs _cameraConfigs;

    // The camera moves from the player's input
    private Camera _camera;
    private CameraState _cameraState;
    private float _currentSensitivityMove;
    private float _currentSensitivityZoom;
    private IEnumerator _iECameraMoveing;
    private bool _isIECameraMoveingActive;
    private float _currentTimeToStopMoveing;
    private bool _isTimerToStopMovementActive;
    private Vector3 _newMovePosition;
    private Vector3 _newZoomPosition;

    // The camera looks and moves toward the target 
    private Vector3 _targetPosition;
    private Vector3 _satellitePosition;
    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;
    private float _satelliteCurrentAngle;
    private IEnumerator _iESatelliteMoveing;
    private Transform _trCamera;
    public event Action CameraNearTarget;
    public event Action CameraBackToDefault;

    [Inject]
    private void Construct(CameraConfigs cameraConfigs) {
        // Set configurations
        _cameraConfigs = cameraConfigs;

        // Set component
        _camera = transform.GetChild(0).GetComponent<Camera>();
        _trCamera = _camera.GetComponent<Transform>();

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

                case CameraState.CameraMoveingToTarget:
                case CameraState.CameraOrbitingAndLookingOnTarget:
                case CameraState.CameraMoveingToDefault:
                    OrbitingCamera();
                break;
            }

            yield return null;
        }
    }

    private void CameraMovementStop() {
        Debug.Log("Movement Stoped"); // FIX IT !
        StopCoroutine(_iECameraMoveing);

        _isTimerToStopMovementActive = false;
        _isIECameraMoveingActive = false;

        SwitchCameraState(CameraState.CameraIsStatic);
    }

    private IEnumerator SatelliteMovementStart() {
        while (true) {
            _satelliteCurrentAngle += _cameraConfigs.SatelliteSpeed * Time.deltaTime;

            _satelliteCurrentAngle %= 360f;

            Vector3 stepPosition = new Vector3(
                Mathf.Cos(_satelliteCurrentAngle * Mathf.Deg2Rad) * _cameraConfigs.OrbitRadius,
                _cameraConfigs.OrbitHeight,
                Mathf.Sin(_satelliteCurrentAngle * Mathf.Deg2Rad) * _cameraConfigs.OrbitRadius
            );

            _satellitePosition = _targetPosition + stepPosition;

            yield return null;
        }
    }

    private void MoveingCamera() {
        Vector3 stepPosition = Vector3.Lerp(transform.position, _newMovePosition, _cameraConfigs.MovementSmoothSpeed);

        transform.position = stepPosition;
    }

    private void ZoomingCamera() {
        Vector3 stepPosition = Vector3.Lerp(transform.position, _newZoomPosition, _cameraConfigs.MovementSmoothSpeed);

        transform.position = stepPosition;

        SetSensitivityWithHeight();
    }

    private void OrbitingCamera() {
        switch (_cameraState) {
            case CameraState.CameraMoveingToTarget:
                OrbitingCameraSetPosition();

                if (Vector3.Distance(transform.position, _satellitePosition) < 2f) CameraNearTarget?.Invoke();
            break;

            case CameraState.CameraOrbitingAndLookingOnTarget:
                OrbitingCameraSetPosition();
            break;

            case CameraState.CameraMoveingToDefault:
                Vector3 stepPosition = Vector3.Lerp(transform.position, _defaultPosition, _cameraConfigs.MovementSmoothSpeed);

                transform.position = stepPosition;

                Quaternion stepRotation = Quaternion.Lerp(_trCamera.rotation, _defaultRotation, _cameraConfigs.RotationSmoothSpeed * 2 * Time.deltaTime);

                _trCamera.rotation = stepRotation;

                if (Vector3.Distance(transform.position, _defaultPosition) < 5f &&
                Quaternion.Angle(_trCamera.rotation, _defaultRotation) == 0f) {
                    _trCamera.rotation = _defaultRotation;
                    
                    CameraBackToDefault?.Invoke();
                }
            break;
        }
    }

    private void OrbitingCameraSetPosition() {
        Vector3 stepPosition = Vector3.Lerp(transform.position, _satellitePosition, _cameraConfigs.MovementSmoothSpeed);

        transform.position = stepPosition;

        Vector3 directionToTarget = (_targetPosition - _trCamera.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        Quaternion stepRotation = Quaternion.Lerp(_trCamera.rotation, targetRotation, _cameraConfigs.RotationSmoothSpeed * Time.deltaTime);

        _trCamera.rotation = stepRotation;
    }

    private void SetSensitivityWithHeight() {
        float percentageOfMaxHeight = transform.position.y * 100 / _cameraConfigs.MaxHeight;

        _currentSensitivityZoom = _cameraConfigs.SensitivityZoom * percentageOfMaxHeight / 100;
        _currentSensitivityMove = _cameraConfigs.SensitivityMove * percentageOfMaxHeight / 100;
    }

    private Vector3 CheckMapBorder(Vector3 cameraPosition) {
        float x = Mathf.Clamp(cameraPosition.x, _cameraConfigs.WestBorder, _cameraConfigs.EastBorder);
        float y = Mathf.Clamp (cameraPosition.y, _cameraConfigs.MinHeight, _cameraConfigs.MaxHeight);
        float z = Mathf.Clamp(cameraPosition.z, _cameraConfigs.SouthBorder, _cameraConfigs.NorthBorder);

        return new Vector3(x, y, z);
    }

    public void SetNewMovePosition(Vector3 vec3) {        
        _newMovePosition = CheckMapBorder(_newMovePosition + vec3 * _currentSensitivityMove * Time.deltaTime);
    }

    public void SetNewZoomPosition(Vector3 vec3) {
        if (_newZoomPosition.y == _cameraConfigs.MaxHeight && vec3.y > 0 || _newZoomPosition.y == _cameraConfigs.MinHeight && vec3.y < 0) return;

        _newZoomPosition = CheckMapBorder(_newZoomPosition + vec3 * _currentSensitivityZoom * Time.deltaTime);
    }

    public void SetNewTargetPosition(Vector3 vec3) {
        _targetPosition = vec3;

        Vector3 directionToCamera = (transform.position - _targetPosition).normalized;

        Vector3 satellitePosition = _targetPosition + directionToCamera * _cameraConfigs.OrbitRadius;

        Vector3 flatDirectionToSatellite = new Vector3 (
            satellitePosition.x - _targetPosition.x,
            0f,
            satellitePosition.z - _targetPosition.z
        ).normalized;

        // Satellite launch angle relative to the target
        _satelliteCurrentAngle = Mathf.Atan2(flatDirectionToSatellite.z, flatDirectionToSatellite.x) * Mathf.Rad2Deg;

        _defaultPosition = transform.position;
        _defaultRotation = _trCamera.rotation;
    }

    public void SwitchCameraState(CameraState cameraState) {
        Debug.Log("Camera state = " + cameraState); // FIX IT !

        _newMovePosition = transform.position;
        _newZoomPosition = transform.position;

        _cameraState = cameraState;
    }

    public void SetCameraMovementActive(bool isActive) {
        if (isActive) {
            _isTimerToStopMovementActive = false;

            if (!_isIECameraMoveingActive) {
                StartCoroutine(_iECameraMoveing = CameraMovementStart());
                _isIECameraMoveingActive = true;
            }
        }
        else {
            if (!_isTimerToStopMovementActive) {
                _currentTimeToStopMoveing = Time.time + _cameraConfigs.TimeToStopMoveing;
                _isTimerToStopMovementActive = true;
            } 
        }
    }

    public void SetSatelliteMovementActive(bool isActive) {
        if (isActive) {
            StartCoroutine(_iESatelliteMoveing = SatelliteMovementStart());
        }
        else {
            StopCoroutine(_iESatelliteMoveing);
        }
    }

    public bool ScreenPositionIntoRayFromCamera(Vector2 position, RaycastCheckTargetType checkType, out RaycastHit hit) {
        Ray ray = _camera.ScreenPointToRay(position);
        
        switch (checkType) {
            case RaycastCheckTargetType.CheckHexagon:
                Debug.DrawRay(ray.origin, ray.direction * _cameraConfigs.RaycastHexagonDistance, Color.red, 10f); // FIX IT !
                return Physics.Raycast(ray, out hit, _cameraConfigs.RaycastHexagonDistance, _cameraConfigs.HexagonLayer);
            // break;

            case RaycastCheckTargetType.CheckUI:
                Debug.DrawRay(ray.origin, ray.direction * _cameraConfigs.RaycastUIDistance, Color.blue, 10f); // FIX IT !
                return Physics.Raycast(ray, out hit, _cameraConfigs.RaycastUIDistance, _cameraConfigs.UILayer);
            // break;

            default: 
                hit = new RaycastHit();
                return false;
            // break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(_cameraConfigs.WestBorder, 0, _cameraConfigs.NorthBorder);
        Vector3 topRight = new Vector3(_cameraConfigs.EastBorder, 0, _cameraConfigs.NorthBorder);
        Vector3 bottomLeft = new Vector3(_cameraConfigs.WestBorder, 0, _cameraConfigs.SouthBorder);
        Vector3 bottomRight = new Vector3(_cameraConfigs.EastBorder, 0, _cameraConfigs.SouthBorder);

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
}

public interface ICameraZoom {
    public void SwitchCameraState(CameraState cameraState);
    public void SetCameraMovementActive(bool isActive);
    public void SetNewZoomPosition(Vector3 vec3);
}

public interface ICameraSatelliteMovement {
    public void SwitchCameraState(CameraState cameraState);
    public void SetCameraMovementActive(bool isActive);
    public void SetNewTargetPosition(Vector3 vec3);
    public void SetSatelliteMovementActive(bool isActive);
    public event Action CameraNearTarget;
    public event Action CameraBackToDefault;
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
    CameraMoveingToTarget,
    CameraOrbitingAndLookingOnTarget,
    CameraMoveingToDefault
}