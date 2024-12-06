using UnityEngine;
using InputSystemActions;

public sealed class InputManager : MonoBehaviour
{
    private TouchscreenInputActions _touchscreenInputActions;
    private ICameraControl _iCameraControl;
    private CameraAction _cameraAction;
    private bool _isCameraStaticStarted;
    private float _oldDistanceTouchPosition;

    private void Awake() {
        _iCameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();

        _touchscreenInputActions = new TouchscreenInputActions(new InputMap());

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

        private void CameraMoveOnEnable(bool onEnable) {
            if (onEnable) {
                if (_isCameraStaticStarted) _iCameraControl.CameraStaticOnEnable(_isCameraStaticStarted = false);

                _iCameraControl.SwitchCameraAction(_cameraAction = CameraAction.CameraMove);
            }
            else {
                if (_cameraAction == CameraAction.CameraMove) _iCameraControl.CameraStaticOnEnable(_isCameraStaticStarted = true);
            }
        }

        private void CameraZoomOnEnable(bool onEnable) {
            if (onEnable) {
                if (_isCameraStaticStarted) _iCameraControl.CameraStaticOnEnable(_isCameraStaticStarted = false);

                _oldDistanceTouchPosition = 0f;

                _iCameraControl.SwitchCameraAction(_cameraAction = CameraAction.CameraZoom);
            }
            else {
                if (_cameraAction == CameraAction.CameraZoom) _iCameraControl.CameraStaticOnEnable(_isCameraStaticStarted = true);
            }
        }

        private void CameraMove(Vector2 vec2) {
            Vector3 position =  new Vector3(vec2.x, 0f, vec2.y);

            _iCameraControl.SetNewMovePosition(position);
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

            _iCameraControl.SetNewZoomPosition(position);

            _oldDistanceTouchPosition = correntTouchDistance;
        }
    #endregion
}