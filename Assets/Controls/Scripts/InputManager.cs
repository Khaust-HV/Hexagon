using UnityEngine;
using InputSystemActions;
using Zenject;

public sealed class InputManager : MonoBehaviour
{
    private TouchscreenInputActions _touchscreenInputActions;
    private CameraState _cameraState;
    private bool _isTimerStaticActive;
    private float _oldDistanceTouchPosition;

    private IPlayerManagerInput _iPlayerManagerInput;
    private ICameraMove _iCameraMove;

    [Inject]
    private void Construct(IPlayerManagerInput iPlayerManagerInput, ICameraMove iCameraMove) {
        _iPlayerManagerInput = iPlayerManagerInput;
        _iCameraMove = iCameraMove;

        transform.SetParent(GameObject.Find("Managers").transform);
    }

    private void Start() {
        _touchscreenInputActions = new TouchscreenInputActions(new InputMap());

        _touchscreenInputActions.TapPosition += TapOnScreenPosition;

        _touchscreenInputActions.FirstTouchIsActive += SetCameraMoveActive;
        _touchscreenInputActions.SingleSwipeDelta += CameraMove;

        _touchscreenInputActions.SecondTouchIsActive += SetCameraZoomActive;
        _touchscreenInputActions.DoubleTouchPositions += CameraZoom;
        
        SetGameplayInputActionMapActive(true);
    }

    #region GameplayInputActionMap

        private void SetGameplayInputActionMapActive(bool isActive) {
            _touchscreenInputActions.SetGameplayInputActive(isActive);
        }

        private void TapOnScreenPosition(Vector2 position) {
            _iPlayerManagerInput.TapPositionCheck(position);
        }

        private void SetCameraMoveActive(bool isActive) {
            if (isActive) {
                if (_isTimerStaticActive) _iCameraMove.SetTimerStaticActive(_isTimerStaticActive = false);

                _iCameraMove.SwitchCameraState(_cameraState = CameraState.CameraMoveing);
            }
            else {
                if (_cameraState == CameraState.CameraMoveing) _iCameraMove.SetTimerStaticActive(_isTimerStaticActive = true);
            }
        }

        private void SetCameraZoomActive(bool isActive) {
            if (isActive) {
                if (_isTimerStaticActive) _iCameraMove.SetTimerStaticActive(_isTimerStaticActive = false);

                _oldDistanceTouchPosition = 0f;

                _iCameraMove.SwitchCameraState(_cameraState = CameraState.CameraZooming);
            }
            else {
                if (_cameraState == CameraState.CameraZooming) _iCameraMove.SetTimerStaticActive(_isTimerStaticActive = true);
            }
        }

        private void CameraMove(Vector2 vec2) {
            Vector3 position =  new Vector3(vec2.x, 0f, vec2.y);

            _iCameraMove.SetNewMovePosition(position);
        }

        private void CameraZoom(Vector2 firstVec2, Vector2 secondVec2) {
            float correntTouchDistance = Vector2.Distance(firstVec2, secondVec2);

            if (_oldDistanceTouchPosition == 0f) {
                _oldDistanceTouchPosition = correntTouchDistance;
                return;
            }
            
            Vector3 position;

            if (correntTouchDistance > _oldDistanceTouchPosition) {
                position = new Vector3(0f, -1f, 1f);
            }
            else {
                position = new Vector3(0f, 1f, -1f);
            }

            _iCameraMove.SetNewZoomPosition(position);

            _oldDistanceTouchPosition = correntTouchDistance;
        }
    #endregion
}