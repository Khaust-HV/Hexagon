using UnityEngine;
using InputSystemActions;
using System;
using Zenject;
using CameraControl;

namespace Managers {
    public sealed class InputManager : ISwitchGameplayInput, IDisposable {
        private float _oldDistanceTouchPosition;
        private CameraState _cameraState;

        private TouchscreenInputActions _touchscreenInputActions;

        #region DI
            private IPlayerManagerInput _iPlayerManagerInput;
            private ICameraMove _iCameraMove;
            private ICameraZoom _iCameraZoom;
        #endregion

        [Inject]
        private void Construct(IPlayerManagerInput iPlayerManagerInput, ICameraMove iCameraMove, ICameraZoom iCameraZoom) {
            // Set DI
            _iPlayerManagerInput = iPlayerManagerInput;
            _iCameraMove = iCameraMove;
            _iCameraZoom = iCameraZoom;

            // Set component
            _touchscreenInputActions = new TouchscreenInputActions(new InputMap());
        }

        public void Dispose() {
            SetAllGameplayActive(false);
            SetGameplayInputActionMapActive(false);
        }

        #region GameplayInputSwitch
            public void SetAllGameplayActive(bool isActive) {
                if (isActive) {
                    _touchscreenInputActions.TapPosition += TapOnScreenPosition;

                    _touchscreenInputActions.FirstTouchActive += SetCameraMoveActive;
                    _touchscreenInputActions.SingleSwipeDelta += CameraMove;

                    _touchscreenInputActions.SecondTouchActive += SetCameraZoomActive;
                    _touchscreenInputActions.DoubleTouchPositions += CameraZoom;
                } 
                else {
                    _touchscreenInputActions.TapPosition -= TapOnScreenPosition;

                    _touchscreenInputActions.FirstTouchActive -= SetCameraMoveActive;
                    _touchscreenInputActions.SingleSwipeDelta -= CameraMove;

                    _touchscreenInputActions.SecondTouchActive -= SetCameraZoomActive;
                    _touchscreenInputActions.DoubleTouchPositions -= CameraZoom;
                }
            }

            public void SetMoveAndZoomActive(bool isActive) {
                if (isActive) {
                    _touchscreenInputActions.FirstTouchActive += SetCameraMoveActive;
                    _touchscreenInputActions.SingleSwipeDelta += CameraMove;

                    _touchscreenInputActions.SecondTouchActive += SetCameraZoomActive;
                    _touchscreenInputActions.DoubleTouchPositions += CameraZoom;
                } 
                else {
                    _touchscreenInputActions.FirstTouchActive -= SetCameraMoveActive;
                    _touchscreenInputActions.SingleSwipeDelta -= CameraMove;

                    _touchscreenInputActions.SecondTouchActive -= SetCameraZoomActive;
                    _touchscreenInputActions.DoubleTouchPositions -= CameraZoom;
                }
            }

            public void SetTapOnScreenActive(bool isActive) {
                if (isActive) {
                    _touchscreenInputActions.TapPosition += TapOnScreenPosition;
                } 
                else {
                    _touchscreenInputActions.TapPosition -= TapOnScreenPosition;
                }
            }
        #endregion

        #region GameplayInputActionMap
            public void SetGameplayInputActionMapActive(bool isActive) {
                _touchscreenInputActions.SetGameplayInputActive(isActive);
            }

            private void TapOnScreenPosition(Vector2 position) {
                _iPlayerManagerInput.TapPositionCheck(position);
            }

            private void SetCameraMoveActive(bool isActive) {
                if (isActive) {
                    _iCameraMove.SwitchCameraState(_cameraState = CameraState.CameraMoveing);
                    _iCameraMove.SetCameraMovementActive(true);
                }
                else {
                    if (_cameraState == CameraState.CameraMoveing) _iCameraMove.SetCameraMovementActive(false);
                }
            }

            private void SetCameraZoomActive(bool isActive) {
                if (isActive) {
                    _oldDistanceTouchPosition = 0f;
                    _iCameraZoom.SwitchCameraState(_cameraState = CameraState.CameraZooming);
                    _iCameraZoom.SetCameraMovementActive(true);
                }
                else {
                    if (_cameraState == CameraState.CameraZooming) _iCameraZoom.SetCameraMovementActive(false);
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

                _iCameraZoom.SetNewZoomPosition(position);

                _oldDistanceTouchPosition = correntTouchDistance;
            }
        #endregion
    }
}

public interface ISwitchGameplayInput {
    public void SetGameplayInputActionMapActive(bool isActive);
    public void SetAllGameplayActive(bool isActive);
    public void SetMoveAndZoomActive(bool isActive);
    public void SetTapOnScreenActive(bool isActive);
}