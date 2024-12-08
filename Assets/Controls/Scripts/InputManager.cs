using UnityEngine;
using InputSystemActions;
using Zenject;

public sealed class InputManager : MonoBehaviour
{
    private TouchscreenInputActions _touchscreenInputActions;
    private CameraState _cameraState;
    private bool _isCameraStaticStarted;
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

        _touchscreenInputActions.FirstTouchActive += CameraMoveOnEnable;
        _touchscreenInputActions.SingleSwipeDelta += CameraMove;

        _touchscreenInputActions.SecondTouchActive += CameraZoomOnEnable;
        _touchscreenInputActions.DoubleTouchPositions += CameraZoom;
        
        GameplayInputActionMapOnEnable(true);
    }

    #region GameplayInputActionMap

        private void GameplayInputActionMapOnEnable(bool onEnable) {
            _touchscreenInputActions.GameplayInputOnEnable(onEnable);
        }

        private void TapOnScreenPosition(Vector2 position) {
            _iPlayerManagerInput.TapPositionCheck(position);
        }

        private void CameraMoveOnEnable(bool onEnable) {
            if (onEnable) {
                if (_isCameraStaticStarted) _iCameraMove.TimerStaticOnEnable(_isCameraStaticStarted = false);

                _iCameraMove.SwitchCameraAction(_cameraState = CameraState.CameraMove);
            }
            else {
                if (_cameraState == CameraState.CameraMove) _iCameraMove.TimerStaticOnEnable(_isCameraStaticStarted = true);
            }
        }

        private void CameraZoomOnEnable(bool onEnable) {
            if (onEnable) {
                if (_isCameraStaticStarted) _iCameraMove.TimerStaticOnEnable(_isCameraStaticStarted = false);

                _oldDistanceTouchPosition = 0f;

                _iCameraMove.SwitchCameraAction(_cameraState = CameraState.CameraZoom);
            }
            else {
                if (_cameraState == CameraState.CameraZoom) _iCameraMove.TimerStaticOnEnable(_isCameraStaticStarted = true);
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