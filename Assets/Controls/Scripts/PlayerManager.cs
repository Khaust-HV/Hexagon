using UnityEngine;
using Zenject;

public sealed class PlayerManager : MonoBehaviour, IPlayerManagerInput
{
    private PlayerState _playerState;

    private ICameraRaycast _iCameraRaycast;
    private IHexagonTarget _iHexagonTarget;

    [Inject]
    private void Construct(ICameraRaycast iCameraRaycast, IHexagonTarget iHexagonTarget) {
        _iCameraRaycast = iCameraRaycast;
        _iHexagonTarget= iHexagonTarget;

        transform.SetParent(GameObject.Find("Managers").transform);
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

                    if (_iHexagonTarget.SetHexagonTarget(hit.collider.GetComponent<HexagonControl>().HexagonID)){
                        Debug.Log("Hexagon suitable how target");
                    }
                }
                else Debug.Log("Tap checking failed"); // FIX IT !
            break;

            case PlayerState.ChoosingToBuildOrImprove:
                if (_iCameraRaycast.ScreenPositionIntoRayFromCamera(position, RaycastCheckTargetType.CheckUI, out hit)){
                    Debug.Log($"Tap checking successful [{hit.collider.name}]"); // FIX IT !

                    //Press button?
                }
                else Debug.Log("Tap checking failed"); // FIX IT !
            break; 
        }        
    }
}

public interface IPlayerManagerInput {
    public void TapPositionCheck(Vector2 position);
}

public enum PlayerState {
    FreeMovementOnMap,
    ChoosingToBuildOrImprove
}