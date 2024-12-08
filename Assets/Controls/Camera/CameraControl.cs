using UnityEngine;

public class CameraControl : MonoBehaviour, ICameraMove
{
    [Header("Camera move control")]
    [SerializeField] float _smoothSpeed;
    [SerializeField] float _timeToCameraStatic;
    [SerializeField] private float _sensitivityMove;
    [SerializeField] private float _sensitivityZoom;
    [Header("Camera height borders")]
    [SerializeField] float _maxHeight;
    [SerializeField] float _minHeight;
    [Header("Camera map borders")]
    [SerializeField] float _westBorder;
    [SerializeField] float _eastBorder;
    [SerializeField] float _northBorder;
    [SerializeField] float _southBorder;

    private CameraState _cameraState;
    private bool _isTimerStatic;
    private float _timeCameraStatic;
    private float _correntSensitivityMove;
    private float _correntSensitivityZoom;

    private Vector3 _newMovePosition;
    private Vector3 _newZoomPosition;

    private void Awake() {
        _newMovePosition = transform.position;
        _newZoomPosition = transform.position;

        SetSensitivityWithHeight();
    }

    private void Update() {
        if (_isTimerStatic && Time.time > _timeCameraStatic) _cameraState = CameraState.CameraOnStatic;

        switch (_cameraState) {
            case CameraState.CameraOnStatic:
                return;
            //break;

            case CameraState.CameraMove:
                MoveCamera();
            break;

            case CameraState.CameraZoom:
                ZoomCamera();
            break;
            
            case CameraState.CameraLooked:
                //Camera looks on the lookTarget
            break;
        }
    }

    private void MoveCamera() {
        Vector3 newPosition = Vector3.Lerp(transform.position, _newMovePosition, _smoothSpeed);

        newPosition = CheckMapBorder(newPosition);

        transform.position = newPosition;
    }

    private void ZoomCamera() {
        Vector3 newPosition = Vector3.Lerp(transform.position, _newZoomPosition, _smoothSpeed);
        
        newPosition = CheckMapBorder(newPosition);

        transform.position = newPosition;

        SetSensitivityWithHeight();
    }

    private void SetSensitivityWithHeight() {
        float percentageOfMaxHeight = transform.position.y * 100 / _maxHeight;

        _correntSensitivityZoom = _sensitivityZoom * percentageOfMaxHeight / 100;
        _correntSensitivityMove = _sensitivityMove * percentageOfMaxHeight / 100;
    }

    private Vector3 CheckMapBorder(Vector3 cameraPosition) {
        float x = Mathf.Clamp(cameraPosition.x, _westBorder, _eastBorder);
        float y = Mathf.Clamp (cameraPosition.y, _minHeight, _maxHeight);
        float z = Mathf.Clamp(cameraPosition.z, _southBorder, _northBorder);

        return new Vector3(x, y, z);
    }

    public void SetNewMovePosition(Vector3 vec3) {
        _newMovePosition += vec3 * _correntSensitivityMove * Time.deltaTime;
    }

    public void SetNewZoomPosition(Vector3 vec3) {
        float heightValue = transform.position.y;

        if (vec3.y > 0f && heightValue != _maxHeight || vec3.y < 0f && heightValue != _minHeight) {
            _newZoomPosition += vec3 * _correntSensitivityZoom * Time.deltaTime;
        }
    }

    public void SwitchCameraAction(CameraState cameraState) {
        _cameraState = cameraState;

        _newMovePosition = transform.position;
        _newZoomPosition = transform.position;
    }

    public void TimerStaticOnEnable(bool onEnable) {
        if (onEnable) {
            _timeCameraStatic = Time.time + _timeToCameraStatic;

            _isTimerStatic = true;
        }
        else {
            _isTimerStatic = false;
        }
    }

    private void OnDrawGizmos()
    {
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
    public void SwitchCameraAction(CameraState cameraAction);
    public void SetNewMovePosition(Vector3 vec3);
    public void SetNewZoomPosition(Vector3 vec3);
    public void TimerStaticOnEnable(bool onEnable);
}

public enum CameraState {
    CameraOnStatic,
    CameraMove,
    CameraZoom,
    CameraLooked
}