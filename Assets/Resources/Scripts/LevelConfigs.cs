using UnityEngine;
using Managers;

namespace GameConfigs {
    [CreateAssetMenu(menuName = "Configs/LevelConfigs", fileName = "LevelConfigs")]
    public sealed class LevelConfigs : ScriptableObject {
        [field: Header("Map settings")]
        [field: SerializeField] public float SizeAllObject { get; private set; }
        [field: SerializeField] public Texture2D DefaultDestroyTextureParticle { get; private set; }
        [field: SerializeField] public float DefaultDestroySizeParticles { get; private set; }
        [field: SerializeField] public float DefaultSpawnTimeAllObject { get; private set; }
        [field: SerializeField] public float DefaultDestroyTimeAllObject { get; private set; }
        [field: SerializeField] public float DefaultHologramSpawnTimeAllObject { get; private set; }
        [field: SerializeField] public AlgorithmOfLevelBuilding AlgorithmOfLevelBuilding { get; private set; }
        [field: SerializeField] public int NumberOfRings { get; private set; }
        [field: Space(25)]
        
        [field: Header("Hexagon and HexagonObjects settings")]
        [field: SerializeField] public GameObject HexagonControllerPrefab { get; private set; }
        [field: SerializeField] public GameObject HexagonObjectsControllerPrefab { get; private set; }
        [field: SerializeField] public int NumberObjectsCreatedInCaseOfShortage { get; private set; }
    }
}