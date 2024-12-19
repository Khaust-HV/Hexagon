using Hexagon;
using System;
using UnityEngine;
using Zenject;

public sealed class PlayerManager : IPlayerManagerInput, IInteractingWithObject, IDisposable {
    private PlayerState _playerState;

    #region DI
        private ICameraRaycast _iCameraRaycast;
        private ICameraSatelliteMovement _iCameraSatelliteMovement;
        private IHexagonTarget _iHexagonTarget;
        private ISwitchGameplayInput _iSwitchInput;
    #endregion

    private int _currentTargetID;

    [Inject]
    private void Construct (
        ICameraRaycast iCameraRaycast, 
        ICameraSatelliteMovement iCameraSatelliteMovement, 
        IHexagonTarget iHexagonTarget, 
        ISwitchGameplayInput iSwitchInput
        ) {
        _iCameraRaycast = iCameraRaycast;
        _iCameraSatelliteMovement = iCameraSatelliteMovement;
        _iHexagonTarget = iHexagonTarget;
        _iSwitchInput = iSwitchInput;

        _iCameraSatelliteMovement.CameraNearTarget += CameraNearTarget;
        _iCameraSatelliteMovement.CameraBackToDefault += CameraBackToDefault;
    }

    public void Dispose() {
        _iCameraSatelliteMovement.CameraNearTarget -= CameraNearTarget;
        _iCameraSatelliteMovement.CameraBackToDefault -= CameraBackToDefault;
    }

    public void TapPositionCheck(Vector2 position) {
        RaycastHit hit;

        switch (_playerState) {
            case PlayerState.FreeMovementOnMap:
                if (_iCameraRaycast.ScreenPositionIntoRayFromCamera(position, RaycastCheckTargetType.CheckUI, out hit)){
                    Debug.Log($"Tap checking successful [{hit.collider.name}]"); // FIX IT !

                    //Press button?
                }
                else if (_iCameraRaycast.ScreenPositionIntoRayFromCamera(position, RaycastCheckTargetType.CheckHexagon, out hit)) {
                    Debug.Log($"Tap checking successful [{hit.collider.name}]"); // FIX IT !

                    if (_iHexagonTarget.IsMakeThisHexagonAsTarget(hit.collider.GetComponent<HexagonUnitAreaControl>().HexagonID)){
                        Debug.Log("Hexagon suitable how target");

                        _currentTargetID = hit.collider.GetComponent<HexagonUnitAreaControl>().HexagonID;

                        StartChoosingToBuildOrImprove(hit.collider.transform.position);
                    }
                }
                else Debug.Log("Tap checking failed"); // FIX IT !
            break;

            case PlayerState.ChoosingToBuildOrImprove:
                if (_iCameraRaycast.ScreenPositionIntoRayFromCamera(position, RaycastCheckTargetType.CheckUI, out hit)){
                    Debug.Log($"Tap checking successful [{hit.collider.name}]"); // FIX IT !

                    //Press button?
                }
                else {
                    Debug.Log("Tap checking failed"); // FIX IT !
                    CancelChoosingToBuildOrImprove(); // FIX IT !
                }
            break; 
        }        
    }

    private void StartChoosingToBuildOrImprove(Vector3 position) {
        _iHexagonTarget.SetThisHexagonTargetActive(_currentTargetID, true);

        _iSwitchInput.SetAllGameplayActive(false);
        _iCameraSatelliteMovement.SetNewTargetPosition(position);
        _iCameraSatelliteMovement.SetSatelliteMovementActive(true);
        _iCameraSatelliteMovement.SwitchCameraState(CameraState.CameraMoveingToTarget);
        _iCameraSatelliteMovement.SetCameraMovementActive(true);
    }

    public void CancelChoosingToBuildOrImprove() {
        _iHexagonTarget.SetThisHexagonTargetActive(_currentTargetID, false);

        SwitchPlayerState(PlayerState.WaitingForTheEventToEnd);
        _iSwitchInput.SetAllGameplayActive(false);
        _iCameraSatelliteMovement.SwitchCameraState(CameraState.CameraMoveingToDefault);
        _iCameraSatelliteMovement.SetSatelliteMovementActive(false);
    }

    private void SwitchPlayerState(PlayerState playerState) {
        Debug.Log("Player state = " + playerState); // FIX IT !

        _playerState = playerState;
    }

    private void CameraNearTarget() {
        SwitchPlayerState(PlayerState.ChoosingToBuildOrImprove);
        _iSwitchInput.SetTapOnScreenActive(true);
        _iCameraSatelliteMovement.SwitchCameraState(CameraState.CameraOrbitingAndLookingOnTarget);
    }

    private void CameraBackToDefault() {
        SwitchPlayerState(PlayerState.FreeMovementOnMap);
        _iSwitchInput.SetAllGameplayActive(true);
        _iCameraSatelliteMovement.SwitchCameraState(CameraState.CameraMoveing);
    }
}

public interface IPlayerManagerInput {
    public void TapPositionCheck(Vector2 position);
}

public interface IInteractingWithObject {
    public void CancelChoosingToBuildOrImprove();
}

public enum PlayerState {
    FreeMovementOnMap,
    ChoosingToBuildOrImprove,
    WaitingForTheEventToEnd
}