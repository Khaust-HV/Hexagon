using UnityEngine;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public sealed class LevelConfigs : ScriptableObject
    {
        [field: SerializeField] public GameObject HexagonPrefab { get; private set; }
        [field: SerializeField] public float HexagonSize { get; private set; }
        [field: SerializeField] public AlgorithmOfLevelBuilding AlgorithmOfLevelBuilding { get; private set; }
        [field: SerializeField] public int NumberOfRings { get; private set; }
    }
}