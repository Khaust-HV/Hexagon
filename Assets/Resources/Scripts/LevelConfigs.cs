using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public sealed class LevelConfigs : ScriptableObject {
        [field: Header("Map settings")]
        [field: SerializeField] public GameObject HexagonControllerPrefab { get; private set; }
        [field: SerializeField] public float HexagonSize { get; private set; }
        [field: SerializeField] public AlgorithmOfLevelBuilding AlgorithmOfLevelBuilding { get; private set; }
        [field: SerializeField] public int NumberOfRings { get; private set; }
        
        [field: Header("Hexagon Objects settings")]
        [field: SerializeField] public GameObject HexagonObjectsControllerPrefab { get; private set; }
        [field: SerializeField] public int NumberObjectsCreatedInCaseOfShortage { get; private set; }
        [field: SerializeField] public float HexagonObjectSize { get; private set; }
        
    }
}