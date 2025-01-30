using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/CameraConfigs", fileName = "CameraConfigs")]
    public sealed class CameraConfigs : ScriptableObject {
        [field: Header("Camera raycast settings")]
        [field: SerializeField] public float RaycastUIDistance { get; private set; }
        [field: SerializeField] public LayerMask UILayer { get; private set; }
        [field: SerializeField] public float RaycastHexagonDistance { get; private set; }
        [field: SerializeField] public LayerMask HexagonLayer { get; private set; }
        [field: Space(25)]

        [field: Header("Camera move control")]
        [field: SerializeField] public float MovementSmoothSpeed { get; private set; }
        [field: SerializeField] public float RotationSmoothSpeed { get; private set; }
        [field: SerializeField] public float SensitivityMove { get; private set; }
        [field: SerializeField] public float SensitivityZoom { get; private set; }
        [field: SerializeField] public float TimeToStopMoveing { get; private set; }
        [field: Space(25)]

        [field: Header("Camera as satellite control")]
        [field: SerializeField] public float OrbitRadius { get; private set; }
        [field: SerializeField] public float OrbitHeight { get; private set; }
        [field: SerializeField] public float SatelliteSpeed { get; private set; }
        [field: Space(25)]
        
        [field: Header("Camera map borders")]
        [field: SerializeField] public float MaxHeight { get; private set; }
        [field: SerializeField] public float MinHeight { get; private set; }
        [field: SerializeField] public float WestBorder { get; private set; }
        [field: SerializeField] public float EastBorder { get; private set; }
        [field: SerializeField] public float NorthBorder { get; private set; }
        [field: SerializeField] public float SouthBorder { get; private set; }
        [field: Space(25)]
        
        [field: Header("LODGroup settings")]
        [field: SerializeField] public float LODObjectSize { get; private set; }
        [field: Range(0,1)]
        [field: SerializeField] public float LOD0Distance { get; private set; }
        [field: Range(0,1)]
        [field: SerializeField] public float LOD1Distance { get; private set; }
        [field: Range(0,1)]
        [field: SerializeField] public float LOD2Distance { get; private set; }
    }
}